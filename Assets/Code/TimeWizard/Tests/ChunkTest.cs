using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TimeWizard.Core;
using TimeWizard;
using TimeWizard.Persistence;

public class ChunkTest : MonoBehaviour
{
    public SaveManager Manager => SaveManager.Instance;

    public bool TriggerSave = false;
    public bool TriggerCapture = false;

    public void AddStore(ISaveStore store)
    {
        Manager.Register(store);
    }

    void Awake()
    {
        // SaveContext Save = Manager.GetSaveController("Save1Test");
        // Task.WaitAll(Save.Create());
        // Manager.CaptureSnapshot();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TriggerSave)
        {
            TriggerSave = false;
            Manager.CaptureSnapshot();
            Manager.ApplySnapshot("Save1Test");
        }

        if(TriggerCapture)
        {
            TriggerCapture = false;
            Manager.CaptureSnapshot();
        }
    }
}
