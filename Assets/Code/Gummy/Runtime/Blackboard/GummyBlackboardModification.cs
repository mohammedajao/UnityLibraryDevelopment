using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.References;
using Gummy.Tools;
using Gummy.Shared;
using Gummy.Util;

namespace Gummy.Blackboard
{
    [Serializable]
    public class GummyBlackboardModification
    {
        internal enum ModificationMode
        {
            ADD,
            SET
        }
        [SerializeField] public GummyEntryReference reference;
        [SerializeField] internal ModificationMode mode = ModificationMode.SET;
        [SerializeField] public int number = 0;

        public void Mutate(GummyDatabase database)
        {
            if(reference == 0) return;
            var board = database.GetBlackboardForEntry(reference);
            if(mode == ModificationMode.SET) {
                board.Set(reference, number);
            } else {
                var currentValue = board.Get(reference);
                board.Set(reference, currentValue + number);
            }
            GummyUtil.RaiseEntryChanged(reference, board);
        }
    }
}