using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Blackboard;
using Gummy.References;
using Gummy.Shared;
using Gummy.Tools;
using Gummy.Util;

namespace Gummy.Entries
{
    [Serializable]
    public class GummyRuleEntry : GummyBaseEntry, IComparable<GummyRuleEntry>
    {
        [SerializeField] public bool isCancellable;
        [SerializeField] private int padding;
        [SerializeField] private float delay;

        [RecreateLookup]
        [GummyEntryFilter(EntryType = GummyEntryFilterType.Event, AllowEmpty = false)]
        public GummyEntryReference triggeredBy;

        [RecreateLookup]
        [GummyEntryFilter(EntryType = GummyEntryFilterType.Event)]
        public GummyEntryReference triggers;

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

        public virtual IEnumerator Execute()
        {
            yield return new WaitForSeconds(delay);
            yield break;
        }

        public override void AddToTable(GummyCollection collection)
        {
            collection.rules.Add(this);
        }

        public override void RemoveFromTable(GummyCollection collection)
        {
            collection.rules.Remove(this);
        }
    }
}
