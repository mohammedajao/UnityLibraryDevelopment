using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TimeWizard.Core;
using TimeWizard.Persistence;

namespace TimeWizard
{
    [Serializable]
    public struct LocationData
    {
        [SerializeField] public string ScenePath;
    }

    public class SaveContext : ISaveInterpreter
    {
        public readonly string Name = "SaveContext";
    
        public LocationData Location;
        private SaveController _controller;
        private SaveContainer _saveContainer;

        public SaveManager Manager => SaveManager.Instance;

        private Chunk[] _chunks = new Chunk[0];

        private bool _isDirty = true;
        public bool IsDirty() => _isDirty;

        // private bool _showSaveMetadata = false;
        public string JSON { get; private set; }

        // public void DrawInspectorGUI()
        // {
        //     string Label = $"Save Metadata";
        //     _showSaveMetadata = EditorGUILayout.Foldout(_showSaveMetadata, Label);

        //     if(_showSaveMetadata)
        //     {
        //         EditorGUI.indentLevel++;
        //         EditorGUILayout.SelectableLabel(json, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        //         EditorGUI.indentLevel--;
        //     }
        // }

        public void UpdateSceneLocation(string activeScene)
        {
            Location.ScenePath = activeScene;
            _chunks = ApplyModifications(_chunks);
            Manager.UpdateSnapshot(_chunks);
            _isDirty = true;
        }

        public Chunk[] ApplyModifications(Chunk[] saveChunks)
        {
            foreach(var saveChunk in saveChunks)
            {
                if(saveChunk.Name.ToLower() == "global")
                {
                    if(Location.ScenePath != null)
                        saveChunk.storage[Name] = JsonUtility.ToJson(Location);
                }
            }
            _isDirty = false;
            return saveChunks;
        }

        public void ProcessChunks(Chunk[] saveChunks)
        {
            _chunks = saveChunks;
            foreach(var saveChunk in saveChunks)
            {
                if(saveChunk.Name.ToLower() == "global")
                {
                    if(saveChunk.TryGetValue(Name, out var _json))
                    {
                        JSON = _json;
                        Location = JsonUtility.FromJson<LocationData>(JSON);
                    }
                }
            }
        }

        public Task<SaveContext> Create()
        {
            _controller.LoadSnapshot(_saveContainer);
            return Task.FromResult(this);
        }
        
        public SaveContext(string _saveId, SaveController controller)
        {
            _controller = controller;
            _saveContainer = new SaveContainer() { Name = _saveId };
            Location.ScenePath = SceneManager.GetActiveScene().path;
            JSON = JsonUtility.ToJson(Location);
        }

    }
}
