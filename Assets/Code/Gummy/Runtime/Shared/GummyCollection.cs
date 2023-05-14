using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;

namespace Gummy.Shared
{
    [Serializable]
    public class GummyCollection : ScriptableObject
    {
        public string Name;
        // I would probably have a member variable to hold the database object given
        [SerializeReference] public List<GummyFactEntry> facts = new();
        [SerializeReference] public List<GummyRuleEntry> rules = new();
        [SerializeReference] public List<GummyEventEntry> events = new();

        public virtual void RemoveEntry(GummyBaseEntry entry)
        {
            entry.RemoveFromTable(this);
        }

        public virtual void AddEntry(GummyBaseEntry entry)
        {
            entry.AddToTable(this);
        }

        public virtual void Setup(GummyDatabase database)
        {
        }
    }
}