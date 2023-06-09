using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Shared;

namespace Gummy.Entries
{
    [Serializable]
    public class GummyRuntimeEntryDescriptor
    {
        public GummyEntryType type { get; protected set; }
        public static GummyRuntimeEntryDescriptor RuleDescriptor = new GummyRuntimeEntryDescriptor(GummyEntryType.Rule);
        public static GummyRuntimeEntryDescriptor FactDescriptor = new GummyRuntimeEntryDescriptor(GummyEntryType.Fact);
        public static GummyRuntimeEntryDescriptor EventDescriptor = new GummyRuntimeEntryDescriptor(GummyEntryType.Event);

        protected GummyRuntimeEntryDescriptor(GummyEntryType type)
        {
            this.type = type;
        }

        public void AddToTable(GummyCollection table, GummyRuleEntry entry) 
        {
            table.rules.Add(entry);
        }

        public void AddToTable(GummyCollection table, GummyFactEntry entry) 
        {
            table.facts.Add(entry);
        }

        public void AddToTable(GummyCollection table, GummyEventEntry entry) 
        {
            table.events.Add(entry);
        }

        public void RemoveFromTable(GummyCollection table, GummyRuleEntry entry) 
        {
            table.rules.Remove(entry);
        }

        public void RemoveFromTable(GummyCollection table, GummyFactEntry entry) 
        {
            table.facts.Remove(entry);
        }

        public void RemoveFromTable(GummyCollection table, GummyEventEntry entry) 
        {
            table.events.Remove(entry);
        }
    }
}