using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Shared;
using Gummy.Entries;

namespace Gummy.Util {
    public delegate void TableAction(GummyCollection table);
    public delegate void EntryAction(GummyBaseEntry entry);

    public static class GummyUtil
    {
        public static event TableAction OnTableSelected;
        public static event EntryAction OnEntrySelected;

        public static void RaiseTableSelected(GummyCollection table)
        {
            OnTableSelected?.Invoke(table);
        }
    }
}
