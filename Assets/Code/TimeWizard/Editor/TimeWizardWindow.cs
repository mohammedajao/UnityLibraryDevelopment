using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEditor;
using TimeWizard.Core;
using TimeWizard.Util;
using TimeWizard;

// Catch snapshot - IDK yet
// Make Snapshot - Captures snapshot and changes of current stores and interpreters into window
// Apply Snapshot - Applies changes made in window to world
// Reload, Save, Delete - Affect actual files

namespace TimeWizard.UnityEditor
{
    public class TimeWizardWindow : EditorWindow
    {
        public static event Action SnapshotUpdate;

        public SaveManager Manager => SaveManager.Instance;
        public SaveSnapshot currentSnapshot;

        [SerializeField] private Chunk[] _chunks = new Chunk[0];

        private List<ITimeWizardInterpreter> _editorInterpreters = new List<ITimeWizardInterpreter>() {
            new ChunksListInterpreter()
        };

        [MenuItem("Window/TimeWizard")]
        public static void ShowWindow()
        {
            GetWindow<TimeWizardWindow>(false, "TimeWizard", true);
        }

        void OnGUI()
        {
            foreach(var i in _editorInterpreters)
            {
                i.DrawInspectorGUI();
            }  

            if(currentSnapshot != null) {
                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("Reload"))
                {
                    if(currentSnapshot != null)
                    {
                        SaveContext Save = Manager.GetSaveController(currentSnapshot.Snapshot.Title);
                        Task.WaitAll(Save.Create());
                        Manager.CaptureSnapshot();
                    }
                }
                if(GUILayout.Button("Save"))
                {
                    if(currentSnapshot != null) {
                        Manager.ApplySnapshot(currentSnapshot.Snapshot.Title);
                        Manager.CaptureSnapshot();
                    } else {
                        TimeWizardUtils.LogWarning("[TimeWizard] No snapshot is currently captured. Use the interface to attempt to capture one.");
                    }
                }
                if(GUILayout.Button("Delete"))
                {

                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Catch Snapshot"))
            {
                GameObject context = GameObject.Find("Context");
                if(context != null)
                {
                    currentSnapshot = context.GetComponent<SaveSnapshot>();
                }
                Manager.CaptureSnapshot();
            }
            if(currentSnapshot != null) {
                if(GUILayout.Button("Make snapshot"))
                {
                    Manager.CaptureSnapshot();
                }
                if(GUILayout.Button("Apply snapshot"))
                {
                    SnapshotUpdate?.Invoke();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnEnable()
        {
            // Manager.CaptureSnapshot();
            foreach(var i in _editorInterpreters)
            {
                Manager.Register(i);
                i.OnEnable();
            }
            // Manager.CaptureSnapshot();
        }

        private void OnDisable()
        {
            foreach(var i in _editorInterpreters)
            {
                Manager.Unregister(i);
                i.OnDisable();
            }  
        }
    }
}

