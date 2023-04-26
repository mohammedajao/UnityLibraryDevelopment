using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;

// Currently bugged
public class PositionalStoreTest : MonoBehaviour, ISaveStore
{
    public static readonly string Name = "PositionalStoreTest";

    [Serializable]
    public class LocationSaveState
    {
        [SerializeField] public Vector3 LastPosition;
    }

    private Vector3 _lastPosition;
    public bool Activate = false;

    public string GetIdentifier()
    {
        return Name;
    }

    public void LoadChunkData(string chunkName, ChunkDataSegment info)
    {
        var state = info.As<LocationSaveState>();
        if(state != null)
            _lastPosition = state.LastPosition;
    }

    public List<SaveState> FetchSaveStates()
    {
        return new List<SaveState>()
        {
            new SaveState()
            {
                ChunkName = "OverworldArea",
                Data = new LocationSaveState() {
                    LastPosition = _lastPosition
                }
            }
        };
    }

    void Awake() {
        _lastPosition = transform.position;
        ChunkTest controller = GameObject.Find("SavesInitializer").GetComponent<ChunkTest>();
        controller.AddStore(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // ChunkTest controller = GameObject.Find("SavesInitializer").GetComponent<ChunkTest>();
        // controller.AddStore(this); 
        transform.position = _lastPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _lastPosition = transform.position;
    }
}
