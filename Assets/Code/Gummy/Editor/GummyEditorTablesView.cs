using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Shared;
using Gummy.Util;

namespace Gummy.Editor
{
    public class GummyEditorTablesView : IGummyEditorSection
    {
        GummyCollection currentTable;
        string searchString;

        public void Initialize()
        {
            
        }

        void DrawHorizontalLine()
        {
            var rect = EditorGUILayout.GetControlRect(GUILayout.Height(12));
            rect.height = 1;
            rect.y += 5;
            rect.x -= 5;
            rect.width += 9;
            EditorGUI.DrawRect(rect, Color.black);
        }

        public void OnInspectorGUI(GummyDatabase database, SerializedObject db)
        {
            // EditorGUILayout.Space();
            // EditorGUILayout.BeginHorizontal();
            // searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
            // EditorGUILayout.EndHorizontal();
            // EditorGUILayout.Space();
            // DrawHorizontalLine();
            // foreach(var table in database.tables) {
            //     if(!string.IsNullOrEmpty(searchString) && !table.Name.ToLower().Contains(searchString.ToLower())) continue;
            //     if(GUILayout.Button(table.Name)) {
            //         currentTable = table;
            //         GummyUtil.RaiseTableSelected(table);
            //     }
            // }
        }
    }
}