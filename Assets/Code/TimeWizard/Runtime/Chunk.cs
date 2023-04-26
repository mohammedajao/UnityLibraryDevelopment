using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeWizard
{
    [Serializable]
    public class Chunk
    {
        [SerializeField] public Guid ID = Guid.NewGuid();
        [SerializeField] public string Name;

        [SerializeField] public StringStringMap storage = new StringStringMap();

        public bool TryGetValue(string id, out string json)
        {
            if(!storage.ContainsKey(id)) {
                json = null;
                return false;
            }
            json = storage[id];
            return true;
        }

        public void AddToChunk(string key, object data, bool overwrite = false)
        {
            var json = JsonUtility.ToJson(data);
            if(storage.ContainsKey(key) && overwrite)
            {
                storage[key] = json;
                return;
            }
            storage.Add(key, json);
        }

        public override int GetHashCode()
        {
            return $"{ID}--{Name}".GetHashCode();
        }

        public Chunk(string name = "")
        {
            Name = name;
        }
    }
}