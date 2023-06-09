using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.References;
using Gummy.Shared;
using Gummy.Tools;

namespace Gummy.Entries
{
    [Serializable]
    public class GummyRuleEntry : GummyBaseEntry, IComparable<GummyRuleEntry>
    {
        [SerializeField] public bool isCancellable;
        [SerializeField] private int padding;
        [SerializeField] private float delay;

        public GummyEntryReference triggeredBy;
        public GummyEntryReference triggers;

        public override GummyRuntimeEntryDescriptor descriptor => GummyRuntimeEntryDescriptor.RuleDescriptor;

        [SerializeField]
        internal GummyEvent.Triggerable[] onStart = Array.Empty<GummyEvent.Triggerable>();

        [SerializeField]
        internal GummyEvent.Triggerable[] onEnd = Array.Empty<GummyEvent.Triggerable>();

        public int Weight => criteria.Length + padding;
        public float Delay => delay;

        public int CompareTo(GummyRuleEntry other)
        {
            return this.Weight.CompareTo(other.Weight);
        }

        public override void AddToTable(GummyCollection collection)
        {
            descriptor.AddToTable(collection, this);
        }

        public override void RemoveFromTable(GummyCollection collection)
        {
            descriptor.RemoveFromTable(collection, this);
        }
    }
}
