using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Blackboard;
using Gummy.Entries;

namespace Gummy.Shared
{
    [Serializable]
    public class StringGummyCollectionMap : SerializedDictionary<string, GummyCollection> {};

    // DB has a provider object with GetBlackboardForEntry apparently
    [CreateAssetMenu(fileName = "GummyDatabase", menuName = "Gummy/Shared/GummyDatabase", order = 0)]
    public class GummyDatabase : ScriptableObject
    {
        [SerializeField] public GummyDatabaseProvider provider;
        public GummyEventBus eventBus;

        private readonly Dictionary<int, GummyEventEntry> _eventLookup = new();
        private readonly Dictionary<int, List<GummyRuleEntry>> _ruleLookup = new();

        [SerializeReference] public List<GummyCollection> tables = new();
        [SerializeReference] private StringGummyCollectionMap _setupTables = new();
        [SerializeReference] private StringGummyCollectionMap dirtyTables = new();

        private void OnValidate() {
            RecreateTables();
        }

        public List<GummyCollection> GetDirtyTables() => dirtyTables.Select(kvp => kvp.Value).ToList();
        public void ClearDirtyTables() { dirtyTables.Clear(); }

        public void RecreateTables()
        { //-50106142 -1040841110
            // Remove all tables with missing references
            var toRemove = _setupTables.Where(kvp => !tables.Contains(kvp.Value)).ToList();
            foreach(var item in toRemove) {
                _setupTables.Remove(item.Key);
            }

            // Setup tables not currently being tracked
            foreach(var table in tables) {
                if(!(table is null) && !_setupTables.ContainsKey(table.Name)) {
                    _setupTables.Add(table.Name, table);
                    table.Setup(this);
                    if(!dirtyTables.ContainsKey(table.Name)) dirtyTables.Add(table.Name, table);
                }
            }
        }

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
            CreateLookupIfNecessary();
            int id = Guid.NewGuid().GetHashCode();
            GummyBaseEntry entry = null;
            if(entryType == typeof(GummyRuleEntry)) {
                GummyRuleEntry newRule = new GummyRuleEntry() { id = id };
                newRule.AddToTable(collection);
                entry = newRule;
            } else if(entryType == typeof(GummyEventEntry)) {
                GummyEventEntry newEvent = new GummyEventEntry() { id = id };
                newEvent.AddToTable(collection);
                entry = newEvent;
            } else if(entryType == typeof(GummyFactEntry)) {
                GummyFactEntry newFact = new GummyFactEntry() { id = id };
                newFact.AddToTable(collection);
                entry = newFact;
            }
            if(EditorApplication.isPlaying)
            {
                EditorUtility.SetDirty(collection);
            }
            return entry;
        }
    }
}