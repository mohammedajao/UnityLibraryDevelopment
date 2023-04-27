using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TimeWizard.UnityEditor
{
    public class ChunksListAsset : ScriptableObject
    {
        [SerializeField] public List<Chunk> chunks = new();
    }
}