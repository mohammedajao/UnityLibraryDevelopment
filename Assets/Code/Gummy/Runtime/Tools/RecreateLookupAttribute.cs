using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Util;

namespace Gummy.Tools
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RecreateLookupAttribute : Attribute
    {
        public RecreateLookupAttribute() {}
    }
}