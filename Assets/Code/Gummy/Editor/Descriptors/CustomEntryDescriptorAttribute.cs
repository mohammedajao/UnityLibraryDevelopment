using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gummy.Entries;
using Gummy.Shared;

namespace Gummy.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomEntryDescriptorAttribute : PropertyAttribute
    {
        public Type type;
        public CustomEntryDescriptorAttribute(Type type)
        {
            this.type = type;
        }
    }
}