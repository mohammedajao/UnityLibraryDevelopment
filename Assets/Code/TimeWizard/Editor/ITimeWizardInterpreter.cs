using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;

namespace TimeWizard.UnityEditor
{
    public interface ITimeWizardInterpreter : ISaveInterpreter
    {
        void OnEnable();
        void DrawInspectorGUI();
        void OnDisable();
    }
}
