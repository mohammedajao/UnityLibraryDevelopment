using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;
using TimeWizard.Core;
using Gummy.Blackboard;
using Gummy.Entries;
using Gummy.Shared;

[Serializable]
[CreateAssetMenu(fileName="BlackboardProvider", menuName="Gummy/Shared/BlackboardProvider")]
public class GameBlackboardProvider : GummyDatabaseProvider, ISaveStore
{
    public static readonly string Name = "BlackboardProvider";
    public SaveManager Manager => SaveManager.Instance;


    [Serializable]
    public class BlackboardSaveState {
        [SerializeField] public IGummyBlackboard blackboard;
        [SerializeField] public int scope;
        [SerializeField] public string chunk;
    }

    [Serializable]
    public class ScopeEntrySaveState
    {
        [SerializeField] public List<GummyScopeEntry> scopes = new();
    }

    private Dictionary<GummyScopeEntry, List<IGummyBlackboard>> _blackboardLookup = new();
    private Dictionary<int, GummyScopeEntry> _scopeList = new();
    private string[] _scopeKeys = { "default", "areas", "scenes" };

    public GameBlackboardProvider()
    {
        foreach(var key in _scopeKeys)
        {
            byte[] gb = Guid.NewGuid().ToByteArray();
            int id = BitConverter.ToInt32(gb,0);
            GummyScopeEntry entry = new GummyScopeEntry();
            entry.id = id;
            entry.key = key;
            entry.chunkName = key;
            _scopeList.Add(id, entry);
        }
    }

    public List<GummyScopeEntry> GetScopes()
    {
        return _scopeList.Select(kvp => kvp.Value).ToList();
    }

    void OnEnable()
    {
        Manager.Register(this);
    }

    void OnDisable()
    {
        Manager.Unregister(this);
    }

    public string GetIdentifier()
    {
        return Name;
    }

    public void LoadChunkData(string chunkName, ChunkDataSegment info)
    {
        if(chunkName == "global")
        {
            var state = info.As<ScopeEntrySaveState>();
            _scopeList = state.scopes.ToDictionary(scope => scope.id, scope => scope);
        } else {
            var state = info.As<BlackboardSaveState>();
            if(!_scopeList.TryGetValue(state.scope, out GummyScopeEntry scope)) {
                scope = new GummyScopeEntry();
                scope.id = state.scope;
                scope.chunkName = state.chunk;
            }
            if(!_blackboardLookup.ContainsKey(scope))
            {
                _blackboardLookup.Add(scope, new List<IGummyBlackboard>());
            }
            _blackboardLookup[scope].Add(state.blackboard);
        }
    }

    public List<SaveState> FetchSaveStates()
    {
        List<SaveState> saveStates = new();
        foreach(var kvp in _blackboardLookup)
        {
            Debug.Log(kvp.Key);
            if(string.IsNullOrEmpty(kvp.Key.chunkName)) continue;
            foreach(var board in kvp.Value)
            {
                saveStates.Add(new SaveState() {
                    ChunkName = kvp.Key.chunkName,
                    Data = new BlackboardSaveState() {
                        blackboard = board,
                        scope = kvp.Key.id,
                        chunk = kvp.Key.chunkName
                    }
                });
            }
        }
        saveStates.Add(new SaveState() {
            ChunkName = "global",
            Data = new ScopeEntrySaveState() {
                scopes = _scopeList.Select(kvp => kvp.Value).ToList()
            }
        });
        return saveStates;
    }

    public IGummyBlackboard FetchBlackboard(int scopeID, int? blackboardID)
    {
        var scope = _scopeList[scopeID];
        var blackboardList = _blackboardLookup[scope];
        
        foreach(GummyBlackboard board in blackboardList)
        {
            if(board.ID == blackboardID) return board;
        }
        var blackboardBase = new BaseBlackboard();
        return blackboardBase;
    }

    public override IGummyBlackboard GetBlackboard(int scope, IGummyBlackboard context)
    {
        if(!_scopeList.ContainsKey(scope)) {
            return EmptyBlackboard.Instance;
        }
        var entry = _scopeList[scope];
        if(_blackboardLookup.TryGetValue(entry, out var blackboardList))
        {
            // Scene Context will state area & scene so we will determine based on active area and scene
            // return blackboard;
        }
        return EmptyBlackboard.Instance;
    }
}
