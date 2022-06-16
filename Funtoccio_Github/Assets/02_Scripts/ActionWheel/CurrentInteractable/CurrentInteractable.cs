using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuppetPlay/CurrentInterctable", fileName = "CurrentInteractable")]
public class CurrentInteractable : ScriptableObject
{
    public string currentObjectName;

    public GameObject currentGameObject;

    public bool isInContact = false;

    public Transform positionA;
    public Transform positionB;

    public Collider crumbleCollider;
}
