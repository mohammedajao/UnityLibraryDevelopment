using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using Gummy.Entries;
using Gummy.Shared;

namespace Gummy.Editor
{
    public class GummyDatabaseWindow : EditorWindow
    {
        static GummyDatabase database;
        static SerializedObject serializedDatabase;

        static bool applicationQuitEventAdded = false;

        static TwoPaneSplitView factsSplitView;
        static VisualElement factsPane;
        static ListView tableListView;
        static VisualElement factsDetailsView;

        static StyleSheet entryStylesheet;
        static StyleSheet paneStylesheet;
        static StyleSheet factsDetailsStylesheet;

        // View Persistence
        [SerializeField] private int _selectedTableIndex = -1;
        [SerializeField] private int _selectedEntryIndex = -1;
        static GummyCollection currentTable;
        static GummyBaseEntry currentEntry;

        static string currentEntrySearchValue;
        static string currentTableSearchValue;

        static List<GummyCollection> filteredTables = new();
        
        [MenuItem("Window/GummyModern")]
        public static void ShowWindow()
        {
            GetWindow<GummyDatabaseWindow>(false, "Gummy", true);
            InitializeIfNeeded();
            if(!applicationQuitEventAdded) {
                applicationQuitEventAdded = true;
                EditorApplication.wantsToQuit += OnEditorApplicationQuit;
            }
        }

        private void OnEnable() {
            entryStylesheet = Resources.Load<StyleSheet>("GummyEntryUSS");
            paneStylesheet = Resources.Load<StyleSheet>("GummyPaneStyle");
            factsDetailsStylesheet = Resources.Load<StyleSheet>("GummyFDVStyle");
        }

        public List<GummyCollection> SetTableLVData()
        {
            filteredTables = database.tables.Where(table => {
                if(string.IsNullOrEmpty(currentTableSearchValue)) return true;
                return table.Name.ToLower().Contains(currentTableSearchValue.ToLower());
            }).ToList();
            tableListView.itemsSource = filteredTables;
            return filteredTables;
        }

        public ListView CreateGummyTablesListView() {
            tableListView = new ListView();
            filteredTables = database.tables;
            tableListView.selectedIndex = _selectedTableIndex;
            tableListView.makeItem = () => HandleTableViewItemCreated();
            tableListView.bindItem = (item, index) => {
                var title = item.Q<Label>();
                title.text = filteredTables[index].Name;
            };
            tableListView.itemsSource = filteredTables;
            tableListView.fixedItemHeight = 30;
            tableListView.selectionChanged +=  (items) => HandleTableSelected(items, tableListView);
            return tableListView;
        }

        public ListView CreateEntriesListView(List<GummyBaseEntry> entries, SerializedProperty property, string title = "<EntryType>", bool addBottomBorder = true) {
            var entriesListView = new ListView();
            if(addBottomBorder) {
                entriesListView.AddToClassList("entries-group");
            } else {
                entriesListView.AddToClassList("entries-group-nb");
            }
            entriesListView.headerTitle = title;
            entriesListView.showFoldoutHeader = true;
            entriesListView.showBoundCollectionSize = false;
            entriesListView.selectedIndex = _selectedEntryIndex;
            entriesListView.makeItem = () => HandleEntryViewItemCreated();
            entriesListView.bindItem = (item, index) => HandleEntryViewBind(item, index, entries, property);
            entriesListView.itemsSource = entries;
            entriesListView.itemHeight = 30;
            entriesListView.selectionChanged += (items) => HandleEntrySelected(items, entriesListView);
            return entriesListView;
        }

        public VisualElement HandleEntryViewItemCreated() {
            var root = new VisualElement();
            root.styleSheets.Add(entryStylesheet);
            root.AddToClassList("gummy-entry");
            VisualElement alignmentContainer = new();
            root.Add(alignmentContainer);
            alignmentContainer.AddToClassList("align-horizontal");
            Label title = new("");
            title.AddToClassList("gummy-entry-name");
            title.name = "Entry Title";
            title.RegisterCallback<ChangeEvent<string>>(
                e => { if(currentEntry != null) currentEntry.key = (e.target as Label).text; });

            Label type = new("");
            type.name = "Entry Type";
            alignmentContainer.Add(title);
            alignmentContainer.Add(type);
            return root;
        }

        public void HandleEntryViewBind(VisualElement root, int index, List<GummyBaseEntry> entries, SerializedProperty property)
        {
            var title = (Label)root.Q("Entry Title");
            var type = (Label)root.Q("Entry Type");
            GummyBaseEntry entry = entries[index];
            string text = string.IsNullOrEmpty(entry.key) ? "<EntryTitle>" : entry.key;
            var descriptor = DescriptorCache.Descriptors[DescriptorCache.Instance.GetEntryDescriptorType(entry)];
            // var entryKeySO = new SerializedObject(entry.key);
            title.text = text;
            // title.Bind(property.GetArrayElementAtIndex(index).FindPropertyRelative("key"));
            type.text = descriptor.Name;
            type.style.color = descriptor.ParsedColor;
            // title.Bind(entryKeySO);
        }

