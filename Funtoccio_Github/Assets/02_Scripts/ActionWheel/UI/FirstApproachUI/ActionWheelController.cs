using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ActionWheelController : MonoBehaviour
{
    [SerializeField] private bool actionsActivated = false;
    private BoxCollider boxCollider;

    public List<Action> actionList;

    private GameObject wheelObject;
    private GameObject signPostionParent;

    public CurrentInteractable currentInteractable;

    public GameEvent InContact;
    public GameEvent OutOfContact;

    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        Transform child = transform.Find("ActionWheel");
        if (child != null)
        {
            wheelObject = child.gameObject;
        }

        Transform signchild = transform.Find("SignPositions");
        if (signchild != null)
        {
            signPostionParent = signchild.gameObject;
        }

    }

    private void Update()
    {
        SwitchingUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actionsActivated = true;

            if (currentInteractable != null)
            {
                currentInteractable.currentObjectName = this.gameObject.name;
                currentInteractable.currentGameObject = this.gameObject;
                currentInteractable.isInContact = true;

                if (signPostionParent != null) 
                {
                    currentInteractable.positionA = signPostionParent.transform.GetChild(0);
                    currentInteractable.positionB = signPostionParent.transform.GetChild(1);
                }

                if (InContact != null) 
                {
                    InContact.Invoke();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            actionsActivated = false;

            if (currentInteractable != null)
            {
                currentInteractable.currentObjectName = null;
                currentInteractable.isInContact = true;
                if (OutOfContact != null) 
                {
                    OutOfContact.Invoke();
                }
            }
        }
    }

    private void SwitchingUI()
    {
        if (wheelObject != null) 
        {
            if (actionsActivated == true)
            {
                wheelObject.SetActive(true);
            }
            else
            {
                wheelObject.SetActive(false);
            }
        }
    }

    

    private void OnApplicationQuit()
    {
        if (currentInteractable != null)
        {
            currentInteractable.currentObjectName = null;
            currentInteractable.currentObjectName = null;
            currentInteractable.isInContact = false;

            currentInteractable.positionA = null;
            currentInteractable.positionB = null;
        }
    }
}
