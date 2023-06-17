using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;

[CreateAssetMenu(fileName = "SpeakerRuntimeSet", menuName = "RuntimeSets/Speakers")]
public class SpeakerSet : ScriptableObject
{
    private readonly Dictionary<int, GameObject> data = new();

    public void RegisterSpeaker(int id, GameObject speaker)
    {
        data[id] = speaker;
    }

    public void UnregisterSpeaker(int id)
    {
        if (!data.ContainsKey(id)) return;
        data.Remove(id);
    }

    public bool TryGetSpeaker(int id, out GameObject speaker)
    {
        speaker = null;
        if (data.ContainsKey(id)) {
            speaker = data[id];
            return true;
        }
        return false;
    }
}
