using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Editor;

[CustomEntryDescriptorAttribute(typeof(DialogueEntry))]
public class DialogueEntryDescriptor : RuleEntryDescriptor
{
    public override string Name => "Dialogue";

    public DialogueEntryDescriptor()
    {
        this.RealType = typeof(DialogueEntry);
    }
}
