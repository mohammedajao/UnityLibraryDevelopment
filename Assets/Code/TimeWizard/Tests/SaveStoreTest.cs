using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;
using TimeWizard.Core;

public class SaveStoreTest : MonoBehaviour, ISaveStore
{
    public static readonly string Name = "SaveStoreTest";

    public SaveManager Manager => SaveManager.Instance;

    [Serializable]
    public class CountSaveState
    {
        [SerializeField] public int LastCount;
    }

    public int _count = 0;
    public bool Activate = false;
    
    public string GetIdentifier()
    {
        return Name;
    }

    public void LoadChunkData(string chunkName, ChunkDataSegment info)
    {
        var state = info.As<CountSaveState>();
        _count = state.LastCount;
    }

    public List<SaveState> FetchSaveStates()
    {
        return new List<SaveState>()
        {
            new SaveState()
            {
                ChunkName = "OverworldArea",
                Data = new CountSaveState() {
                    LastCount = _count
                }
            }
        };
    }

    void Awake() {
        GameContext.Current.Stores.Register(this);
    }

    void OnEnable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        // ChunkTest controller = GameObject.Find("SavesInitializer").GetComponent<ChunkTest>();
        // controller.AddStore(this); 
    }

    private void OnDestroy()
    {
        // GameContext.Current.Stores.Unregister(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Activate)
        {
            _count++;
            Activate = false;
        }
    }
}
