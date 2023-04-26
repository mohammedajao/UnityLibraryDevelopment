using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;

[CreateAssetMenu(fileName="ChunkAssetTest", menuName="TimeWizard/ChunkAssetTest")]
public class ChunkAssetTest : ScriptableObject
{
    [SerializeField] public List<Chunk> Data;
}
