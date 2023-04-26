using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using TimeWizard;

namespace TimeWizard.UnityEditor
{
    public class ChunksListInterpreter : ITimeWizardInterpreter
    {
        [SerializeField] private Chunk[] _chunks = new Chunk[0];
        private bool _isDirty = false;
        private Dictionary<int, List<string>> tracking = new();
        public ChunksListAsset ViewableChunks;

        private SerializedObject _serializedChunksAsset;
        private SerializedProperty _serializedChunksList;
        
        public class ChunksListAsset : ScriptableObject
        {
            [SerializeField] public List<Chunk> chunks = new();
        }

        public void OnEnable()
        {
            ViewableChunks = ScriptableObject.CreateInstance<ChunksListAsset>();
            ViewableChunks.chunks = _chunks.ToList();
            _serializedChunksAsset = new SerializedObject(ViewableChunks);
            _serializedChunksList = _serializedChunksAsset.FindProperty("chunks");
        }

        public void OnDisable()
        {
            ViewableChunks = null;
            _serializedChunksAsset = null;
            _serializedChunksList = null;
        }

        public void DrawInspectorGUI()
        {
            string label = $"Data{(_isDirty ? "*" : "")}";
            EditorGUILayout.BeginVertical();
            if(ViewableChunks != null && _serializedChunksAsset != null && _serializedChunksList != null)
            {
                EditorGUILayout.PropertyField(_serializedChunksList, new GUIContent(label), true);
            }
            EditorGUILayout.EndVertical();
            // _serializedChunksAsset.ApplyModifiedProperties();
        }

        public bool IsDirty() => _isDirty;
        public void ProcessChunks(Chunk[] saveChunks)
        {
            tracking.Clear();
            _chunks = saveChunks;
            _isDirty = false;
            OnEnable();
        }
        public Chunk[] ApplyModifications(Chunk[] saveChunks)
        {
            foreach(var entry in tracking)
            {
                var dirtyChunk = saveChunks[entry.Key];
                foreach(var saveChunk in saveChunks)
                {
                    if(saveChunk.Name == dirtyChunk.Name)
                    {
                        foreach(var modifiedKey in entry.Value)
                        {
                            if(saveChunk.storage.ContainsKey(modifiedKey))
                            {
                                saveChunk.storage[modifiedKey] = dirtyChunk.storage[modifiedKey];
                            }
                        }
                    }
                }
            }
            return saveChunks;
        }
    }
}
