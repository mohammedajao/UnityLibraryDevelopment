using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeWizard;

namespace TimeWizard.Persistence
{
    public interface ISaveLoader
    {
        bool TryLoad(string name, out Chunk[] chunks, out Exception ex);
        bool TrySave(string name, Chunk[] chunks, out Exception ex);
        SaveContainer[] ListSaves();
    }
}