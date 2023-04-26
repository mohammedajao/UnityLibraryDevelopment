using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeWizard
{
    public interface ISaveInterpreter
    {
        void ProcessChunks(Chunk[] chunks);
        Chunk[] ApplyModifications(Chunk[] chunks);
        bool IsDirty();
    }
}