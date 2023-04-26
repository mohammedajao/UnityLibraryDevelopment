using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard.Core;

namespace TimeWizard.Persistence
{
    public class RuntimeSaveLoader : ISaveLoader
    {
        public readonly List<SaveContainer> _containers = new List<SaveContainer>();
        public readonly Dictionary<string, Chunk[]> _chunks = new Dictionary<string, Chunk[]>();

        public SaveContainer[] ListSaves() => _containers.ToArray();

        public bool TrySave(string name, Chunk[] chunks, out Exception ex)
        {
            ex = null;
            if(!_chunks.ContainsKey(name))
            {
                _chunks.Add(name, chunks);
                _containers.Add(new SaveContainer() { Name = name, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                return true;
            } else {
                if(_chunks.TryGetValue(name, out _))
                {
                    for (int i = 0; i < _containers.Count; i++)
                    {
                        var container = _containers[i];
                        if(container.Name == name)
                        {
                            container.UpdatedAt = DateTime.UtcNow;
                            break;
                        }
                    }
                    _chunks[name] = chunks;
                    return true;
                }
            }
            return false;
        }

        public bool TryClear(string name, out Exception ex)
        {
            ex = null;
            if(!_chunks.ContainsKey(name))
            {
                return true;
            }
            _chunks.Remove(name);
            return true;
        }

        public bool TryLoad(string name, out Chunk[] chunks, out Exception ex)
        {
            ex = null;
            chunks = new Chunk[0];
            if(_chunks.TryGetValue(name, out chunks)) return true;
            return false;
        }
    }
}
