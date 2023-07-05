using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gummy.Shared
{
    public class RuntimeWorker : MonoBehaviour
    {
        public RuntimeWorkerSet WorkerSet;

        private void OnDisable() {
            WorkerSet.RemoveWorker();
        }
    }
}