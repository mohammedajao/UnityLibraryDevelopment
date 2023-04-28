using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TimeWizard;
using TimeWizard.Core;
using TimeWizard.UnityEditor;

// Logic error
public class SaveMetadataInterpreter : ITimeWizardInterpreter
{
    public SaveManager Manager => SaveManager.Instance;

    private bool _isDirty = false;
    public bool IsDirty() => _isDirty;

    private bool _showSaveMetadata = false;
    private string _json;

    public void OnEnable()
    {
        _json = Manager.CurrentSaveContext?.JSON;
    }

    public void OnDisable()
    {
    }

    public void DrawInspectorGUI()
    {
        string Label = $"Save Metadata";
        _showSaveMetadata = EditorGUILayout.Foldout(_showSaveMetadata, Label);

        if(_showSaveMetadata)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.SelectableLabel(_json, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            EditorGUI.indentLevel--;
        }
    }

    public Chunk[] ApplyModifications(Chunk[] saveChunks)
    {
        return saveChunks;
    }

    public void ProcessChunks(Chunk[] saveChunks)
    {
        Manager.CurrentSaveContext?.ProcessChunks(saveChunks);
        _json = Manager.CurrentSaveContext?.JSON;
    }
}
