using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Blackboard;
using Gummy.Entries;

namespace Gummy.Shared
{
    public abstract class IGummyDatabaseProvider : ScriptableObject
    {
        public abstract IGummyBlackboard GetBlackboard(int scope, IGummyBlackboard context);
    }
    public interface IGummyDatabase
    {
        IGummyBlackboard GetBlackboardForEntry(int fact);
        // Assign an entry to a blackboard
        void ApplyEntry(GummyBaseEntry entry, IGummyBlackboard context);
        // GummyBaseEntry CreateEntry(GummyCollection table, Type type);
        // Use below to attempt to fetch an entry and assign to a variable
        // Since our blackboards are based on fact scope, we check the entry's scope for the blackboard
        // We have global, area, scene, and temporary scopes.
        bool TryGetEntry(int id, out GummyBaseEntry candidate);
        bool TryGetRule(int id, IGummyBlackboard context, out GummyRuleEntry match);
        bool TryGetCollection(int id, out GummyCollection table);
        bool TestEntry(GummyBaseEntry entry, IGummyBlackboard context);
        void CreateLookupIfNecessary(); // This function could check the length of rule and event Lists in tables and recreate the lookup tables if it's lower or remove them
        Dictionary<GummyRuleEntry, IGummyBlackboard> GetRules(int entry);
    }
}