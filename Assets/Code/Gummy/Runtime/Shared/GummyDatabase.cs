using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Blackboard;
using Gummy.Entries;
using Gummy.References;
using Gummy.Tools;
using Gummy.Util;

namespace Gummy.Shared
{
    [Serializable]
    public class StringGummyCollectionMap : SerializedDictionary<string, GummyCollection> {};
    [Serializable]
    public class IntBaseEntryLookup : SerializedDictionary<int, GummyBaseEntry> {};
    [Serializable]
    public class ContextualBlackboard : SerializedDictionary<int, IGummyBlackboard> {}

    // DB has a provider object with GetBlackboardForEntry apparently
    [CreateAssetMenu(fileName = "GummyDatabase", menuName = "Gummy/Shared/GummyDatabase", order = 0)]
    public class GummyDatabase : ScriptableObject
    {
        [SerializeField] public IGummyDatabaseProvider provider;
        public GummyEventBus eventBus;

        [SerializeField] public ContextualBlackboard BlackboardContexts = new();

        private readonly Dictionary<int, GummyEventEntry> _eventLookup = new();
        private readonly Dictionary<int, List<GummyRuleEntry>> _ruleLookup = new();
        private readonly Dictionary<int, GummyBaseEntry> _entryLookup = new();

        [SerializeReference] public List<GummyCollection> tables = new();
        [SerializeReference] private StringGummyCollectionMap _setupTables = new();
        [SerializeReference] private StringGummyCollectionMap dirtyTables = new();

        private bool _requireCreateLookup = true;

        private void OnValidate() {
            RecreateTables();
        }

        private void OnEnable() {
            CreateLookupIfNecessary();
            Debug.Log("Running bindings");
            foreach(var kvp in _eventLookup) {
                var eventEntry = kvp.Value;
                Debug.Log($"Binding event {eventEntry.id}");
                eventBus.AddListener(eventEntry.id, HandleEntryEvent);
            }
        }

        public void RequireLookup() {
            _requireCreateLookup = true;
        }

        public void Initialize()
        {
            CreateLookupIfNecessary();
            foreach(var kvp in _eventLookup) {
                var eventEntry = kvp.Value;
                eventBus.AddListener(eventEntry.id, HandleEntryEvent);
            }
        }

        public List<GummyCollection> GetDirtyTables() => dirtyTables.Select(kvp => kvp.Value).ToList();
        public void ClearDirtyTables() { dirtyTables.Clear(); }

        public void RecreateTables()
        {
            // Remove all tables with missing references
            var toRemove = _setupTables.Where(kvp => !tables.Contains(kvp.Value)).ToList();
            foreach(var item in toRemove) {
                _setupTables.Remove(item.Key);
            }

            // Setup tables not currently being tracked
            foreach(var table in tables) {
                if(!(table is null) && !_setupTables.ContainsValue(table)) {
                    _setupTables.Add(table.Name, table);
                    table.Setup(this);
                    if(!dirtyTables.ContainsKey(table.Name)) dirtyTables.Add(table.Name, table);
                } else if(_setupTables.ContainsValue(table)) {
                    foreach(var kvp in _setupTables) {
                        if(kvp.Value == table) {
                            _setupTables.Remove(kvp.Key);
                            _setupTables[table.Name] = table;
                            break;
                        }
                    }
                }
            }
        }

        private void HandleEntryEvent(int id, IGummyBlackboard context)
        {
            Debug.Log($"[Gummy] Handling event {id}");
            if(_eventLookup.TryGetValue(id, out var entry) && !TestEntry(entry, context)) return;
            if(TryGetRule(id, context, out GummyRuleEntry match)) {
                ApplyEntry(match, context);
                var enumerator = match.Execute();
                while(enumerator.MoveNext()) {
                    var possibleEvent = enumerator.Current;
                    if (possibleEvent is int) {
                        int eventID = (int)possibleEvent;
                        eventBus.Invoke(eventID, context);
                    }
                }
                return;
            }
            ApplyEntry(entry, context);
        }

        public bool TryGetRule(int id, IGummyBlackboard context, out GummyRuleEntry match)
        {
            CreateLookupIfNecessary();
            match = null;
            if(_eventLookup.TryGetValue(id, out var eventEntry) && !TestEntry(eventEntry, context)) return false;

            if(_ruleLookup.TryGetValue(id, out var list)) {
                var targetWeight = int.MinValue;
                var initialIndex =  -1;
                var numberOfMatches = 0;
                List<GummyRuleEntry> rules = new();

                for(var index = 0; index < list.Count; index++) {
                    var response = list[index];
                    if (initialIndex < 0 && TestEntry(response, context)) {
                        targetWeight = response.Weight;
                        initialIndex = index;
                    }

                    if(response.Weight == targetWeight) {
                        rules.Add(response);
                        numberOfMatches++;
                    }
                }

                var rnd = new System.Random();
                int outputIndex = rnd.Next(rules.Count);
                match = rules[outputIndex];

                if (initialIndex > -1) {
                    if(eventEntry != null) {
                        ApplyEntry(eventEntry, context);
                    }
                }
            }

            if (match != null) return true;
            
            return false;
        }

        public IGummyBlackboard GetBlackboardForEntry(GummyEntryReference entry)
        {
            return EmptyBlackboard.Instance;
        }

        public bool TestEntry(GummyBaseEntry entry, IGummyBlackboard context)
        {
            if (entry.once && entry.scope != null && provider.GetBlackboard(entry.scope, context).Get(entry.id) != 0) {
                return false;
            }
            if(!_entryLookup.TryGetValue(entry.id, out var fact)) return false;
            for(int i = 0; i < fact.criteria.Length; i++) {
                if(!fact.criteria[i].Check(this)) return false;
            }
            return true;
        }

        private void CreateLookupIfNecessary()
        {
            Debug.Log($"Is lookup require? {_requireCreateLookup}");
            if(!_requireCreateLookup) {
                foreach(var table in tables)
                {
                    Debug.Log($"Create lookup for table {table.Name}");
                    foreach(var eventEntry in table.events)
                    {
                        _eventLookup[eventEntry.id] = eventEntry;
                        _entryLookup[eventEntry.id] = eventEntry;
                    }
                    foreach(var ruleEntry in table.rules)
                    {
                        if (ruleEntry.triggeredBy != 0) {
                            _ruleLookup[ruleEntry.triggeredBy].Add(ruleEntry);
                        }
                        _entryLookup[ruleEntry.id] = ruleEntry;
                    }
                    foreach(var factEntry in table.facts) 
                    {
                        _entryLookup[factEntry.id] = factEntry;
                    }
                }
                foreach(var kvp in _ruleLookup) {
                    var ruleList = kvp.Value;
                    _ruleLookup[kvp.Key] = ruleList.OrderBy(rule => rule.Weight).ToList();
                }
                _requireCreateLookup = false;
            }
        }

        public void ApplyEntry(GummyBaseEntry entry, IGummyBlackboard context)
        {
            var board = provider.GetBlackboard(entry.scope, context);
            int currentValue = board.Get(entry.id);
            board.Set(entry.id, currentValue + 1);
            GummyUtil.RaiseEntryChanged(entry.id,context);
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
            _entryLookup[id] = entry;
            if(EditorApplication.isPlaying)
            {
                EditorUtility.SetDirty(collection);
            }
            return entry;
        }
    }
}