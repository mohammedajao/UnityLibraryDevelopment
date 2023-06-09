using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;
using Gummy.Shared;

namespace Gummy.Editor
{
    public class EntryDescriptor
    {
        public Type RealType { get; internal set; }
        public virtual string Name => RealType.Name;
        public virtual GummyEntryType Type { get; protected set; } = GummyEntryType.Rule;
        public virtual string Color { get; protected set; } = "#ffffff";
        public virtual bool Optional { get; protected set; }
        public virtual bool HasCustomization => true;
        public virtual bool HasNavigation => true;

        public virtual void CreateNextMenu(GummyBaseEntry entry, List<string> names, List<int> ids, ref int current)
        {
        }

        public virtual void CreateAlternativeMenu(GummyBaseEntry entry, List<string> names, List<int> ids, ref int current)
        {
        }

        public virtual void CreatePreviousMenu(GummyBaseEntry entry, List<string> names, List<int> ids, ref int current)
        {
        }

        public virtual void HandleEntryCreated(GummyBaseEntry entry, GummyCollection table)
        {
        }

        public Color ParsedColor
        {
            get
            {
                ColorUtility.TryParseHtmlString(Color, out var color);
                return color;
            }
        }
        public EntryDescriptor() {}
    }
}