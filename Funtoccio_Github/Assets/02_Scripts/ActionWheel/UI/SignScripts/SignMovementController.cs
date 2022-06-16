using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(PlayerInput))]
public class SignMovementController : MonoBehaviour
{
    #region  //Variables//

    public Transform startPosition;

    public CurrentInteractable currentInteractable;

    #endregion

    void Start()
    {
        SetStartPosition();

    }

  

    private void SetStartPosition() 
    {
        transform.position = startPosition.position;
    }

    public void MoveSignUp(float duration) 
    {
        transform.position = currentInteractable.positionA.position;
        transform.DOMove(currentInteractable.positionB.position, duration);
    }

    public void MoveSignDown(float duration)
    {
        if (transform.position == currentInteractable.positionB.position) 
        {
            transform.DOMove(currentInteractable.positionA.position, duration);
        }
    }

}
