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

namespace TimeWizard.UnityEditor
{
    public class TimeWizardWindow : EditorWindow
    {
        public static event Action SnapshotUpdate;

        public SaveManager Manager => SaveManager.Instance;
        public SaveSnapshot currentSnapshot;

        [SerializeField] private Chunk[] _chunks = new Chunk[0];

        private List<ITimeWizardInterpreter> _editorInterpreters = new List<ITimeWizardInterpreter>() {
            new SaveMetadataInterpreter(),
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
                EditorGUILayout.Space();
            }  

            if(currentSnapshot != null) {
                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("Reload"))
                {
                    if(currentSnapshot != null && !string.IsNullOrWhiteSpace(currentSnapshot.Snapshot.Title))
                    {
                        SaveContext Save = Manager.GetSaveController(currentSnapshot.Snapshot.Title);
                        Task.WaitAll(Save.Create());
                        Manager.CaptureSnapshot();
                    } else {
                        TimeWizardUtils.LogWarning("Reloading denied. No snapshot is currently captured. Use the interface to attempt to capture one.");
                    }
                }
                if(GUILayout.Button("Save"))
                {
                    if(currentSnapshot != null && !string.IsNullOrWhiteSpace(currentSnapshot.Snapshot.Title)) {
                        Manager.ApplySnapshot(currentSnapshot.Snapshot.Title);
                        Manager.CaptureSnapshot();
                    } else {
                        TimeWizardUtils.LogWarning("Saving denied. No snapshot is currently captured. Use the interface to attempt to capture one.");
                    }
                }
                if(GUILayout.Button("Delete"))
                {
                    if(currentSnapshot != null && !string.IsNullOrWhiteSpace(currentSnapshot.Snapshot.Title)) {
                        Manager.DeleteSave(currentSnapshot.Snapshot.Title);
                        currentSnapshot.Snapshot.Title = "";
                        currentSnapshot = null;
                    } else {
                        TimeWizardUtils.LogWarning("Deletion denied. No snapshot is currently captured. Use the interface to attempt to capture one.");
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Catch Snapshot"))
            {
                GameObject context = GameObject.Find("Context");
                if(context != null)
                {
                    currentSnapshot = context.GetComponent<SaveSnapshot>();
                    if(string.IsNullOrWhiteSpace(currentSnapshot.Snapshot.Title))
                    {
                        currentSnapshot.Snapshot.Title = Guid.NewGuid().ToString();
                        TimeWizardUtils.Log($"Created new snapshot with ID {currentSnapshot.Snapshot.Title}");
                    }
                }
                Manager.CaptureSnapshot();
            }
            if(GUILayout.Button("Make snapshot"))
            {
                GameObject context = GameObject.Find("Context");
                if(context == null)
                {
                    context = new GameObject();
                    context.name = "Context";
                    context.AddComponent<SaveSnapshot>();
                }
                currentSnapshot = context.GetComponent<SaveSnapshot>();
                if(currentSnapshot == null)
                {
                    context.AddComponent<SaveSnapshot>();
                    currentSnapshot = context.GetComponent<SaveSnapshot>();
                    currentSnapshot.Snapshot.Title = Guid.NewGuid().ToString();
                    currentSnapshot = context.GetComponent<SaveSnapshot>();
                }
                currentSnapshot.Snapshot.Title = Guid.NewGuid().ToString();
                currentSnapshot = context.GetComponent<SaveSnapshot>();
                Manager.CaptureSnapshot();
            }
            if(currentSnapshot != null) {
                if(GUILayout.Button("Apply snapshot"))
                {
                    SnapshotUpdate?.Invoke();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnEnable()
        {
            foreach(var i in _editorInterpreters)
            {
                Manager.Register(i);
                i.OnEnable();
            }
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

