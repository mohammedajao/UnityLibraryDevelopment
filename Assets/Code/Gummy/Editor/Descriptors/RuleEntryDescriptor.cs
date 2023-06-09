using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;

namespace Gummy.Editor
{
    [CustomEntryDescriptorAttribute(typeof(GummyRuleEntry))]
    public class RuleEntryDescriptor : EntryDescriptor
    {
        public override string Name => "Rule";
        public override GummyEntryType Type { get; protected set; } = GummyEntryType.Rule;
        public override bool Optional => false;
        public override string Color => "#ae200d ";
    }
}
