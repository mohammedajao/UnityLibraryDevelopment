using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.References;
using Gummy.Shared;

namespace Gummy.Entries
{
    [Serializable]
    public class GummyEventEntry : GummyBaseEntry
    {

        public override void AddToTable(GummyCollection collection)
        {
            collection.events.Add(this);
        }

        public override void RemoveFromTable(GummyCollection collection)
        {
            collection.events.Remove(this);
        }
    }
}