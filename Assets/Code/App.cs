using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TimeWizard.Core;
using TimeWizard.Persistence;
using TimeWizard;

public interface AppComponent
{
    void OnLoad();
}

public class App : UnitySingleton<App>
{
    public SaveManager SaveService => SaveManager.Instance;
    public SaveSnapshotData currentSnapshot;
    private string _activeScene;
    public SpeakerSet ActiveSpeakers;

    public bool IsEditor = false;

    public static List<AppComponent> Components = new();

    // Test stuff
    public bool Trigger = false;
    void Update()
    {
        if(Trigger == true)
        {
            Trigger = false;
            NextScene();
        }
    }

    // End TestStuff

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
        OnLoad();
        Object.DontDestroyOnLoad(app);
    }

    public static void OnLoad()
    {
        foreach(var component in Components)
        {
            component.OnLoad();
        }
    }

    protected override void SingletonAwake()
    {
        // gameObject.AddComponent<SaveManagerService>();
        // SaveService = GetComponent<SaveManagerService>();
        // SceneManager.activeSceneChanged += SyncSaveContextOnSceneChange;
    }

    protected override void SingletonOnEnable()
    {
        currentSnapshot = GameContext.Current.Snapshot;
        if(!string.IsNullOrEmpty(currentSnapshot.Title)) {
            StartCoroutine(Start());
        };
    }

    public IEnumerator Start()
    {
        SaveContext Save = SaveService.GetSaveController(currentSnapshot.Title);
        yield return Save.Create().AsIEnumerator();
        _activeScene = Save.Location.ScenePath;
        if(SceneManager.GetActiveScene().path != _activeScene) {
            yield return SceneManager.LoadSceneAsync(_activeScene, LoadSceneMode.Additive);
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_activeScene));
        GameContext.Current.Stores.OnLoad(Save);
    }

    private void SyncSaveContextOnSceneChange(Scene curr, Scene next)
    {
        Debug.Log($"Prev Scene {curr.name} -- Next Scene {next.name}");
        SaveService.CurrentSaveContext.UpdateSceneLocation(next.path);
    }

    public void NextScene()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    public IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad2 = SceneManager.UnloadSceneAsync("SampleScene");
        while (!asyncLoad2.isDone)
        {
            yield return null;
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("TestLevel", LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("TestLevel"));
    }
}