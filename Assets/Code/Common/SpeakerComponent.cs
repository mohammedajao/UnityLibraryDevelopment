using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;
using Gummy.References;
using Gummy.Shared;
using Gummy.Tools;
using TMPro;

public class SpeakerComponent : MonoBehaviour
{
    [GummyEntryFilter(EntryType =  GummyEntryFilterType.Fact, TableName = "Speakers")]
    public GummyEntryReference speaker;
    public SpeakerSet Speakers;

    public GummyEvent binder;
    public GummyEvent unbinder;
    public GameBlackboardProvider provider;
    public GummyEventBus eventBus;

    private TextMeshPro chatText;
    private SpriteRenderer background;
    private Transform chatObject;

    private int _currentVisibleCharacterIndex;

    private WaitForSeconds _simpleDelay = new WaitForSeconds(0.05f);

    private IEnumerator TypewriterEffect()
    {
        TMP_TextInfo textInfo = chatText.textInfo;
        while(chatText.maxVisibleCharacters < textInfo.characterCount + 1) {
            chatText.maxVisibleCharacters++;
            yield return _simpleDelay;
        }
    }

    public IEnumerator Execute(DialogueEntry entry)
    {
        if(chatText == null || background == null) yield break;
        chatObject.gameObject.SetActive(true);
        chatText.SetText(entry.text);
        chatText.ForceMeshUpdate();
        Vector2 textSize = chatText.GetRenderedValues(false);
        background.size = new Vector2(background.size.x, Math.Max(3.81f, chatText.textBounds.extents.y + 2f));
        chatText.maxVisibleCharacters = 0;
        yield return TypewriterEffect();
        yield return new WaitForSeconds(0.5f);
        chatObject.gameObject.SetActive(false);
        yield return new WaitForSeconds(entry.Delay);
        Debug.Log($"[Dialogue: {entry.id}] - {entry.text}");
    }

     void Awake() {
        if(speaker == 0) return;
        binder.context = provider.BlackboardIdentifier;
        unbinder.context = provider.BlackboardIdentifier;
        Speakers.RegisterSpeaker(speaker, this);
        binder.Invoke(eventBus);
    }

    void Start()
    {
        chatObject = transform.Find("ChatBubble");
        if(chatObject == null) return;
        background = chatObject.Find("Background").GetComponent<SpriteRenderer>();
        chatText = chatObject.Find("ChatText").GetComponent<TextMeshPro>();
    }

    void OnDestroy()
    {
        if(speaker == 0) return;
        Speakers.UnregisterSpeaker(speaker);
        unbinder.Invoke(eventBus);
    }

}
