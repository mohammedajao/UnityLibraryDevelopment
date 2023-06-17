using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Blackboard;
using Gummy.Entries;
using Gummy.Shared;

[Serializable]
public class ScopesBlackboardMap : SerializedDictionary<int, IGummyBlackboard> {}

[Serializable]
[CreateAssetMenu(fileName="BlackboardProvider", menuName="Gummy/Shared/BlackboardProvider")]
public class GameBlackboardProvider : IGummyDatabaseProvider
{
    public static readonly string Name = "BlackboardProvider";
    public BaseBlackboard BlackboardIdentities = new();
    public ScopesBlackboardMap ScopesBlackboards = new();

    public void SetScopeBlackboard(int scope, IGummyBlackboard context)
    {
        ScopesBlackboards[scope] = context;
    }

    public override IGummyBlackboard GetBlackboard(int scope, IGummyBlackboard context)
    {
        if(ScopesBlackboards.ContainsKey(scope))
        {
            return ScopesBlackboards[scope];
        }
        return EmptyBlackboard.Instance;
    }
}
