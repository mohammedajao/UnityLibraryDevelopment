using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Blackboard;
using Gummy.Entries;

namespace Gummy.Shared
{
    // DB has a provider object with GetBlackboardForEntry apparently
    [CreateAssetMenu(fileName = "GummyDatabase", menuName = "Gummy/Shared/GummyDatabase", order = 0)]
    public class GummyDatabase : ScriptableObject
    {
        [SerializeField] public GummyDatabaseProvider provider;

        private readonly Dictionary<int, GummyEventEntry> _eventLookup = new();
        private readonly Dictionary<int, List<GummyRuleEntry>> _ruleLookup = new();

        // I think this returns the next rule given a rule
        public bool TryGetRule(int id, IGummyBlackboard context, out GummyRuleEntry match)
        {
            // Create Lookup if necessary
            CreateLookupIfNecessary();
            match = null;
            if(_eventLookup.TryGetValue(id, out var entry) && !TestEntry(entry, context)) return false;
            
            // Check rule lookup
            // Find responses that satisfy the context (TestEntry)
            // Choose the best response
            return false;
        }

        public bool TestEntry(GummyBaseEntry entry, IGummyBlackboard context)
        {
            if(entry.scope != null && provider.GetBlackboard(entry.scope, context).Get(entry.id) != 0) return true;
            return false;
        }

        private void CreateLookupIfNecessary()
        {
            // idk
        }
    
        public GummyBaseEntry CreateEntry(GummyCollection collection, Type entryType)
        {
            int id = Guid.NewGuid().GetHashCode();
            GummyBaseEntry entry = null;
            if(entryType == typeof(GummyRuleEntry))
                entry = new GummyRuleEntry() { id = id };
            else if(entryType == typeof(GummyEventEntry))
                entry = new GummyEventEntry() { id = id };
            else if(entryType == typeof(GummyFactEntry))
                entry = new GummyFactEntry() { id = id };
            entry.AddToTable(collection);
            return entry;
        }
    }
}