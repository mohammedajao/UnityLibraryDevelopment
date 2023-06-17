using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Blackboard;
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

        [RecreateLookup]
        public GummyEntryReference triggeredBy;

        [RecreateLookup]
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

        // For dialogue, we'll override this function
        // The onStart/onEnd events will not be invoked in the case of dialogue
        // Instead, the Run(...) method will be called
        // Execute will call it prior to onEnd in the overriden variation
        // It will do so by fetching the speaker gameObject and passing the Run function to it
        // The GameObject will start a new coroutine and pass the required params to Run
        public virtual IEnumerator Execute()
        {
            for (int i = 0; i < onStart.Length; i++) {
                yield return onStart[i];
            }
            for (int i = 0; i < onEnd.Length; i++) {
                yield return onEnd[i];
            }
            yield break;
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
