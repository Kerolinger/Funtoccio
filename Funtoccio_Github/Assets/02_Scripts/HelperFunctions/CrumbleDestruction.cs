using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbleDestruction : MonoBehaviour
{
    public CurrentInteractable currentInteractable;
    public void OnCrumbleEat()
    {
        if (currentInteractable.currentGameObject == this.gameObject) 
        {
            Destroy(this.gameObject);
        }
    }
}
