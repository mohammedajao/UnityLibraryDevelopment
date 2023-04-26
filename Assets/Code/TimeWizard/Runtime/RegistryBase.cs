using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeWizard
{
    public class RegistryBase<T> : IRegistry<T>
    {
        private readonly List<T> _items = new List<T>();
        public event ItemRegistered<T> OnItemRegistered;

        public void Register(T instance)
        {
            if(!_items.Contains(instance))
            {
                _items.Add(instance);
                OnItemRegistered?.Invoke(instance);
            }
        }

        public void Unregister(T instance)
        {
            _items.Remove(instance);
        }

        private void Prune()
        {
            int idx = 0;
            while(idx < _items.Count)
            {
                if(_items[idx] == null)
                {
                    _items.RemoveAt(idx);
                    continue;
                }
                idx++;
            }
        }

        public T[] List()
        {
            Prune();
            return _items.ToArray();
        }
    }
}