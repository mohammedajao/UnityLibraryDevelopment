using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gummy.Shared
{
    [CreateAssetMenu(fileName = "RuntimeWorkerSet", menuName = "Gummy/RuntimeWorker")]
    public class RuntimeWorkerSet : ScriptableObject
    {
        private GameObject Worker;

        public void BindWorker(GameObject worker)
        {
            this.Worker = worker;
            this.Worker.AddComponent<RuntimeWorker>();
            RuntimeWorker component = this.Worker.GetComponent<RuntimeWorker>();
            component.WorkerSet = this;
        }

        // #nullable
        public RuntimeWorker GetWorker()
        {
            if(Worker != null) return this.Worker.GetComponent<RuntimeWorker>();
            return null;
        }

        public void RemoveWorker()
        {
            Worker = null;
        }
    }
}