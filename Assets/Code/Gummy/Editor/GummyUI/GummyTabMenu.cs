using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Gummy.Editor
{
    public delegate void TabAction(int tabIndex);
    public class GummyTabMenu
    {
        public event TabAction OnTabSelected;

        private const string tabClassName = "gummy-ui-tab";
        private const string currentlySelectedTabClassName = "gummy-ui-current-tab";
        private const string tabNamePrefix = "gummy-tab__";
        private const string contentNamePrefix = "gummy-tab-content__";
        private const string contentAreaClassName = "gummy-tab-menu-content";
        private const string tabsBarClassName = "gummy-tabs-bar";

        private int _currentTabIndex;
        private readonly VisualElement wrapper;
        private VisualElement tabsBar;
        private VisualElement contentArea;

        private readonly List<VisualElement> tabContent = new();
        private readonly List<VisualElement> tabPills = new();

        public GummyTabMenu(VisualElement root)
        {
            wrapper = new VisualElement();
            wrapper.AddToClassList("gummy-tab-menu");
            StyleSheet style = Resources.Load<StyleSheet>("GummyTabMenuStyle");
            BuildUI(wrapper);
            root.Add(wrapper);
            wrapper.styleSheets.Add(style);
        }

        public void AddNewTab(string title, VisualElement content)
        {
            CreateNewTab(title);
            CreateTabContent(content);
        }

        private VisualElement CreateNewTab(string name)
        {
            VisualElement tab = new();
            Label title = new(name);
            tab.Add(title);

            tab.RegisterCallback<ClickEvent>((evt) => OnTabClicked(evt));
            
            string newTabName = $"{tabNamePrefix}{tabPills.Count + 1}";
            tab.name = newTabName;
            tab.AddToClassList(tabClassName);
            tabPills.Add(tab);
            tabsBar.Add(tab);
            return tab;
        }

        private VisualElement CreateTabContent(VisualElement content)
        {
            VisualElement container = new();
            string newContentName = $"{contentNamePrefix}{tabContent.Count + 1}";
            container.name = newContentName;
            container.Add(content);
            tabContent.Add(container);
            return container;
        }
        
        private VisualElement BuildUI(VisualElement root)
        {
            tabsBar = new VisualElement();
            contentArea = new VisualElement();

            contentArea.AddToClassList(contentAreaClassName);
            tabsBar.AddToClassList(tabsBarClassName);

            root.Add(tabsBar);
            root.Add(contentArea);
            return root;
        }

        private void OnTabClicked(ClickEvent evt)
        {
            VisualElement target = evt.currentTarget as VisualElement;
            int index = tabPills.IndexOf(target);
            if(!IsTabCurrentlySelected(target) && index > -1) {
                GetAllTabs().Where(
                    (tab) => tab != target && IsTabCurrentlySelected(tab)
                ).ForEach(UnselectTab);
                SelectTab(target, index);
                OnTabSelected?.Invoke(index);
            }
        }

        private void SelectTab(VisualElement tab, int index)
        {
            _currentTabIndex = index;
            tab.AddToClassList(currentlySelectedTabClassName);
            var currentContent = tabContent[index];
            contentArea.Add(currentContent);
        }

        private void UnselectTab(VisualElement tab)
        {
            tab.RemoveFromClassList(currentlySelectedTabClassName);
            contentArea.Clear();
            _currentTabIndex = -1;
        }

        private bool IsTabCurrentlySelected(VisualElement tab)
        {
            return tab.ClassListContains(currentlySelectedTabClassName);
        }

        private UQueryBuilder<VisualElement> GetAllTabs()
        {
            return wrapper.Query<VisualElement>(className: tabClassName);
        }
    }
}