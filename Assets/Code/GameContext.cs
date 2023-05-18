using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;

public class GameContext : MonoBehaviour
{
    // private static GameContext currentContext;
    // public static GameContext Current;
    public static GameContext Current;
    public SaveStoreManager Stores;
    public SaveSnapshotData Snapshot;

    public GameContext()
    {
        GameContext.Current = this;
        Stores = new SaveStoreManager();
    }

    public void Awake()
    {
        GameContext.Current = this;
    }

    public void OnDestroy()
    {
        Stores.OnDestroy();
    }
}