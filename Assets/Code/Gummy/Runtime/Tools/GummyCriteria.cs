using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Blackboard;
using Gummy.Shared;

namespace Gummy.Tools
{
    [Serializable]
    public class GummyCriteria
    {
        [SerializeField]
        public List<GummyBlackboardCriterion> criteria = new();

        public bool Test(GummyDatabase database)
        {
            for(int i = 0; i < criteria.Count; i++) {
                var criterion = criteria[i];
                if(!criterion.Check(database)) return false;
            }
            return true;
        }
    }
}