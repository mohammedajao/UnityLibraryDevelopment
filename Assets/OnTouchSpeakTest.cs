using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.References;
using Gummy.Shared;
using Gummy.Tools;

public class OnTouchSpeakTest : MonoBehaviour
{
    public GummyEvent theEvent = new();
    public GameBlackboardProvider provider;
    public GummyEventBus eventBus;
    // Start is called before the first frame update
    void Start()
    {
        theEvent.context = provider.GetBlackboard(theEvent.eventReference.id, provider.BlackboardIdentifier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Player") {
           theEvent.Invoke(eventBus);
        }
    }
}
