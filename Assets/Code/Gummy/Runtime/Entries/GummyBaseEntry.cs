using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Blackboard;
using Gummy.References;
using Gummy.Shared;

namespace Gummy.Entries
{
    [Serializable]
    public abstract class GummyBaseEntry
    {
        public int id;
        public string key;

        public GummyEntryReference scope;

        public bool once;

        public virtual GummyRuntimeEntryDescriptor descriptor => GummyRuntimeEntryDescriptor.FactDescriptor;

        [SerializeField]
        internal GummyBlackboardCriterion[] criteria = Array.Empty<GummyBlackboardCriterion>();

        [SerializeField]
        internal GummyBlackboardModification[] modifications = Array.Empty<GummyBlackboardModification>();

        public abstract void AddToTable(GummyCollection collection);
        public abstract void RemoveFromTable(GummyCollection collection);
    }
}