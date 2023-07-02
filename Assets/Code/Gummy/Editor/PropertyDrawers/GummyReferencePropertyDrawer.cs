using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using Gummy.References;
using Gummy.Util;

// https://docs.unity3d.com/Manual/UIE-create-a-popup-window.html
// https://forum.unity.com/threads/uielements-runtime-popup-example.827565/
namespace Gummy.Editor
{
    [CustomPropertyDrawer(typeof(GummyEntryReference))]
    public class GummyReferencePropertyDrawer : PropertyDrawer
    {

        bool openPopup;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new DropdownField();
            container.AddToClassList("unity-base-field__aligned");
            container.AddToClassList("gummy-reference");

            var sheet = Resources.Load<StyleSheet>("GummyReferenceStyle");
            container.styleSheets.Add(sheet);
            // container.style.justifyContent = Justify.SpaceBetween;
            // container.style.flexDirection = FlexDirection.Row;
            // container.style.overflow = Overflow.Visible;
            return BuildUI(container, property);
        }

        private VisualElement BuildUI(VisualElement root, SerializedProperty property)
        {
            var popField = (DropdownField)root;
            var idProp = property.FindPropertyRelative("id");
            var id = idProp.intValue;
            Label title = new(property.displayName);

            var target = popField.Q<VisualElement>(className: "unity-base-popup-field__input");
            target.pickingMode = PickingMode.Position;
            target.AddToClassList("unity-property-field__input");

            // var ve = popField.Q<VisualElement>(className: "unity-base-popup-field__input");
            // ve.style.width = 
            popField.label = property.displayName;
            if(id == 0) {
                popField.value = "(Empty Reference)";
            } else {
                if (GummyUtil.database.GetTableNameFromID(id, out var name) && GummyUtil.database.GetEntryFromID(id, out var entry))
                {
                    popField.value = $"{name}/{entry.key}";
                }
            }

            target.AddManipulator(new Clickable(evt => {
                var box = popField.Q<VisualElement>(className: "unity-base-popup-field__input");
                UnityEditor.PopupWindow.Show(box.worldBound, new EntriesPopupContent((int id) => {
                    property.serializedObject.Update();
                    GummyUtil.database.RequireLookup();
                    if (GummyUtil.database.GetTableNameFromID(id, out var name) && GummyUtil.database.GetEntryFromID(id, out var entry))
                    {
                        popField.value = $"{name}/{entry.key}";
                        idProp.intValue = id;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }));
            }));

            return root;
        }
    }
}