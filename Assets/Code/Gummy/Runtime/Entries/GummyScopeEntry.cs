using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Shared;

namespace Gummy.Entries
{
    [Serializable]
    public class GummyScopeEntry : GummyFactEntry
    {
        public override GummyRuntimeEntryDescriptor descriptor => GummyRuntimeEntryDescriptor.ScopeDescriptor;
    }
}