using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard.Persistence;
using TimeWizard;

namespace TimeWizard.Core
{
    public class SaveManager
    {
        private ISaveLoader _loader;
        private IRegistry<ISaveStore> _storeRegistry = new RegistryBase<ISaveStore>();
        private IRegistry<ISaveInterpreter> _interpreterRegistry = new RegistryBase<ISaveInterpreter>();
        private SaveController controller;

        private static SaveManager _instance = null;
        private static readonly object _lock = new object();
        private SaveContext _currentSaveContext = null;

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
            if(_currentSaveContext != null) Unregister(_currentSaveContext);
            _currentSaveContext = new SaveContext(saveId, controller);
            Register(_currentSaveContext);
            return _currentSaveContext;
        }
    
        // protected override void SingletonAwake() {
        //     _loader = new FileSaveLoader("BaseDevSaves"); // Later app/GM singleton should provide context with inline bootstrap unity attr
        //     controller = new SaveController(_storeRegistry, _interpreterRegistry, _loader);
        // }

        public SaveContext CurrentSaveContext => _currentSaveContext;
        public int IRSize => _interpreterRegistry.List().Length;
        public void Register(ISaveStore store) => _storeRegistry.Register(store);
        public void Register(ISaveInterpreter interpreter) => _interpreterRegistry.Register(interpreter);
        public void Unregister(ISaveStore store) => _storeRegistry.Unregister(store);
        public void Unregister(ISaveInterpreter interpreter) => _interpreterRegistry.Unregister(interpreter);

        public void CaptureSnapshot(bool overwriteChunks = true) => controller.CaptureSnapshot(overwriteChunks);
        public void ApplySnapshot(string saveName) => controller.ApplySnapshot(new SaveContainer() { Name = saveName });
        public void LoadSnapshot(string saveName) => controller.LoadSnapshot(new SaveContainer() { Name = saveName });
        public void UpdateSnapshot(Chunk[] saveChunks) => controller.UpdateSnapshot(saveChunks);
        public void DeleteSave(string saveName) => controller.DeleteSnapshot(saveName);
        public SaveContainer[] ListSaves() => _loader.ListSaves();
    }
}