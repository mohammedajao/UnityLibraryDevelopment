using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Shared;

namespace Gummy.Entries
{
    [Serializable]
    public class GummyScopeEntry : GummyBaseEntry
    {
        public string chunkName = "default";

        public override void AddToTable(GummyCollection collection)
        {
        }

        public override void RemoveFromTable(GummyCollection collection)
        {
        }
    }
}