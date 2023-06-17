using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using Gummy.Entries;

// https://forum.unity.com/threads/uielements-listview-with-serializedproperty-of-an-array.719570/
// https://docs.unity3d.com/Manual/UIE-create-list-view-complex.html

namespace Gummy.Editor
{
    // [CustomPropertyDrawer(typeof(GummyBaseEntry))]
    // public class GummyBaseEntryDrawer : PropertyDrawer
    // {

    //     private Type GetEntryDescriptorType(GummyBaseEntry entry)
    //     {
    //         Type currType = entry.GetType();
    //         if(DescriptorCache.Descriptors.ContainsKey(currType)) return currType;
    //         foreach(var kvp in DescriptorCache.Descriptors) {
    //             if(entry.GetType().IsSubclassOf(kvp.Key)) {
    //                 currType = kvp.Key;
    //             }
    //         }
    //         DescriptorCache.Descriptors[entry.GetType()] = DescriptorCache.Descriptors[currType];
    //         return currType;
    //     }

    //     private Texture2D MakeTex(int width, int height, Color col)
    //     {
    //         Color[] pix = new Color[width * height];
    //         for (int i = 0; i < pix.Length; ++i)
    //         {
    //             pix[i] = col;
    //         }
    //         Texture2D result = new Texture2D(width, height);
    //         result.SetPixels(pix);
    //         result.Apply();
    //         return result;
    //     }

    //     public override VisualElement CreatePropertyGUI(SerializedProperty property)
    //     {
    //         var entry = (GummyBaseEntry)property.boxedValue;
    //         var container = new VisualElement();
    //         container.AddToClassList("gummy-entry");
    //         return BuildUI(container, property);
    //     }

    //     private VisualElement BuildUI(VisualElement root, SerializedProperty property)
    //     {
    //         VisualElement alignmentContainer = new();
    //         alignmentContainer.AddToClassList("align-horizontal");
    //         Label title = new("<EntryTitle>");
    //         alignmentContainer.Add(title);
    //         return root;
    //     }

    //     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     {
    //         EditorGUI.BeginProperty(position, label, property);
    //         var name = property.FindPropertyRelative("key");
    //         var id = property.FindPropertyRelative("id"); //
    //         // var descriptor = property.FindPropertyRelative("descriptor").objectReferenceValue;
    //         var entry = (GummyBaseEntry)property.boxedValue;
    //         Type descriptorType = GetEntryDescriptorType(entry);
    //         var descriptor = DescriptorCache.Descriptors[descriptorType];
    //         var targetObjectType = property.serializedObject.targetObject.GetType();
    //         var eventEntryRow = EditorGUILayout.BeginHorizontal();
    //         EditorGUILayout.SelectableLabel($"{id.intValue}");
    //         bool mouseOver = eventEntryRow.Contains(Event.current.mousePosition);
    //         GUIStyle bgStyle = new GUIStyle(GUI.skin.box);
    //         if(mouseOver) {
    //             GUI.backgroundColor = Color.red;
    //             GUI.Box(eventEntryRow, "", bgStyle);
    //         }
    //         var eventEntryNameRect = new Rect(eventEntryRow.x, eventEntryRow.y, eventEntryRow.width / 2, eventEntryRow.height);
    //         var eventEntryTypeRect = new Rect(eventEntryRow.x + (eventEntryRow.width / 2) - 10, eventEntryRow.y, eventEntryRow.width / 2, eventEntryRow.height);

    //         GUIStyle style = new GUIStyle(GUI.skin.label);
    //         style.alignment = TextAnchor.MiddleRight;
    //         style.fontStyle = FontStyle.Bold;
    //         style.normal.textColor = descriptor.ParsedColor;
    //         EditorGUI.LabelField(eventEntryTypeRect, $"{descriptor.Name}", style);
    //         EditorGUILayout.EndHorizontal();
    //         EditorGUI.EndProperty();
    //     }

    //     public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
    //         return EditorGUIUtility.singleLineHeight;
    //     }
    // }
}