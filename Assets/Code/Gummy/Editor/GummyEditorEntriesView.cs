using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Shared;

namespace Gummy.Editor
{
    public class GummyEditorEntriesView : IGummyEditorSection
    {
        GummyCollection currentTable;
        bool showFacts = false;
        bool showEvents = false;
        bool showRules =  false;
        public void Initialize()
        {

        }

        void DrawHorizontalLine()
        {
            var rect = EditorGUILayout.GetControlRect(GUILayout.Height(12));
            rect.height = 1;
            rect.y += 5;
            rect.x -= 3;
            rect.width += 4;
            EditorGUI.DrawRect(rect, Color.black);
        }

        public void UpdateTable(GummyCollection table)
        {
            currentTable = table;
        }
        public void OnInspectorGUI(GummyDatabase database, SerializedObject db)
        {
            EditorGUIUtility.LookLikeInspector();
            EditorGUILayout.LabelField("Entries View");
            if(currentTable == null) return;
            EditorGUILayout.LabelField(currentTable.Name);
            showFacts = EditorGUILayout.Foldout(showFacts, "Facts", true);
            if(showFacts) {
                EditorGUI.indentLevel++;
                foreach(var fact in currentTable.facts)
                {
                    EditorGUILayout.LabelField($"{fact.id}");
                }
                EditorGUI.indentLevel--;
            }
            DrawHorizontalLine();
            showEvents = EditorGUILayout.Foldout(showEvents, "Events", true);
            if(showEvents) {
                EditorGUI.indentLevel++;
                foreach(var eventEntry in currentTable.events)
                {
                    EditorGUILayout.LabelField($"{eventEntry.id}");
                }
                EditorGUI.indentLevel--;
            }
            DrawHorizontalLine();
            showRules = EditorGUILayout.Foldout(showRules, "Rules", true);
            if(showRules) {
                EditorGUI.indentLevel++;
                foreach(var ruleEntry in currentTable.rules)
                {
                    EditorGUILayout.LabelField($"{ruleEntry.id}");
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
