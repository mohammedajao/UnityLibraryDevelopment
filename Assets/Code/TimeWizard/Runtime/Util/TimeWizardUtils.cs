using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;

namespace TimeWizard.Util
{
    // public delegate void TimeWizardSnapshotAction(SaveSnapshot snapshot);
    public class TimeWizardUtils
    {
        public static readonly string DebugTitle = "[TimeWizard]";

        // public static event Action SnapshotCaptured;
        // public static event Action SnapshotUpdated;
        // public static event Action SnapshotDeleted;
        // public static event TimeWizardSnapshotAction SnapshotSyncRequested;

        public static void Log(string s) => Debug.Log($"{DebugTitle} {s}");
        public static void LogError(string s) => Debug.LogError($"{DebugTitle} {s}");
        public static void LogWarning(string s) => Debug.LogWarning($"{DebugTitle} {s}");
    }
}
