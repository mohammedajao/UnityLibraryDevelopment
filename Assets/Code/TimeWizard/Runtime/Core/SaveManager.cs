using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard.Persistence;
using TimeWizard;

namespace TimeWizard.Core
{
    // Maybe add IGameService interface?
    // TODO: Add events for TimeWizard inspector
    public class SaveManager
    {
        private ISaveLoader _loader;
        private IRegistry<ISaveStore> _storeRegistry = new RegistryBase<ISaveStore>();
        private IRegistry<ISaveInterpreter> _interpreterRegistry = new RegistryBase<ISaveInterpreter>();
        private SaveController controller;

        private static SaveManager _instance = null;
        private static readonly object _lock = new object();

        SaveManager()
        {
            _loader = new FileSaveLoader("TestSaves");
            controller = new SaveController(_storeRegistry, _interpreterRegistry, _loader);
        }

        public static SaveManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(_lock)
                    {
                        if(_instance == null) 
                        {
                            _instance = new SaveManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public SaveContext GetSaveController(string saveId)
        {
            return new SaveContext(saveId, controller, _storeRegistry);
        }
    
        // protected override void SingletonAwake() {
        //     _loader = new FileSaveLoader("BaseDevSaves"); // Later app/GM singleton should provide context with inline bootstrap unity attr
        //     controller = new SaveController(_storeRegistry, _interpreterRegistry, _loader);
        // }

        public int IRSize => _interpreterRegistry.List().Length;
        public void Register(ISaveStore store) => _storeRegistry.Register(store);
        public void Register(ISaveInterpreter interpreter) => _interpreterRegistry.Register(interpreter);
        public void Unregister(ISaveStore store) => _storeRegistry.Unregister(store);
        public void Unregister(ISaveInterpreter interpreter) => _interpreterRegistry.Unregister(interpreter);

        public void CaptureSnapshot(bool overwriteChunks = false) => controller.CaptureSnapshot(overwriteChunks);
        public void ApplySnapshot(string saveName) => controller.ApplySnapshot(new SaveContainer() { Name = saveName });
        public void LoadSnapshot(string saveName) => controller.LoadSnapshot(new SaveContainer() { Name = saveName });
        public SaveContainer[] ListSaves() => _loader.ListSaves();
    }
}