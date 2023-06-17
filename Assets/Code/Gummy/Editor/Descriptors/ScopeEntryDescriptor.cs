using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;

namespace Gummy.Editor
{
    [CustomEntryDescriptorAttribute(typeof(GummyScopeEntry))]
    public class ScopeEntryDescriptor : EntryDescriptor
    {
        public override string Name => "Scope";
        public override GummyEntryType Type { get; protected set; } = GummyEntryType.Fact;
        public override bool Optional => false;
        public override string Color => "#a0e60c";
    }
}
