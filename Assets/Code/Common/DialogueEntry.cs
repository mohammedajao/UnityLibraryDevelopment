using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;
using Gummy.References;

public class DialogueEntry : GummyRuleEntry
{
    [TextAreaAttribute]
    public string text;
    public GummyEntryReference speaker;
    public DialogueSpeed talkingSpeed;

    public virtual IEnumerator Run()
    {
        if(!App.Instance.ActiveSpeakers.TryGetSpeaker(speaker, out var dialogueSpeaker)) {
            Debug.LogWarningFormat("Skipping dialogue. No suitable speaker found: ({0})", speaker.id);
            yield break;
        }
        for (int i = 0; i < onStart.Length; i++) {
            yield return onStart[i];
        }
        yield return dialogueSpeaker.StartCoroutine(dialogueSpeaker.Execute(this));
        for (int i = 0; i < onEnd.Length; i++) {
            yield return onEnd[i];
        }
        yield break;
    }

    public override IEnumerator Execute()
    {
        IEnumerator runner = Run();
        while(runner.MoveNext()) {
            var current = runner.Current;
            yield return current;
        }
        yield break;
    }
    
    public enum DialogueSpeed
    {
        Slow, 
        Normal, 
        Fast
    }
}
