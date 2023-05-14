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
        public List<GummyBlackboardCriterion> criterion = new();

        public bool Test(GummyDatabase database)
        {
            return false;
        } 
    }
}