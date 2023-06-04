using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="TOW", menuName="Gummy/TOW")]
public class TextObjectWrapper : ScriptableObject
{
    public readonly string ID = System.Guid.NewGuid().ToString();
    public string text;
}