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
        public override GummyRuntimeEntryDescriptor descriptor => GummyRuntimeEntryDescriptor.EventDescriptor;

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