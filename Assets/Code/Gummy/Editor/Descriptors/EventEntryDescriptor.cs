using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;

namespace Gummy.Editor
{
    [CustomEntryDescriptorAttribute(typeof(GummyEventEntry))]
    public class EventEntryDescriptor : EntryDescriptor
    {
        public override string Name => "Event";
        public override GummyEntryType Type { get; protected set; } = GummyEntryType.Event;
        public override bool Optional => false;
        public override string Color => "#e6d90c";
    }
}
