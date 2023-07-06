using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EntriesPopup : EditorWindow
{
    [MenuItem("Window/EntriesPopup")]
    public static void Init()
    {
        EntriesPopup window = EditorWindow.CreateInstance<EntriesPopup>();
        // // window.ShowPopup();
        window.Show();
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        Label label = new Label("TestasDSSADs");
        root.Add(label);
    }

    Rect buttonRect;
    void OnGUI() {
        // Debug.Log(rect.position);
        if (GUILayout.Button("Popup Options", GUILayout.Width(200)))
        {
            UnityEditor.PopupWindow.Show(buttonRect, new EntriesPopupContent((int id) => {}, null));
        }
        if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
    }
}
