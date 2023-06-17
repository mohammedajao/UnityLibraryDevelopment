using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Shared;

public class GummyEventBusTest : MonoBehaviour
{
    public GummyEventBus bus;
    public GameBlackboardProvider provider;
    // Start is called before the first frame update
    void Start()
    {
        if (bus == null || provider == null) return;
        Debug.Log("Invoking event");
        bus.Invoke(-50106142, provider.BlackboardIdentities);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}