using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Gummy.Blackboard;
using Gummy.Entries;
using Gummy.References;
using Gummy.Shared;

namespace Gummy.Tools
{
    [Serializable]
    public class GummyEvent
    {
        [SerializeField]
        [GummyEntryFilter(EntryType = GummyEntryFilterType.Event|GummyEntryFilterType.Rule)]
        public GummyEntryReference eventReference; // rule or entry

        [SerializeField]
        public IGummyBlackboard context;

        public void Invoke(GummyEventBus eventBus)
        {
            Assert.IsNotNull(eventBus, "[Gummy]: Missing EventBus. Event will not be invoked");
            if(eventReference.HasValue)
            {
                eventBus.Invoke(eventReference, context);
            }
        }

        public void Invoke(GummyEventBus eventBus, GummyBlackboard customContext)
        {
            Assert.IsNotNull(eventBus, "[Gummy]: Missing EventBus. Event will not be invoked");
            if(eventReference.HasValue)
            {
                eventBus.Invoke(eventReference, customContext);
            }
        }

        [Serializable]
        internal struct Triggerable
        {
            [GummyEntryFilter(EntryType = GummyEntryFilterType.Event)]
            public GummyEntryReference reference;
        }
    }
}