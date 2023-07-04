using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;
using Gummy.Shared;

namespace Gummy.Editor
{
    [CustomEntryDescriptorAttribute(typeof(GummyFactEntry))]
    public class FactEntryDescriptor : EntryDescriptor
    {
        public override string Name => "Fact";
        public override GummyEntryType Type { get; protected set; } = GummyEntryType.Fact;
        public override bool Optional => false;
        public override string Color => "#a0e60c";
        public override bool HasCustomization => false;

        public override void HandleEntryCreated(GummyBaseEntry entry, GummyCollection table)
        {
            table.AddEntry(entry);
        }

        public FactEntryDescriptor()
        {
            this.RealType = typeof(GummyFactEntry);
        }
    }
}
