using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// App -> Singleton Services + PubSubDownstream
public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shuttingDown = false;
    private static object _lock = new object();
    private static T _instance;
    private bool started = false;
    
    public static T Instance { 
        get {
            if(shuttingDown)
            {
                return null;
            }
            if(_instance == null)
            {
                lock(_lock) 
                {
                    _instance = (T)FindObjectOfType(typeof(T), includeInactive: true);
                    if(_instance == null)
                    {
                        var singletonObj = new GameObject();
                        _instance = singletonObj.AddComponent<T>();
                        singletonObj.name = $"(Singleton)  {typeof(T).ToString()}";
                        // DontDestroyOnLoad(singletonObj); -- App prefab should handle this
                    }
                }
            }
            return _instance;
        }
    }

    private void Awake() {
        CheckInstance(SingletonAwake);
    }

    void OnEnable()
    {
        CheckInstance(SingletonOnEnable);
    }

    private void Start()
    {
        started = true;
        CheckInstance(SingletonStart);
    }

    private void OnDestroy()
    {
        shuttingDown = started && true;
    }

    protected virtual void SingletonStart() {}
    protected virtual void SingletonOnEnable() {}
    protected virtual void SingletonAwake() {}
    

    private void CheckInstance(Action callback)
    {
        if(_instance && _instance != this)
        {
            Destroy(this);
            return;
        }
        callback();
    }

    private void OnApplicationQuit() {
        shuttingDown = true;
    }
}
