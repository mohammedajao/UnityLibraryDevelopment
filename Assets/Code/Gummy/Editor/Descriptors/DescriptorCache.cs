using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Entries;

namespace Gummy.Editor
{
    public class DescriptorCache
    {
        private static DescriptorCache _instance = null;
        private static readonly object padlock = new object();
        public static DescriptorCache Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(padlock)
                    {
                        if(_instance == null) {
                            _instance = new DescriptorCache();
                            _instance.Initialize();
                        }
                    }
                }
                return _instance;
            }
        }

        private Dictionary<Type, EntryDescriptor> _descriptors = new();
        public static Dictionary<Type, EntryDescriptor> Descriptors => Instance._descriptors;

        [UnityEditor.Callbacks.DidReloadScripts]
        public static void RuntimeIntitialization()
        {
            Instance.Initialize();
        }

        public void Initialize()
        {
            var descriptorTypes = TypeCache.GetTypesWithAttribute<CustomEntryDescriptorAttribute>();
            foreach(var m in descriptorTypes) {
                var obj = Activator.CreateInstance(m);
                EntryDescriptor desc = (EntryDescriptor)obj;
                var attr = m.GetCustomAttributes(typeof(CustomEntryDescriptorAttribute), false);
                var attrTargetType = ((CustomEntryDescriptorAttribute)attr[0]).type;
                _descriptors[attrTargetType] = desc;
            }
        }

        public Type GetEntryDescriptorType(GummyBaseEntry entry)
        {
            Type currType = entry.GetType();
            if(_descriptors.ContainsKey(currType)) return currType;
            foreach(var kvp in _descriptors) {
                if(entry.GetType().IsSubclassOf(kvp.Key)) {
                    currType = kvp.Key;
                }
            }
            _descriptors[entry.GetType()] = _descriptors[currType];
            return currType;
        }
    } 
}