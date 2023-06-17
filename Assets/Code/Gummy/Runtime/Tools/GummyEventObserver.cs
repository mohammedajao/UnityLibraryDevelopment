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
    public class GummyEventObserver : MonoBehaviour
    {
        [SerializeField]
        public GummyEventBus eventBus;

        [SerializeField]
        public GummyEntryReference entry;

        internal EventBusAction Callback;

        private void OnEnable() {
            eventBus.AddListener(entry, OnEvent);
        }

        private void OnDisable() {
            eventBus.RemoveListener(entry, OnEvent);
        }

        protected virtual void OnEvent(int id, IGummyBlackboard context)
        {
            Callback?.Invoke(id, context);
        }
    }
}