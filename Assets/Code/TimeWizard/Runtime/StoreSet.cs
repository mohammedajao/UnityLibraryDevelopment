using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;
using TimeWizard.Core;

namespace TimeWizard
{
    public class StoreSet : ScriptableObject
    {
        public List<ISaveStore> Stores = new();
        public SaveContext Save;

        public void Add(ISaveStore store)
        {
            if(!Stores.Contains(store))
            {
                Stores.Add(store);
            }
        }

        public void Remove(ISaveStore store)
        {
            if(!Stores.Contains(store))
            {
                Stores.Remove(store);
            }
        }

        public void OnLoad(SaveContext Save)
        {
            this.Save = Save;
            foreach(var store in Stores)
            {
                Save.Manager.Register(store);
            }
            Save.Manager.CaptureSnapshot();
        }

        public void OnDestroy()
        {
            if(Save == null) return;
            foreach(var store in Stores)
            {
                Save.Manager.Unregister(store);
            }
        }
    }
}
