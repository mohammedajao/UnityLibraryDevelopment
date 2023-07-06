using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Shared;

namespace Gummy.Entries
{
    [Serializable]
    public class GummyFactEntry : GummyBaseEntry
    {
        public override void AddToTable(GummyCollection collection)
        {
            collection.facts.Add(this);
        }

        public override void RemoveFromTable(GummyCollection collection)
        {
            collection.facts.Remove(this);
        }
    }
}