using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameEventBase<T> : ScriptableObject
{
    HashSet<FloatEventListener> _listeners = new HashSet<FloatEventListener>();

    public void Invoke()
    {
        foreach (var globalEventListener in _listeners)
            globalEventListener.RaiseEvent();
    }

    public void Register(FloatEventListener gameEventListener) => _listeners.Add(gameEventListener);
    public void Deregister(FloatEventListener gameEventListener) => _listeners.Remove(gameEventListener);

}

