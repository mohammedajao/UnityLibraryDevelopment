using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TimeWizard;
using TimeWizard.Persistence;

namespace TimeWizard.Core
{
    public class SaveController
    {
        private readonly ISaveLoader _loader;

        private readonly IRegistry<ISaveStore> _storeRegistry;
        private readonly IRegistry<ISaveInterpreter> _interpreterRegistry;

        private Chunk[] _snapshot;

        private Chunk[] ProcessInterpreters(Chunk[] saveChunks)
        {
            foreach(var interpreter in _interpreterRegistry.List())
            {
                if(!interpreter.IsDirty()) continue;
                saveChunks = interpreter.ApplyModifications(saveChunks);
            }
            return saveChunks;
        }

        private void ProcessSnapshotUpdate()
        {
            foreach(var store in _storeRegistry.List())
            {
                var id = store.GetIdentifier();
                foreach(var saveChunk in _snapshot)
                {
                    if(saveChunk.TryGetValue(id, out var json))
                    {
                        var segment = new ChunkDataSegment(json);
                        store.LoadChunkData(saveChunk.Name, segment);
                    }
                }
            }

            foreach(var interpreter in _interpreterRegistry.List())
            {
                interpreter.ProcessChunks(_snapshot);
            }
        }

        public void CaptureSnapshot(bool overwriteChunks = false) // Adds the data of different stores to our global, area, and scene chunks
        {
            var saveChunks = new Dictionary<string, Chunk>();
            foreach(var store in _storeRegistry.List())
            {
                var id = store.GetIdentifier();
                var states = store.FetchSaveStates();
                foreach(var state in states)
                {
                    if(state == null) continue;
                    Chunk saveChunk;
                    if(!saveChunks.TryGetValue(state.ChunkName, out saveChunk))
                    {
                        saveChunk = new Chunk(state.ChunkName);
                        saveChunks.Add(saveChunk.Name, saveChunk);
                    }
                    saveChunk.AddToChunk(id, state.Data, overwriteChunks);
                    saveChunks[saveChunk.Name] = saveChunk;
                }
            }

            _snapshot = saveChunks.Select((pair) => pair.Value).ToArray();

            foreach (var interpreter in _interpreterRegistry.List()) {
                interpreter.ProcessChunks(_snapshot);
            }
        }

        public void ApplySnapshot(SaveContainer container)
        {
            if (_snapshot == null) return;
            Exception ex;
            var snapshot = ProcessInterpreters(_snapshot); // Apply dirty chunks
            if(_loader.TrySave(container.Name, snapshot, out ex))
            {
                Debug.Log($"[TimeWizard] Successfully saved {container.Name}.");
                ProcessSnapshotUpdate(); // Sync
            }
            if(ex != null)
                Debug.Log($"{ex}");
        }

        public void LoadSnapshot(SaveContainer container)
        {
            Debug.Log("[TimeWizard] Attempting to load...");
            Exception ex;
            if(_loader.TryLoad(container.Name, out var chunks, out ex))
            {
                Debug.Log($"[TimeWizard] Successfully loaded save instance: {container.Name}");
                _snapshot = chunks;
                ProcessSnapshotUpdate();
            }
            if(ex != null)
                Debug.LogError($"{ex}");
        }

        public SaveController(
            IRegistry<ISaveStore> storeRegistry = null,
            IRegistry<ISaveInterpreter> interpreterRegistry = null,
            ISaveLoader loader = null
        )
        {
            _loader = loader ?? new RuntimeSaveLoader(); // Turn to in-mem loader later
            _storeRegistry = storeRegistry ?? new RegistryBase<ISaveStore>();
            _interpreterRegistry = interpreterRegistry ?? new RegistryBase<ISaveInterpreter>();

            _storeRegistry.OnItemRegistered += OnStoreItemRegistered;
            _interpreterRegistry.OnItemRegistered += OnInterpreterItemRegistered;
        }

        private void OnStoreItemRegistered(ISaveStore item)
        {
            var id = item.GetIdentifier();
            if(_snapshot == null) return;
            foreach(var chunk in _snapshot)
            {
                if(chunk.TryGetValue(id, out var json))
                {
                    var segment = new ChunkDataSegment(json);
                    item.LoadChunkData(chunk.Name, segment);
                }
            }
        }

        private void OnInterpreterItemRegistered(ISaveInterpreter item)
        {
            if(_snapshot == null) return;
            item.ProcessChunks(_snapshot);
        }
    }
}