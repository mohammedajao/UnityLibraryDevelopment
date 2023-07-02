using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.References;

public class SpeakerComponent : MonoBehaviour
{
    public GummyEntryReference speaker;
    public SpeakerSet Speakers;

    public IEnumerator Execute(DialogueEntry entry)
    {
        yield return new WaitForSeconds(entry.Delay);
        Debug.Log($"[Dialogue: {entry.id}] - {entry.text}");
    }

     void Awake() {
        Speakers.RegisterSpeaker(speaker, this);
    }

    void OnDestroy()
    {
        Speakers.UnregisterSpeaker(speaker);
    }

}
