using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class AnotherWheelControllerUI : MonoBehaviour
{
    public GameObject actionUIObject;

    private GameObject parentProp;
    private GameObject grandpaProp;
    private ActionWheelController wheelController;

    private GameObject UIInstance;
    [SerializeField] private float slotDistance = -5f;

    //cursor variables//
    public int cursorIndex = 0;
    public ActionColorContainer colorContainer;

    //curser controls//
    private PlayerInput playerInput;

    private InputAction upAction;
    private InputAction downAction;
    private InputAction enterAction;

    void Start()
    {
        parentProp = gameObject.transform.parent.gameObject;
        GameObject grandpaProp = parentProp.transform.parent.gameObject;
        if (grandpaProp != null)
        {
            wheelController = grandpaProp.GetComponent<ActionWheelController>();
        }

        SetActionPositions();

        playerInput = GetComponent<PlayerInput>();
        
            upAction = playerInput.actions["ArrowUp"];
            downAction = playerInput.actions["ArrowDown"];
            enterAction = playerInput.actions["EnterButton"];
        
    }

    private void Update()
    {
      MoveCurser();
    }



    public void SetActionPositions()
    {
        for (int i = 0; i <= wheelController.actionList.Count - 1; i++)
        {
            UIInstance = Instantiate(actionUIObject, transform);

            GameObject currentChild = this.gameObject.transform.GetChild(i).gameObject;
            
            currentChild.transform.position = new Vector3(currentChild.transform.position.x, currentChild.transform.position.y + Screen.height / 100 * -(slotDistance * i) / 100, currentChild.transform.position.z);
            currentChild.GetComponent<Image>().color = colorContainer.neutralColor;

            Action currentAction = wheelController.actionList[i];
            currentChild.GetComponentInChildren<TextMeshProUGUI>().text = currentAction.actionName;
        }

    }

    public void MoveCurser()
    {
        if (upAction.triggered)
        {
            MoveCursorUp();
        }
        else
        if (downAction.triggered)
        {
            MoveCursorDown();
        }


        if (enterAction.triggered)
        {
            ExecuteActionEvent();
        }
    }

    public void MoveCursorDown()
    {
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
        if (cursorIndex >= (wheelController.actionList.Count - 1))  //in case of last position
        {
            cursorIndex = 0;
            SetCursorColor(wheelController.actionList.Count - 1);
        }
        else
        {
            cursorIndex += 1;
            SetCursorColor(cursorIndex -1);
        }
    }

    public void ExecuteActionEvent() 
    {
        Action currentAction = wheelController.actionList[cursorIndex];
        currentAction.actionEvent.Invoke();
        Debug.Log(currentAction.actionName + " event was invoked");

    }

    public void SetCursorColor(int lastIndex) 
    {
        GameObject currentChild = this.gameObject.transform.GetChild(cursorIndex).gameObject;
        GameObject lastChild = this.gameObject.transform.GetChild(lastIndex).gameObject;

        currentChild.GetComponent<Image>().color = colorContainer.selectedColor;
        lastChild.GetComponent<Image>().color = colorContainer.neutralColor;

    }
}

