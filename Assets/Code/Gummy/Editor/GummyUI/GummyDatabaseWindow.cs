using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using Gummy.Blackboard;
using Gummy.Entries;
using Gummy.Shared;
using Gummy.Util;

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
        static SerializedObject serializedCurrentTable;
        static GummyBaseEntry currentEntry;
        static SerializedProperty serializedCurrentEntry;

        static string currentEntrySearchValue;
        static string currentTableSearchValue;

        static List<GummyCollection> filteredTables = new();
        
        [MenuItem("Window/GummyModern")]
        public static void ShowWindow()
        {
            GetWindow<GummyDatabaseWindow>(false, "Gummy", true);
            // InitializeIfNeeded();
            database = GummyUtil.database;
            serializedDatabase = GummyUtil.serializedDatabase;
            if(!applicationQuitEventAdded) {
                applicationQuitEventAdded = true;
                EditorApplication.wantsToQuit += OnEditorApplicationQuit;
            }
        }

        private void OnEnable() {
            entryStylesheet = Resources.Load<StyleSheet>("GummyEntryUSS");
            paneStylesheet = Resources.Load<StyleSheet>("GummyPaneStyle");
            factsDetailsStylesheet = Resources.Load<StyleSheet>("GummyFDVStyle");
            database = GummyUtil.database;
            serializedDatabase = GummyUtil.serializedDatabase;
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
                var title = item.Q<TextField>();
                serializedCurrentTable = new SerializedObject(filteredTables[index]);
                var sTableNameProperty = serializedCurrentTable.FindProperty("Name");
                title.BindProperty(sTableNameProperty);
                // title.text = filteredTables[index].Name;
            };
            tableListView.itemsSource = filteredTables;
            tableListView.fixedItemHeight = 30;
            tableListView.selectionChanged +=  (items) => HandleTableSelected(items, tableListView);
            return tableListView;
        }

        public VisualElement CreateEntriesListView(List<GummyBaseEntry> entries, SerializedProperty property, string title = "<EntryType>", bool addBottomBorder = true) {
            var root = new VisualElement();
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
            entriesListView.fixedItemHeight = 30;
            entriesListView.selectionChanged += (items) => HandleEntrySelected(items, entriesListView, property);
            root.Add(entriesListView);

            var target = entriesListView.Q<Toggle>();
            target.focusable = true;
            target.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) => {
                evt.menu.AppendAction("Create", (e) => {
                    Debug.Log("Create an entry via LV click!");
                });
            }));
            return root;
        }

        public VisualElement HandleEntryViewItemCreated() {
            var root = new VisualElement();
            root.focusable = true;
            root.styleSheets.Add(entryStylesheet);
            root.AddToClassList("gummy-entry");
            VisualElement nameValueElement = new();
            VisualElement alignmentContainer = new();
            alignmentContainer.AddToClassList("align-horizontal");
            nameValueElement.AddToClassList("align-horizontal");
            Label entryValue = new("0");
            entryValue.style.display = DisplayStyle.None;
            entryValue.AddToClassList("gummy-entry-value");
            entryValue.name = "Entry Value";
            TextField title = new("");
            title.AddToClassList("gc-editor-title");
            title.AddToClassList("gummy-entry-name");
            title.name = "Entry Title";
            Label type = new("");
            type.name = "Entry Type";
            nameValueElement.Add(entryValue);
            nameValueElement.Add(title);
            alignmentContainer.Add(nameValueElement);
            alignmentContainer.Add(type);
            root.Add(alignmentContainer);
            return root;
        }

        public void HandleEntryViewBind(VisualElement root, int index, List<GummyBaseEntry> entries, SerializedProperty property)
        {
            var title = (TextField)root.Q("Entry Title");
            var valueDisplay = (Label)root.Q("Entry Value");
            GummyBaseEntry entry = entries[index];
            int entryValue = database.GetBlackboardForEntry(entry).Get(entry.id);
            valueDisplay.text = $"{entryValue}";
            if (entryValue != 0) {
                valueDisplay.style.display = DisplayStyle.Flex;
            }
            title.RegisterCallback<ChangeEvent<string>>(
                e => {
                    if(currentEntry != null) {
                        if(factsDetailsView == null) return;
                        var fdvEntryTitle = factsDetailsView.Q<Label>("fdv-entry-title");
                        if(fdvEntryTitle == null) return;
                        fdvEntryTitle.text = $" / {currentEntry.key}";
                    }
                }
            );
            GummyUtil.OnEntryChanged += (int id, IGummyBlackboard context) => {
                if (id == entry.id) {
                    int currentValue = database.provider.GetBlackboard(entry.scope, context).Get(entry.id);
                    valueDisplay.text = $"{currentValue}";
                    if (currentValue != 0) {
                        valueDisplay.style.display = DisplayStyle.Flex;
                    }
                }
            };
            root.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) => {
                Debug.Log("Right click!");
                evt.menu.AppendAction("Create", (x) => {});
                evt.menu.AppendAction("Move To", (x) => {});
                evt.menu.AppendAction("Remove", (x) => {
                    property.DeleteArrayElementAtIndex(index);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }));
            var type = (Label)root.Q("Entry Type");
            string text = string.IsNullOrEmpty(entry.key) ? "<EntryTitle>" : entry.key;
            var descriptor = DescriptorCache.Descriptors[DescriptorCache.Instance.GetEntryDescriptorType(entry)];
            var serializedEntry = property.GetArrayElementAtIndex(index);
            title.BindProperty(serializedEntry.FindPropertyRelative("key"));
            type.text = descriptor.Name;
            type.style.color = descriptor.ParsedColor;
            root.style.display = DisplayStyle.Flex;
            if(!string.IsNullOrEmpty(currentEntrySearchValue) && !entry.key.ToLower().Contains(currentEntrySearchValue.ToLower())) {
                root.style.display = DisplayStyle.None;
            }
        }

        public void HandleEntrySelected(IEnumerable<object> selectedItems, ListView view, SerializedProperty property) {
            _selectedEntryIndex = view.selectedIndex;
            currentEntry = selectedItems.First() as GummyBaseEntry;
            serializedCurrentEntry = property.GetArrayElementAtIndex(_selectedEntryIndex);
            if(currentEntry == null) return;
            if(serializedCurrentEntry == null) return;
            factsDetailsView.Clear();
            
            var bar = new Toolbar();
            var titleHolder = new VisualElement();
            // var title = new Label();
            var tableTitle = new Label();
            var entryTitle = new Label();
            var entryIdLabel = new Label();

            tableTitle.name = "fdv-table-title";
            entryTitle.name = "fdv-entry-title";
            titleHolder.AddToClassList("fdv-title-holder");
            bar.AddToClassList("fdv-bar");
            entryIdLabel.AddToClassList("fdv-bar-id");

            // tableTitle.text = currentTable.Name;
            tableTitle.BindProperty(serializedCurrentTable.FindProperty("Name"));
            entryTitle.text = $"/ {currentEntry.key}";
            entryIdLabel.text = $"{currentEntry.id}";
            titleHolder.Add(tableTitle);
            titleHolder.Add(entryTitle);
            bar.Add(titleHolder);
            bar.Add(entryIdLabel);
            factsDetailsView.Add(bar);
            factsSplitView.Add(factsDetailsView);
            Debug.Log(currentEntry.id);
        }

        public void HandleTableSearch(string query) {
            currentTableSearchValue = query;
            tableListView.Rebuild();
        }

        public void HandleEntrySearch(string query) {
            currentEntrySearchValue = query;
            var eventLVRoot = factsPane.Q<VisualElement>("fp-lv-events");
            var eventLV = eventLVRoot.Q<ListView>();
            eventLV.Rebuild();
            // factsPane.Refresh();
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

            serializedCurrentTable = new SerializedObject(currentTable);
            var serializedFacts = serializedCurrentTable.FindProperty("facts");
            var serializedEvents = serializedCurrentTable.FindProperty("events");
            var serializedRules = serializedCurrentTable.FindProperty("rules");

            var factsSearchBar = CreateSearchbarSection();
            var factsList = CreateEntriesListView(ConvertTableToBaseEntries(currentTable.facts), serializedFacts, "Facts");
            var rulesList = CreateEntriesListView(ConvertTableToBaseEntries(currentTable.rules), serializedRules, "Rules");
            var eventsList = CreateEntriesListView(ConvertTableToBaseEntries(currentTable.events), serializedEvents, "Events", false);

            rulesList.name = "fp-lv-rules";
            factsList.name = "fp-lv-facts";
            eventsList.name = "fp-lv.events";

            factsPane.Add(factsSearchBar);
            factsPane.Add(factsList);
            factsPane.Add(rulesList);
            factsPane.Add(eventsList);

            var bar = new Toolbar();
            var title = new Label();

            title.text = currentTable.Name;
            title.BindProperty(serializedCurrentTable.FindProperty("Name"));
            bar.AddToClassList("fdv-bar");
            bar.Add(title);
            factsDetailsView.Add(bar);

            factsSplitView.Add(factsPane);
            factsSplitView.Add(factsDetailsView);
        }

        public VisualElement HandleTableViewItemCreated()
        {
            var root = new VisualElement();
            var titleLabel = new TextField();
            titleLabel.AddToClassList("gc-editor-title");
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
            if(database == null) return;
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