using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Shared;
using Gummy.Entries;

namespace Gummy.Editor
{
    public class GummyEditorEntriesView : IGummyEditorSection
    {
        GummyCollection currentTable;
        SerializedObject serializedTable;
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
            serializedTable = new SerializedObject(table);
        }

        public void OnInspectorGUI(GummyDatabase database, SerializedObject db)
        {
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
                var tableEvents = serializedTable.FindProperty("events");
                // var tableSo = new SerializedObject()
                for(int i = 0; i < tableEvents.arraySize; i++) {
                    SerializedProperty serializedPropertyID = tableEvents.GetArrayElementAtIndex(i).FindPropertyRelative("id");
                    var eventEntryRow = EditorGUILayout.BeginHorizontal();
                    EditorGUI.PropertyField(eventEntryRow, tableEvents.GetArrayElementAtIndex(i), new GUIContent($"{serializedPropertyID.intValue}"), true);
                    EditorGUILayout.EndHorizontal();
                }
                // foreach(var eventEntry in currentTable.events)
                // {
                    // Type eventEntryType = GetEntryDescriptorType(eventEntry);
                    // var descriptor = GummyEditorWindow.descriptors[eventEntryType];
                    // var eventEntryRow = EditorGUILayout.BeginHorizontal();
                    // eventEntryRow.height = EditorGUIUtility.singleLineHeight;
                    // bool mouseOver = eventEntryRow.Contains(Event.current.mousePosition);
                    // GUIStyle bgStyle;

                    // var eventEntryNameRect = new Rect(eventEntryRow.x, eventEntryRow.y, eventEntryRow.width / 2, eventEntryRow.height);
                    // var eventEntryTypeRect = new Rect(eventEntryRow.x + (eventEntryRow.width / 2) - 10, eventEntryRow.y, eventEntryRow.width / 2, eventEntryRow.height);

                    // GUIStyle style = new GUIStyle(GUI.skin.label);
                    // style.onHover.textColor = Color.red;
                    // style.alignment = TextAnchor.MiddleRight;
                    // style.fontStyle = FontStyle.Bold;
                    // style.normal.textColor = descriptor.ParsedColor;
                    // EditorGUILayout.LabelField( $"{eventEntry.id}");
                    // EditorGUI.LabelField(eventEntryTypeRect, $"{descriptor.Name}", style);
                    // EditorGUILayout.EndHorizontal();
                // }
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
            if(serializedTable != null) {
                serializedTable.ApplyModifiedProperties();
            }
        }
    }
}
