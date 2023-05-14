using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gummy.Blackboard
{
    [Serializable]
    public class EmptyBlackboard : GummyBlackboard // Used for temporary fact scopes
    {
        private static IGummyBlackboard _instance;

        public static IGummyBlackboard Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EmptyBlackboard();
                }
                return _instance;
            }
        }
    }
}