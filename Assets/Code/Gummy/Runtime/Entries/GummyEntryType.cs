using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gummy.Entries
{
    [Serializable]
    public class GummyEntryType
    {
        public static readonly GummyEntryType Fact = new GummyEntryType(typeof(GummyFactEntry));
        public static readonly GummyEntryType Event = new GummyEntryType(typeof(GummyEventEntry));
        public static readonly GummyEntryType Rule = new GummyEntryType(typeof(GummyRuleEntry));

        public Type internalValue { get; protected set; }
        protected GummyEntryType(Type internalValue)
        {
            this.internalValue = internalValue;
        }
    }
}
