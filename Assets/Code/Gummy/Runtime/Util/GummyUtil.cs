using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Blackboard;
using Gummy.Shared;
using Gummy.Entries;

namespace Gummy.Util {
    public delegate void GummyEntryChanged(int id, IGummyBlackboard context);
    public static class GummyUtil
    {
        public static GummyDatabase database;
        public static SerializedObject serializedDatabase;

        public static event GummyEntryChanged OnEntryChanged;

        public static void RaiseEntryChanged(int id, IGummyBlackboard context) {
            OnEntryChanged?.Invoke(id, context);
        }

        [InitializeOnLoadMethod]
        public static void InitializeIfNeeded()
        {
            // if(database == null) {
            //     database = Resources.Load("GummyDatabase") as GummyDatabase;
            //     serializedDatabase = new SerializedObject(database);
            // }
            if(database == null) {
                var assets = AssetDatabase.FindAssets("t:" + typeof(GummyDatabase).FullName);
                if(assets.Length == 0) return;
                var asset = assets[0];
                var assetPath = AssetDatabase.GUIDToAssetPath(asset);
                database = (GummyDatabase)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GummyDatabase));
                serializedDatabase = new SerializedObject(database);
            }
            if(serializedDatabase != null) {
              serializedDatabase.Update(); 
            }
        }
    }
}
