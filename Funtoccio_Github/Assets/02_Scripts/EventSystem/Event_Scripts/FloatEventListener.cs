using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatEventListener : MonoBehaviour
{
    [SerializeField] GameEventFloat _gameEventFloat;
    [SerializeField] UnityEvent _unityEvent;

    private void Awake() => _gameEventFloat.Register(this);
    private void OnDestroy() => _gameEventFloat.Deregister(this);
    public void RaiseEvent() => _unityEvent.Invoke();

}