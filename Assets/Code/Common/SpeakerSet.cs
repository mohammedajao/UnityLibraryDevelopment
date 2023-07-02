using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;

[CreateAssetMenu(fileName = "SpeakerRuntimeSet", menuName = "RuntimeSets/Speakers")]
public class SpeakerSet : ScriptableObject
{
    private readonly Dictionary<int, SpeakerComponent> data = new();

    public void RegisterSpeaker(int id, SpeakerComponent speaker)
    {
        data[id] = speaker;
    }

    public void UnregisterSpeaker(int id)
    {
        if (!data.ContainsKey(id)) return;
        data.Remove(id);
    }

    public bool TryGetSpeaker(int id, out SpeakerComponent speaker)
    {
        speaker = null;
        if (data.ContainsKey(id)) {
            speaker = data[id];
            return true;
        }
        return false;
    }
}
