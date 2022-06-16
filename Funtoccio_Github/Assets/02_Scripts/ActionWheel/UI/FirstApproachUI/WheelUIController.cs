using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelUIController : MonoBehaviour
{
    public GameObject actionUIObject;

    private GameObject parentProp;
    private GameObject grandpaProp;
    private ActionWheelController wheelController;

    private GameObject UIInstance;

    [SerializeField]private float slotDistance = -5f;

    void Start()
    {
        parentProp = gameObject.transform.parent.gameObject;
        GameObject grandpaProp = parentProp.transform.parent.gameObject;
        if (grandpaProp != null) 
        {
            wheelController = grandpaProp.GetComponent<ActionWheelController>();
        }

        SetActionPositions();
    }

    private void Awake()
    {
        //SetActionPositions();
    }

    private void OnEnable()
    {
        //SetActionPositions();
    }

    public void SetActionPositions() 
    {
        for (int i = 0; i <= wheelController.actionList.Count - 1; i++) 
        {
            UIInstance = Instantiate(actionUIObject, transform);

            GameObject currentChild = this.gameObject.transform.GetChild(i).gameObject;
            currentChild.transform.position = new Vector3(currentChild.transform.position.x, currentChild.transform.position.y + Screen.height / 100 * -(slotDistance * i) / 100, currentChild.transform.position.z);

            Action currentAction = wheelController.actionList[i];
            currentChild.GetComponentInChildren<TextMeshProUGUI>().text = currentAction.actionName;
        }

    }
}
