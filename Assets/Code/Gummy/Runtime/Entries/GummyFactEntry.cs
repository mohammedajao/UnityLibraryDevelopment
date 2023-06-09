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
            descriptor.AddToTable(collection, this);
        }

        public override void RemoveFromTable(GummyCollection collection)
        {
            descriptor.RemoveFromTable(collection, this);
        }
    }
}