using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingDatatoBoltSO : MonoBehaviour
{
    public CurrentInteractable currentInteractable;


   public string objectName 
    {
        get { return currentInteractable.currentObjectName; }
    }

    public string GetInteractableName() 
    {
        return objectName;
    }

    public void Blabla() 
    {
        Debug.Log("Blabla");
    }
}
