using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbleDetection : MonoBehaviour
{
    public CurrentInteractable currentInteractable;

    #region  //Check if there are Crumbles//
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crumbles"))
        {
            currentInteractable.crumbleCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentInteractable.crumbleCollider != null)
        {
            currentInteractable.crumbleCollider = null;
        }
    }

    #endregion
}
