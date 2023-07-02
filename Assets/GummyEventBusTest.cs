using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.References;
using Gummy.Shared;

public class GummyEventBusTest : MonoBehaviour
{
    public GummyEventBus bus;
    public GameBlackboardProvider provider;
    // Start is called before the first frame update

    public GummyEntryReference test;
    void Start()
    {
        Debug.Log("Trying");
        Debug.Log(App.Instance);
        if (bus == null || provider == null) return;
        Debug.Log("Invoking event");
        bus.Invoke(test, provider.BlackboardIdentifier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
