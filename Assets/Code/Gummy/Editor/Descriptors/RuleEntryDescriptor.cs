using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;
using Gummy.Shared;

namespace Gummy.Editor
{
    [CustomEntryDescriptorAttribute(typeof(GummyRuleEntry))]
    public class RuleEntryDescriptor : EntryDescriptor
    {
        public override string Name => "Rule";
        public override GummyEntryType Type { get; protected set; } = GummyEntryType.Rule;
        public override bool Optional => false;
        public override string Color => "#f16d42";

        public override void HandleEntryCreated(GummyBaseEntry entry, GummyCollection table)
        {
            table.AddEntry(entry);
        }

        public RuleEntryDescriptor()
        {
            this.RealType = typeof(GummyRuleEntry);
        }
    }
}
