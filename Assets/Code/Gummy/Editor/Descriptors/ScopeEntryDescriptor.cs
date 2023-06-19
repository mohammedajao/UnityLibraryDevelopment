using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;
using Gummy.Shared;

namespace Gummy.Editor
{
    [CustomEntryDescriptorAttribute(typeof(GummyScopeEntry))]
    public class ScopeEntryDescriptor : EntryDescriptor
    {
        public override string Name => "Scope";
        public override GummyEntryType Type { get; protected set; } = GummyEntryType.Fact;
        public override bool Optional => false;
        public override string Color => "#d1863f";

        public override void HandleEntryCreated(GummyBaseEntry entry, GummyCollection table)
        {
            var nentry = (GummyScopeEntry)entry;
            table.facts.Add(nentry);
        }

        public ScopeEntryDescriptor()
        {
            this.RealType = typeof(GummyScopeEntry);
        }
    }
}
