using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;
using TimeWizard.Core;

public class GuidObjectTest : MonoBehaviour, ISaveStore
{
    public string Name = "";

    public SaveManager Manager => SaveManager.Instance;

    [Serializable]
    public class TitleSaveState
    {
        [SerializeField] public string LastTitle;
    }

    public string Title = "";
    
    public string GetIdentifier()
    {
        return Name;
    }

    public void LoadChunkData(string chunkName, ChunkDataSegment info)
    {
        var state = info.As<TitleSaveState>();
        Title = state.LastTitle;
    }

    public List<SaveState> FetchSaveStates()
    {
        return new List<SaveState>()
        {
            new SaveState()
            {
                ChunkName = "OverworldArea",
                Data = new TitleSaveState() {
                    LastTitle = Title
                }
            }
        };
    }

    void Awake() {
        byte[] gb = Guid.NewGuid().ToByteArray();
        long l = BitConverter.ToInt64(gb, 0);
        Debug.Log($"The id in long is {l}");
        Manager.Register(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // ChunkTest controller = GameObject.Find("SavesInitializer").GetComponent<ChunkTest>();
        // controller.AddStore(this); 
    }

    public void OnDestroy()
    {
        Manager.Unregister(this);
    }
}
