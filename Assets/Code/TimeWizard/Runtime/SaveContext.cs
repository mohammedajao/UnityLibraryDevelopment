using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using TimeWizard.Core;
using TimeWizard.Persistence;

namespace TimeWizard
{
    [Serializable]
    public struct LocationData
    {
        [SerializeField] public string ScenePath;
    }

    public class SaveContext : ISaveStore
    {
        public readonly string Name = "SaveContext";
    
        public LocationData Location;
        private SaveController _controller;
        private SaveContainer _saveContainer;

        public SaveContext(string _saveId, SaveController controller, IRegistry<ISaveStore> storeRegistry)
        {
            _controller = controller;
            _saveContainer = new SaveContainer() { Name = _saveId };
            storeRegistry.Register(this);
        }

        public Task<SaveContext> Create()
        {
            _controller.LoadSnapshot(_saveContainer);
            return Task.FromResult(this);
        }

        public string GetIdentifier()
        {
            return Name;
        }

        public void LoadChunkData(string chunkName, ChunkDataSegment info)
        {
            var state = info.As<LocationData>();
            Location.ScenePath = state.ScenePath;
        }

        public List<SaveState> FetchSaveStates()
        {
            return new List<SaveState> {
                new SaveState()
                {
                    ChunkName = "Global",
                    Data = new LocationData() {
                        ScenePath = SceneManager.GetActiveScene().path
                    }
                }
            };
        }
    }
}
