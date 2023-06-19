using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Blackboard;
using Gummy.Entries;
using Gummy.Shared;

[Serializable]
[CreateAssetMenu(fileName="BlackboardProvider", menuName="Gummy/Shared/BlackboardProvider")]
public class GameBlackboardProvider : IGummyDatabaseProvider
{
    public static readonly string Name = "BlackboardProvider";
    [SerializeField] private ContextualBlackboard Contexts = new();

    public void SetScopeBlackboard(int scope, BaseBlackboard context)
    {
        this.Contexts[scope] = context;
        this.BlackboardIdentifier.Set(scope, context.ID);
    }

    public override IGummyBlackboard GetBlackboard(int scope, IGummyBlackboard context)
    {
        var boardID = context.Get(scope);
        if(boardID != 0) {
            if(Contexts.ContainsKey(boardID)) {
                return Contexts[boardID];
            }
        }
        return EmptyBlackboard.Instance;
    }

    public GameBlackboardProvider() {
        this.BlackboardIdentifier = new BaseBlackboard();
    }
}
