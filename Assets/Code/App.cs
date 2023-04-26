using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TimeWizard.Core;
using TimeWizard.Persistence;
using TimeWizard;

public class App : UnitySingleton<App>
{
    public SaveManager SaveService => SaveManager.Instance;
    public SaveSnapshot currentSnapshot;

    public bool IsEditor = false;

    internal class ApplicationException : System.Exception
    {
        public ApplicationException() { }
        public ApplicationException(string message) : base(message) { }
        public ApplicationException(string message, System.Exception inner) : base(message, inner) { }
        protected ApplicationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        var app = Object.Instantiate(Resources.Load("App")) as GameObject;
        if(app == null)
            throw new ApplicationException();
        
        Object.DontDestroyOnLoad(app);
    }

    protected override void SingletonAwake()
    {
        // gameObject.AddComponent<SaveManagerService>();
        // SaveService = GetComponent<SaveManagerService>();
        StartCoroutine(Start());
    }

    public IEnumerator Start()
    {
        GameObject context = GameObject.Find("Context");
        if(context != null)
        {
            currentSnapshot = context.GetComponent<SaveSnapshot>();
            if(currentSnapshot.Snapshot.Title != "") {
                SaveContext Save = SaveService.GetSaveController(currentSnapshot.Snapshot.Title);
                yield return Save.Create().AsIEnumerator();
                SaveService.CaptureSnapshot(true);
            } else {
                SaveContainer viableSave = FindViableSave();
                if(viableSave.Name != "") 
                {
                    currentSnapshot.Snapshot = new SaveSnapshotData() { Title = viableSave.Name };
                }
            }
            SaveService.CaptureSnapshot(true);
        }
    }

    private SaveContainer FindViableSave()
    {
        SaveContainer[] savesList = SaveService.ListSaves();
        if(savesList.Length == 0)
        {
            Debug.LogWarning("[TimeWizard] A save snapshot couldn't be found by the game. An attempt to use the latest save failed as no saves were found. A new save snapshot will be created.");
            return new SaveContainer() { Name = System.Guid.NewGuid().ToString() };
        }
        var savesOrderedByUpdate = savesList.OrderByDescending(save => save.UpdatedAt).ToList();
        return savesOrderedByUpdate[0];
    }

    public void NextScene()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    public IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("TestLevel", LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TestLevel"));
        // AsyncOperation asyncLoad2 = SceneManager.UnloadSceneAsync("SampleScene");
        // while (!asyncLoad2.isDone)
        // {
        //     yield return null;
        // }
    }
}