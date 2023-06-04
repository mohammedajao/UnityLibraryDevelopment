using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Shared;

namespace Gummy.Editor
{
    public interface IGummyEditorSection
    {
        void Initialize();
        void OnInspectorGUI(GummyDatabase database, SerializedObject db);
    }
}