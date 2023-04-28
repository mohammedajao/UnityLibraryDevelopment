using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeWizard
{
    [Serializable]
    public class SaveState
    {
        [SerializeField] public string ChunkName { get; set; }
        [SerializeField] public object Data { get; set; }
        public bool IsOverwritable = true;
    }

    public interface ISaveStore
    {
        string GetIdentifier();
        List<SaveState> FetchSaveStates();
        void LoadChunkData(string chunkName, ChunkDataSegment info);
    }
}