using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;
using TimeWizard.Core;

public class SaveStoreManager
{
    public List<ISaveStore> Stores = new();
    private SaveContext _currentSaveContext;

    public void Register(ISaveStore store)
    {
        Stores.Add(store);
        if(_currentSaveContext != null)
        {
            _currentSaveContext.Manager.Register(store);
        }
    }

    public void Unregister(ISaveStore store)
    {
        Stores.Remove(store);
        if(_currentSaveContext == null) return;
        if(_currentSaveContext != null)
        {
            _currentSaveContext.Manager.Unregister(store);
        }
    }

    public void OnLoad(SaveContext Save)
    {
        _currentSaveContext = Save;
        foreach(var store in Stores)
        {
            _currentSaveContext.Manager.Register(store);
        }
        _currentSaveContext.Manager.CaptureSnapshot();
    }

    public void OnDestroy()
    {
        if(_currentSaveContext != null)
        {
            foreach(var store in Stores)
            {
                _currentSaveContext.Manager.Unregister(store);
            }
        }
    }
}
