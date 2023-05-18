using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Shared;

namespace Gummy.Editor
{
    public class GummyEditorWindow : EditorWindow
    {
        static GummyDatabase db;
        [MenuItem("Window/Gummy")]
        public static void ShowWindow()
        {
            GetWindow<GummyEditorWindow>(false, "Gummy", true);
        }

        private static List<GummyCollection> GetTables()
        {
            return AssetDatabase.FindAssets($"t: {typeof(GummyCollection).Name}").ToList()
                     .Select(AssetDatabase.GUIDToAssetPath)
                     .Select(AssetDatabase.LoadAssetAtPath<GummyCollection>)
                     .ToList();
        }

        void OnEnable() {
            if(db == null) return;
            db.tables = GetTables();
            foreach(var table in db.tables)
            {
                table.Setup(db);
            }
        }

        public static void InitializeIfNeeded()
        {
            if(db == null) {
                var assets = AssetDatabase.FindAssets("GummyDatabase", new[] {
                    "Assets/GameTools/Gummy"
                });
                if(assets.Length == 0) return;
                var asset = assets[0];
                var assetPath = AssetDatabase.GUIDToAssetPath(asset);
                db = (GummyDatabase)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GummyDatabase));
            }
            db.tables = GetTables();
            foreach(var table in db.tables)
            {
                table.Setup(db);
            }
        }

        void OnGUI()
        {
            InitializeIfNeeded();
            if(db == null) return;
            SerializedObject so = new SerializedObject(db);
            SerializedProperty collectionsProperty = so.FindProperty("tables");

            EditorGUILayout.PropertyField(collectionsProperty, true);
            so.ApplyModifiedProperties();
        }

        static void CreateCollection()
        {
            InitializeIfNeeded();
            if(db == null) return;
            //https://answers.unity.com/questions/1326881/right-click-in-custom-editor.html
        }
    }
}
