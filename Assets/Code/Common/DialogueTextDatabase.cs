using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IntTOWMap : SerializedDictionary<int, TextObjectWrapper> {}

[CreateAssetMenu(fileName="Diagloue Database", menuName="Gummy/DialogueDatabase")]
public class DialogueTextDatabase : ScriptableObject
{
    public IntTOWMap Data;
}
