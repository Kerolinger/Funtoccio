using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCrumbles : MonoBehaviour
{
    #region  //Variables//

    [Header("Current Interactable")]
    public CurrentInteractable currentInteractable;

    [Space]
    [Header("GameObjects")]
    public GameObject crumbleObject;
    private GameObject crumbleInstance;

    private GameObject crumblePositionsParent;


    [Header("Maximum Crumble Number")]
    [SerializeField] private int maxNumber = 2;

    #endregion


    private void Start()
    {
        Transform child = transform.Find("CrumblePositions");
        if (child != null)
        {
            crumblePositionsParent = child.gameObject;
        }
    }

    public void InstantiateCrumbleObject(int positonIndex) 
    {
        int currentChildCount = gameObject.transform.childCount;
        if (currentChildCount <= (maxNumber + 1)) 
        {
            crumbleInstance = Instantiate(crumbleObject, transform);
            GameObject currentChild = this.gameObject.transform.GetChild(currentChildCount).gameObject; //get latest child
            currentChild.transform.position = crumblePositionsParent.transform.GetChild(positonIndex).position; //set position of latest child to position of the childindex of choice
        }
    }

    public void DestroyThisCrumble()
    {
        Destroy(currentInteractable.currentGameObject);
    }
}
