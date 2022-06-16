using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(PlayerInput))]

public class SignSelectionController : MonoBehaviour
{
    #region  //Variables//

    [Header("Data Containers")]
    public CurrentInteractable currentInteractable;

    //cursor variables//
    private int cursorIndex = 0;
    public ActionColorContainer colorContainer;

    //curser controls//
    private PlayerInput playerInput;

    private InputAction upAction;
    private InputAction downAction;
    private InputAction enterAction;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrenght;
    [SerializeField] private int shakeVibrato;
    [SerializeField] private float shakeRandomness;

    #endregion

    void Start()
    {

        playerInput = GetComponent<PlayerInput>();

        upAction = playerInput.actions["ArrowUp"];
        downAction = playerInput.actions["ArrowDown"];
        enterAction = playerInput.actions["EnterButton"];

        SetSignStartColors();

    }

    void Update()
    {
        MoveCursor();
    }
    
    public void SetSignTexts() 
    {
        GameObject currentObject = currentInteractable.currentGameObject;
        ActionWheelController wheelController = currentObject.GetComponent<ActionWheelController>();

        //go through each element of the action list and asign it to the matching text of the child object
        for (int i = 0; i <= wheelController.actionList.Count - 1; i++)
        {
            GameObject currentChild = this.gameObject.transform.GetChild(i).gameObject;
            Action currentAction = wheelController.actionList[i];
            currentChild.GetComponentInChildren<TextMeshProUGUI>().text = currentAction.actionName;
        }

        

    }

    private void SetObjectColor(Color colorToSet)
    {
        gameObject.GetComponent<Renderer>().material.color = colorToSet;
    }

    private void MoveCursor()
    {
        if (upAction.triggered)
        {
            MoveCursorUp();
            Debug.Log("up ispressed");
        }
        else
        if (downAction.triggered)
        {
            MoveCursorDown();
            Debug.Log("down ispressed");
        }


        if (enterAction.triggered)
        {
            ExecuteActionEvent();
            Debug.Log("enter action");
        }
    }

    public void MoveCursorDown()
    {
        GameObject currentObject = currentInteractable.currentGameObject;
        ActionWheelController wheelController = currentObject.GetComponent<ActionWheelController>();

        if (cursorIndex <= 0)  //in case of first position
        {
            cursorIndex = (wheelController.actionList.Count - 1);
            SetCursorColor(0);
        }
        else
        {
            cursorIndex -= 1;
            SetCursorColor(cursorIndex + 1);
        }


    }

    public void MoveCursorUp()
    {

        ActionWheelController wheelController = GetWheelController();

        if (cursorIndex >= (wheelController.actionList.Count - 1))  //in case of last position
        {
            cursorIndex = 0;
            SetCursorColor(wheelController.actionList.Count - 1);
        }
        else
        {
            cursorIndex += 1;
            SetCursorColor(cursorIndex - 1);
        }
    }

    public void ExecuteActionEvent() 
    {
        ActionWheelController wheelController = GetWheelController();
        GameObject currentChild = this.gameObject.transform.GetChild(cursorIndex).gameObject;


        Action currentAction = wheelController.actionList[cursorIndex];
        currentAction.actionEvent.Invoke();
        Debug.Log(currentAction.actionName + " event was invoked");

        currentChild.transform.DOShakePosition( shakeDuration, shakeStrenght, 
                                                 shakeVibrato, shakeRandomness);
    }


    public void SetCursorColor(int lastIndex)
    {
        GameObject currentChild = this.gameObject.transform.GetChild(cursorIndex).gameObject;
        GameObject lastChild = this.gameObject.transform.GetChild(lastIndex).gameObject;

        currentChild.GetComponent<Renderer>().material.color = colorContainer.selectedColor;
        lastChild.GetComponent<Renderer>().material.color = colorContainer.neutralColor;
    }

    public void SetSignStartColors() 
    {
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[transform.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //now change their color
        foreach (GameObject child in allChildren)
        {
            child.GetComponent<Renderer>().material.color = colorContainer.neutralColor;
        }

    }

    public ActionWheelController GetWheelController() 
    {
        GameObject currentObject = currentInteractable.currentGameObject;
        ActionWheelController wheelController = currentObject.GetComponent<ActionWheelController>();

        return wheelController;
    }

}



