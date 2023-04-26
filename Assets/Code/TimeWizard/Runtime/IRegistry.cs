using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeWizard
{
    public delegate void ItemRegistered<T>(T item);

    public interface IRegistry<T>
    {
        event ItemRegistered<T> OnItemRegistered;
        T[] List();
        void Register(T instance);
        void Unregister(T instance);
    }
}
