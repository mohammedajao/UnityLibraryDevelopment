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
        private const string EMPTY_KEYWORD = "(Empty Reference)";

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
            property.serializedObject.Update();
            var fieldData = fieldInfo.GetCustomAttributes(false);
            GummyEntryFilterAttribute attributeInfo = null;
            foreach (object atr in fieldData)
            {
                if (atr as GummyEntryFilterAttribute != null)
                {
                    attributeInfo = (GummyEntryFilterAttribute)atr;
                }
            }

            var popField = (DropdownField)root;
            var idProp = property.FindPropertyRelative("id");
            var id = idProp.intValue;
            Label title = new(property.displayName);

            if(attributeInfo != null && !attributeInfo.AllowEmpty && id == 0) {
                Debug.LogError($"[Gummy] Reference is not allowed to be empty for property: {property.displayName}");
            }


            var target = popField.Q<VisualElement>(className: "unity-base-popup-field__input");
            target.pickingMode = PickingMode.Position;
            target.AddToClassList("unity-property-field__input");

            // var ve = popField.Q<VisualElement>(className: "unity-base-popup-field__input");
            // ve.style.width = 
            popField.label = property.displayName;
            if (
                idProp.intValue != 0 
                && GummyUtil.database.GetTableNameFromID(idProp.intValue, out var name) 
                && GummyUtil.database.GetEntryFromID(idProp.intValue, out var entry)
            ) {
                popField.value = $"{name}/{entry.key}";
            } else {
                popField.value = EMPTY_KEYWORD;
                idProp.intValue = 0;
            }

            target.AddManipulator(new Clickable(evt => {
                var box = popField.Q<VisualElement>(className: "unity-base-popup-field__input");
                UnityEditor.PopupWindow.Show(box.worldBound, new EntriesPopupContent((int id) => {
                    property.serializedObject.Update();
                    GummyUtil.database.RequireLookup();
                    if(idProp.intValue == id) {
                        idProp.intValue = 0;
                        popField.value = EMPTY_KEYWORD;
                        property.serializedObject.ApplyModifiedProperties();
                    } else if (GummyUtil.database.GetTableNameFromID(id, out var name) && GummyUtil.database.GetEntryFromID(id, out var entry))
                    {
                        popField.value = $"{name}/{entry.key}";
                        idProp.intValue = id;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }, attributeInfo));
            }));

            return root;
        }
    }
}