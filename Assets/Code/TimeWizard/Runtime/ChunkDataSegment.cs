using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimeWizard
{
    [Serializable]
    struct ChunkSubdata
    {
        public string id;
        public object data;
    }

    [Serializable]
    struct ChunkSubdataWrapper
    {
        public ChunkSubdata[] segments;
    }

    [Serializable]
    public class ChunkDataSegment
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
        private readonly string _json;

        public ChunkDataSegment() { }

        public ChunkDataSegment(string json)
        {
            ChunkSubdataWrapper info = JsonUtility.FromJson<ChunkSubdataWrapper>(json);
            // _data = info.segments.ToDictionary(d => d.id, d => (object) d);
            _json = json;
        }

        public string ToJSON() => _json;
        public T As<T>() => JsonUtility.FromJson<T>(_json);
        public bool ContainsKey(string key) => _data.ContainsKey(key);
        public int Count() => _data.Count;
        public Type CheckType(string key) => ContainsKey(key) ? _data[key].GetType() : null;

        public T Get<T>(string key)
        {
            if(!ContainsKey(key))
            {
                throw new KeyNotFoundException($"The key {key} was not found in the collection.");
            }

            return (T) _data[key];
        }
    }
}