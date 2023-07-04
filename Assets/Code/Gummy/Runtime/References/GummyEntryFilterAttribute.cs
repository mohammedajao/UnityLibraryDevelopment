using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;

namespace Gummy.References
{
    [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public class GummyEntryFilterAttribute : Attribute
    {
        public GummyEntryFilterType EntryType;
        public string TableName;
        public bool AllowEmpty = true;

        public GummyEntryFilterAttribute(GummyEntryFilterType EntryType = (GummyEntryFilterType)15, string TableName = null, bool AllowEmpty = true)
        {
            this.EntryType = EntryType;
            this.TableName = TableName;
            this.AllowEmpty = AllowEmpty;
        }
    }
}