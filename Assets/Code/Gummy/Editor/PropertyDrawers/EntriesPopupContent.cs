using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using Gummy.Entries;
using Gummy.Util;
using Gummy.Shared;

public delegate void EPCSelection(int id);

// https://forum.unity.com/threads/changing-style-for-elements-using-showalternatingrowbackgrounds.1175354/
// https://docs.unity3d.com/Manual/TreeViewAPI.html
// https://docs.unity3d.com/2022.2/Documentation/Manual/UIE-ListView-TreeView.html
public class EntriesPopupContent : PopupWindowContent
{
    EPCSelection callback;
    StyleSheet mainSheet;
    public EntriesPopupContent(EPCSelection cb) {
        callback = cb;
        mainSheet = Resources.Load<StyleSheet>("GummyReferenceStyle");
        gcGroupData.Clear();
        foreach(var table in GummyUtil.database.tables) {
            List<EntryData> fts = new();
            List<EntryData> rls = new();
            List<EntryData> evt = new();
            foreach(var entry in table.facts) {
                fts.Add(new EntryData(entry.key, entry.id));
            }
            foreach(var entry in table.rules) {
                rls.Add(new EntryData(entry.key, entry.id));
            }
            foreach(var entry in table.events) {
                evt.Add(new EntryData(entry.key, entry.id));
            }
            List<GummyCollectionSubdata> subdata = new();
            if(fts.Count > 0) subdata.Add(new GummyCollectionSubdata("Facts", fts));
            if(rls.Count > 0) subdata.Add(new GummyCollectionSubdata("Rules", rls));
            if(evt.Count > 0) subdata.Add(new GummyCollectionSubdata("Events", evt));
            GummyCollectionData gcData = new GummyCollectionData(table.Name, subdata);
            gcGroupData.Add(gcData);
        }
    }
    public override void OnGUI(Rect rect)
    {

    }

    public override void OnOpen() {
        var root = editorWindow.rootVisualElement;
        root.styleSheets.Add(mainSheet);

        var tableList = new TreeView();

        Action<IEnumerable<int>> onSelectionChanged = selectedIndices =>
        {
            if (!selectedIndices.Any())
                return;
            var sampleItem = tableList.GetItemDataForIndex<IGummyCollectionOrEntries>(selectedIndices.First());
            if(sampleItem.GetType() == typeof(EntryData)) {
                var entryInfo = (EntryData)sampleItem;
                callback(entryInfo.entryID);
            }
        };

        tableList.SetRootItems(treeRoots);
        tableList.makeItem = () => new Label();
        tableList.bindItem = (item, index) => { 
            var title = item.Q<Label>();
            title.text = tableList.GetItemDataForIndex<IGummyCollectionOrEntries>(index).name;
        };
        tableList.selectedIndicesChanged += onSelectionChanged;
        tableList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
        root.Add(tableList);
    }

    protected static readonly List<GummyCollectionData> gcGroupData = new List<GummyCollectionData>();

    protected static IList<TreeViewItemData<IGummyCollectionOrEntries>> treeRoots
    {
        get
        {
            int id = 0;
            var roots = new List<TreeViewItemData<IGummyCollectionOrEntries>>(gcGroupData.Count);
            foreach (GummyCollectionData table in gcGroupData)
            {
                var container = new List<TreeViewItemData<IGummyCollectionOrEntries>>(table.container.Count);
                if(table.container.Count < 3) {
                    GummyCollectionSubdata newData;
                    foreach(var subdata in table.container) {
                        var entries = new List<TreeViewItemData<IGummyCollectionOrEntries>>(subdata.entries.Count);
                        newData = new GummyCollectionSubdata($"{table.name}/{subdata.name}", subdata.entries);
                        foreach(var entry in subdata.entries) {
                            entries.Add(new TreeViewItemData<IGummyCollectionOrEntries>(id++, entry));
                        }
                        var treeNewData = new TreeViewItemData<IGummyCollectionOrEntries>(id++, newData, entries);
                        roots.Add(treeNewData);
                    }
                    continue;
                }
                foreach (var subdata in table.container)
                {
                    if(subdata.entries.Count == 0) continue;

                    var entries = new List<TreeViewItemData<IGummyCollectionOrEntries>>(subdata.entries.Count);
                    foreach(var entry in subdata.entries) {
                        entries.Add(new TreeViewItemData<IGummyCollectionOrEntries>(id++, entry));
                    }
                    container.Add(new TreeViewItemData<IGummyCollectionOrEntries>(id++, subdata, entries));
                }

                roots.Add(new TreeViewItemData<IGummyCollectionOrEntries>(id++, table, container));
            }
            return roots;
        }
    }

    protected interface IGummyCollectionOrEntries
    {
        public string name { get; set; }
        public int id { get; }
        public bool populated { get; }
    }

    protected class EntryData : IGummyCollectionOrEntries
    {
        public string name { get; set; }
        public int id { get; }
        public int entryID { get; }
        public bool populated { get; }
        public EntryData(string name, int entryID, bool populated = false)
        {
            this.name = name;
            this.populated = populated;
            this.entryID = entryID;
        }
    }

    protected class GummyCollectionSubdata : IGummyCollectionOrEntries
    {
        public string name { get; set; }
        public int id { get; }
        public bool populated { 
            get {
                var anyEntryPopulated = true;
                foreach(EntryData entry in entries) 
                {
                    anyEntryPopulated = anyEntryPopulated || entry.populated;
                }
                return anyEntryPopulated;
            }
         }
        public readonly IReadOnlyList<EntryData> entries;

        public GummyCollectionSubdata(string name, IReadOnlyList<EntryData> entries) {
            this.id = 0;
            this.name = name;
            this.entries = entries;
        }
    }

    protected class GummyCollectionData : IGummyCollectionOrEntries 
    {
        public string name { get; set; }
        public int id { get; }

        public readonly IReadOnlyList<GummyCollectionSubdata> container;
        public bool populated { 
            get
            {
                var anySubdataPopulated = false;
                foreach(GummyCollectionSubdata subdata in container) 
                {
                    anySubdataPopulated  = anySubdataPopulated  || subdata.populated;
                }
                return anySubdataPopulated ;
            }
        }
        public GummyCollectionData(string name, IReadOnlyList<GummyCollectionSubdata> data)
        {
            this.name = name;
            this.id = 0;
            this.container = data;
        }
    }
}
