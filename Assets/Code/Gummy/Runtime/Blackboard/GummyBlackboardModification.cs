using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.References;
using Gummy.Tools;
using Gummy.Shared;

namespace Gummy.Blackboard
{
    public class GummyBlackboardModification : GummyModification
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
            var board = database.GetBlackboardForEntry(reference);
            if(mode == ModificationMode.SET) {
                board.Set(reference, number);
            } else {
                var currentValue = board.Get(reference);
                board.Set(reference, currentValue + number);
            }
        }
    }
}