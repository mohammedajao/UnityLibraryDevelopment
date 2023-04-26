using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeWizard
{
    [Serializable]
    public struct SaveSnapshotData
    {
        public bool Taken;
        public string Title;
        public int Completion;
    }

    [Serializable]
    public class SaveSnapshot : MonoBehaviour
    {
        public SaveSnapshotData Snapshot;
    }

}