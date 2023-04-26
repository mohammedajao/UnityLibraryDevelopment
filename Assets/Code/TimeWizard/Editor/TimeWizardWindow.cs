using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using TimeWizard.Core;
using TimeWizard;

namespace TimeWizard.UnityEditor
{
    public class TimeWizardWindow : EditorWindow
    {
        public SaveManager Manager => SaveManager.Instance;
        public SaveSnapshot currentSnapshot;

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

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Reload"))
            {
                GameObject context = GameObject.Find("Context");
                if(context != null)
                {
                    currentSnapshot = context.GetComponent<SaveSnapshot>();
                    if(currentSnapshot != null) {
                        SaveContext Save = Manager.GetSaveController(currentSnapshot.Snapshot.Title);
                        Task.WaitAll(Save.Create());
                    }
                }
                Manager.CaptureSnapshot(true);
            }
            if(GUILayout.Button("Save"))
            {

            }
            if(GUILayout.Button("Delete"))
            {

            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Catch snapshot"))
            {
                Manager.CaptureSnapshot();
                GameObject context = GameObject.Find("Context");
                if(context != null)
                {
                    currentSnapshot = context.GetComponent<SaveSnapshot>(); // Improve on this to get snapshot
                } else {
                    Debug.LogWarning("[TimeWizard] No save snapshot was found within the active scenes.");
                }
            }
            if(GUILayout.Button("Make snapshot"))
            {
                
            }
            if(GUILayout.Button("Apply snapshot"))
            {
                if(currentSnapshot != null) {
                    Manager.ApplySnapshot(currentSnapshot.Snapshot.Title);
                    Manager.CaptureSnapshot(true);
                } else {
                    Debug.LogWarning("[TimeWizard] No snapshot is currently captured. Use the interface to attempt to capture one.");
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

