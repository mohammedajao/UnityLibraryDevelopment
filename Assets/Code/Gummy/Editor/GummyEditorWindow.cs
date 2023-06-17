using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Shared;
using Gummy.Util;

namespace Gummy.Editor
{
    [InitializeOnLoad]
    public class GummyEditorWindow : EditorWindow
    {
        // static GummyDatabase db;
        // static SerializedObject serializedDB;

        // static GummyCollection currentTable;

        // static GummyEditorTablesView tablesView = new GummyEditorTablesView();
        // static GummyEditorEntriesView entriesView = new GummyEditorEntriesView();

        // static bool applicationQuitEventAdded = false;

        // [MenuItem("Window/Gummy")]
        // public static void ShowWindow()
        // {
        //     GetWindow<GummyEditorWindow>(false, "Gummy", true);
        //     InitializeIfNeeded();
        //     if(!applicationQuitEventAdded) {
        //         applicationQuitEventAdded = true;
        //         EditorApplication.wantsToQuit += OnEditorApplicationQuit;
        //     }
        // }

        // static bool OnEditorApplicationQuit()
        // {
        //     if(db == null) return true;
        //     foreach(var table in db.GetDirtyTables()) {
        //         EditorUtility.SetDirty(table);
        //     }
        //     db.ClearDirtyTables();
        //     AssetDatabase.SaveAssets();
        //     AssetDatabase.Refresh();
        //     return true;
        // }

        // private void OnDestroy()
        // {
        //     GummyUtil.OnTableSelected -= SetCurrentTable;
        // }

        // private static void SetCurrentTable(GummyCollection table)
        // {
        //     currentTable = table;
        //     entriesView.UpdateTable(table);
        // }

        // [InitializeOnLoadMethod]
        // public static void InitializeIfNeeded()
        // {
        //     if(db == null) {
        //         var assets = AssetDatabase.FindAssets("GummyDatabase", new[] {
        //             "Assets/GameTools/Gummy"
        //         });
        //         if(assets.Length == 0) return;
        //         var asset = assets[0];
        //         var assetPath = AssetDatabase.GUIDToAssetPath(asset);
        //         db = (GummyDatabase)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GummyDatabase));
        //         serializedDB = new SerializedObject(db);
        //     }
        //     if(serializedDB != null) {
        //       serializedDB.Update(); 
        //     }
        //     if(!applicationQuitEventAdded) {
        //         applicationQuitEventAdded = true;
        //         EditorApplication.wantsToQuit += OnEditorApplicationQuit;
        //     }
        //     GummyUtil.OnTableSelected -= SetCurrentTable;
        //     GummyUtil.OnTableSelected += SetCurrentTable;
        //     // db.ClearDirtyTables();
        // }

        // void OnGUI()
        // {
        //     if(db == null) return;
        //     serializedDB.Update();
        //     EditorGUILayout.BeginHorizontal();
        //     var tableViewsRect = EditorGUILayout.BeginVertical();
        //     tablesView.OnInspectorGUI(db, serializedDB);
        //     EditorGUILayout.EndVertical();
        //     EditorGUI.DrawRect(new Rect(tableViewsRect.x + tableViewsRect.width, 0, 1, position.height), Color.black);
        //     var entriesViewsRect = EditorGUILayout.BeginVertical();
        //     if(currentTable != null) {
        //         entriesView.OnInspectorGUI(db, serializedDB);
        //     }
        //     EditorGUILayout.EndVertical();
        //     EditorGUI.DrawRect(new Rect(entriesViewsRect.x + entriesViewsRect.width, 0, 1, position.height), Color.black);
        //     EditorGUILayout.EndHorizontal();
        //     serializedDB.ApplyModifiedProperties();
        // }
    }
}
