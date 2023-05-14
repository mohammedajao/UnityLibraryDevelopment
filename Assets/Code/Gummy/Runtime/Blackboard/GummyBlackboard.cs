using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gummy.Blackboard
{
    public interface IGummyBlackboard
    {
        int? Get(int identifier);
        void Set(int identifier, int value);
    }

    [Serializable]
    public abstract class GummyBlackboard : IGummyBlackboard
    {
        [SerializeField] private IntIntMap storage = new();
        [SerializeField] public string Name;
        [SerializeField] public readonly int ID;

        public GummyBlackboard()
        {
            byte[] gb = Guid.NewGuid().ToByteArray();
            int identifier = BitConverter.ToInt32(gb,0);
            ID = identifier;
            Name = identifier.ToString();
        }

        public bool TryGet(int identifier, out int value, int defaultValue = 0)
        {
            if(storage.ContainsKey(identifier))
            {
                value = storage[identifier];
                return true;
            }
            value = defaultValue;
            return false;
        }

        public int? Get(int identifier)
        {
            if(!storage.ContainsKey(identifier))
                return null;
            return storage[identifier];
        }

        public void Set(int identifier, int value)
        {
            storage[identifier] = value;
        }

        public void Clear()
        {
            storage.Clear();
        }
    }
}