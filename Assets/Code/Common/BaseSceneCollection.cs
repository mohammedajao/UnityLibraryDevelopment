using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gummy.Entries;
using Gummy.References;
using Gummy.Shared;

[CreateAssetMenu(fileName = "BaseSceneCollection", menuName = "Gummy/Collections/BaseSceneCollection", order = 0)]
public class BaseSceneCollection : GummyCollection
{
    public GummyDatabase database;

    private GummyEntryReference enterEvent;

    public override void Setup(GummyDatabase db)
    {
        var onEnter = db.CreateEntry(this, typeof(GummyEventEntry));
        onEnter.key = "on_enter";

        enterEvent = onEnter.id;
        database = db;
    }
}
