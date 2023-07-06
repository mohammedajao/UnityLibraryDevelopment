using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Editor;
using Gummy.Entries;

[CustomEntryDescriptorAttribute(typeof(SpeakerEntry))]
public class SpeakerEntryDescriptor : FactEntryDescriptor
{
    public override string Name => "Speaker";
    public override string Color => "#e8b496";

    public SpeakerEntryDescriptor()
    {
        this.RealType = typeof(SpeakerEntry);
    }
}
