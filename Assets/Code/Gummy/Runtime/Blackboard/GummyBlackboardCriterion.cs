using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.References;
using Gummy.Shared;
using Gummy.Tools;

namespace Gummy.Blackboard
{
    [Serializable]
    public class GummyBlackboardCriterion
    {
        internal enum CompareMode
        {
            EQUALS_TO,
            LESS_THAN,
            GREATER_THAN,
            LESS_THAN_EQ,
            GREATER_THAN_EQ,
            ALIKE,
            NOT_ALIKE
        }

        [SerializeField] public GummyEntryReference reference;
        [SerializeField] internal CompareMode mode = CompareMode.EQUALS_TO;
        [SerializeField] public int comparand = 0;

        public virtual bool Check(GummyDatabase database) {
            if (reference == 0) return false;
            int referenceValue = database.GetBlackboardForEntry(reference).Get(reference);

            if (mode == CompareMode.ALIKE) { // Let editor show comparand as a fact
                if (comparand == 0) return false;
                int rhs = database.GetBlackboardForEntry(comparand).Get(comparand);
                return referenceValue == rhs;
            }

            if (mode == CompareMode.NOT_ALIKE) { // Let editor show comparand as a fact
                if (comparand == 0) return false;
                int rhs = database.GetBlackboardForEntry(comparand).Get(comparand);
                return referenceValue != rhs;
            }

            switch(mode)
            {
                case CompareMode.EQUALS_TO:
                    return referenceValue == comparand;
                case CompareMode.LESS_THAN:
                    return referenceValue < comparand;
                case CompareMode.LESS_THAN_EQ:
                    return referenceValue <= comparand;
                case CompareMode.GREATER_THAN:
                    return referenceValue > comparand;
                case CompareMode.GREATER_THAN_EQ:
                    return referenceValue >= comparand;
                default:
                    return false;
            }
        }
    }
}