        public void HandleEntrySelected(IEnumerable<object> selectedItems, ListView view) {
            _selectedEntryIndex = view.selectedIndex;
            currentEntry = selectedItems.First() as GummyBaseEntry;
            if(currentEntry == null) return;
            factsDetailsView.Clear();
            
            var bar = new Toolbar();
            var title = new Label();
            var entryIdLabel = new Label();

            bar.AddToClassList("fdv-bar");
            entryIdLabel.AddToClassList("fdv-bar-id");

            title.text = $"{currentTable.Name} / {currentEntry.key}";
            entryIdLabel.text = $"{currentEntry.id}";
            bar.Add(title);
            bar.Add(entryIdLabel);
            factsDetailsView.Add(bar);
            factsSplitView.Add(factsDetailsView);
            Debug.Log(currentEntry.id);
        }

        public void HandleTableSearch(string query) {
            currentTableSearchValue = query;
            tableListView.Rebuild();
        }

        public List<GummyBaseEntry> ConvertTableToBaseEntries<T>(List<T> entries) where T : GummyBaseEntry
        {
            return entries.Select(item => (GummyBaseEntry)item).ToList();
        }

        public void HandleTableSelected(IEnumerable<object> selectedItems, ListView view)
        {
            factsSplitView.Clear();
            factsPane.Clear();
            factsDetailsView.Clear();
            _selectedTableIndex = view.selectedIndex;
            currentTable = selectedItems.First() as GummyCollection;
            if(currentTable == null) return;
            Debug.Log(currentTable.Name);

            var serializedTable = new SerializedObject(currentTable);
            var serializedFacts = serializedTable.FindProperty("facts");
            var serializedEvents = serializedTable.FindProperty("events");
            var serializedRules = serializedTable.FindProperty("rules");

            var factsSearchBar = CreateSearchbarSection();
            var factsList = CreateEntriesListView(ConvertTableToBaseEntries(currentTable.facts), serializedFacts, "Facts");
            var rulesList = CreateEntriesListView(ConvertTableToBaseEntries(currentTable.rules), serializedRules, "Rules");
            var eventsList = CreateEntriesListView(ConvertTableToBaseEntries(currentTable.events), serializedEvents, "Events", false);
            factsPane.Add(factsSearchBar);
            factsPane.Add(factsList);
            factsPane.Add(rulesList);
            factsPane.Add(eventsList);

            var bar = new Toolbar();
            var title = new Label();

            title.text = currentTable.Name;
            bar.AddToClassList("fdv-bar");
            bar.Add(title);
            factsDetailsView.Add(bar);

            factsSplitView.Add(factsPane);
            factsSplitView.Add(factsDetailsView);
        }

        public VisualElement HandleTableViewItemCreated()
        {
            var root = new VisualElement();
            var titleLabel = new Label();
            root.Add(titleLabel);
            root.AddToClassList("gc-name");
            return root;
        }

        public VisualElement CreateSearchbarSection()
        {
            var root = new VisualElement();
            root.AddToClassList("searchbar-container");
            var searchbar = new ToolbarSearchField();
            var spacer = new ToolbarSpacer();
            searchbar.AddToClassList("pane-searchbar");
            root.Add(searchbar);
            root.Add(spacer);
            return root;
        }

        public void CreateGUI()
        {
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            var tablePane = new VisualElement();
            var tablePaneSearchbar = CreateSearchbarSection();
            var tableSearchbar = tablePaneSearchbar.Q<ToolbarSearchField>();
            // tablePaneSearchbar.searchButton = new Button();
            // tablePaneSearchbar.paddingRight = -30;
            factsSplitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            factsPane = new VisualElement();
            factsDetailsView = new VisualElement();
            var tablesListView = CreateGummyTablesListView();

            tableSearchbar.RegisterCallback<ChangeEvent<string>>(e => {
                currentTableSearchValue = (e.target as ToolbarSearchField).value;
                SetTableLVData();
                tablesListView.Rebuild();
            });

            tablePane.AddToClassList("gummy-pane");
            factsPane.AddToClassList("gummy-pane");

            tablePane.Add(tablePaneSearchbar);
            tablePane.Add(tablesListView);
        
            splitView.Add(tablePane);
            splitView.Add(factsSplitView);
            factsSplitView.Add(factsPane);
            factsSplitView.Add(factsDetailsView);

            rootVisualElement.Add(splitView);
            splitView.styleSheets.Add(paneStylesheet);
            factsDetailsView.styleSheets.Add(paneStylesheet);
            factsDetailsView.styleSheets.Add(factsDetailsStylesheet);
        }

        [InitializeOnLoadMethod]
        public static void InitializeIfNeeded()
        {
            if(database == null) {
                var assets = AssetDatabase.FindAssets("GummyDatabase", new[] {
                    "Assets/GameTools/Gummy"
                });
                if(assets.Length == 0) return;
                var asset = assets[0];
                var assetPath = AssetDatabase.GUIDToAssetPath(asset);
                database = (GummyDatabase)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GummyDatabase));
                serializedDatabase = new SerializedObject(database);
            }
            if(serializedDatabase != null) {
              serializedDatabase.Update(); 
            }
            if(!applicationQuitEventAdded) {
                applicationQuitEventAdded = true;
                EditorApplication.wantsToQuit += OnEditorApplicationQuit;
            }
        }

        static bool OnEditorApplicationQuit()
        {
            if(database == null) return true;
            foreach(var table in database.GetDirtyTables()) {
                EditorUtility.SetDirty(table);
            }
            database.ClearDirtyTables();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return true;
        }
    }
}