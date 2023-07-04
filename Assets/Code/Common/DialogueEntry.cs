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
        // yield return dialogueSpeaker.StartCoroutine(dialogueSpeaker.Execute(this));
        yield return dialogueSpeaker.Execute(this);
        yield break;
    }

    public override IEnumerator Execute()
    {
        yield return base.Execute();
        yield return Run();
        yield break;
    }
    
    public enum DialogueSpeed
    {
        Slow, 
        Normal, 
        Fast
    }
}
