using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Shared;
using Gummy.Blackboard;

namespace Gummy.Tools
{
    [Serializable]
    public class GummyModification
    {
        [SerializeField] public List<GummyBlackboardModification> modifications = new();

        public void Modify(GummyDatabase database)
        {
            foreach(var modification in modifications) {
                modification.Mutate(database);
            }
        }
    }
}