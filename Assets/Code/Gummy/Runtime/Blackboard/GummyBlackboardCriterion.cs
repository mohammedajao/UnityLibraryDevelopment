using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.References;
using Gummy.Tools;

namespace Gummy.Blackboard
{
    [Serializable]
    public class GummyBlackboardCriterion
    {

        public enum CompareMode
        {
            EQUALS_TO,
            LESS_THAN,
            GREATER_THAN,
            LESS_THAN_EQ,
            GREATER_THAN_EQ
        }

        [SerializeField] public GummyEntryReference reference;
        [SerializeField] public CompareMode mode = CompareMode.EQUALS_TO;
        [SerializeField] public bool UseConstant = true;
        [SerializeField] public int comparand = 0;

        public bool Check() {
            int referenceValue = 0; // TODO replace with Get reference value
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