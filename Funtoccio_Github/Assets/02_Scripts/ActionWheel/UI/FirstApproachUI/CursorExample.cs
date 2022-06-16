using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorExample : MonoBehaviour
{
    public int positionIndex = 0;

    [SerializeField] private Vector3 positionZero;
    [SerializeField] private Vector3 positionOne;
    [SerializeField] private Vector3 positionTwo;

    private Vector3[] positionList = new Vector3[3];
    

    private void Start()
    {
        positionList[0] = positionZero;
        positionList[1] = positionOne;
        positionList[2] = positionTwo;
    }

    public void MoveCursorLeft()
    {
        if (positionIndex <= 0)  //in case of first position
        {
            positionIndex = (positionList.Length - 1);
            transform.position = positionList[positionIndex];
        }
        else
        {
            positionIndex -= 1;
            transform.position = positionList[positionIndex];
        }

    }

    public void MoveCursorRight()
    {
        if (positionIndex >= (positionList.Length - 1))  //in case of last position
        {
            positionIndex = 0;
            transform.position = positionList[positionIndex];
        }
        else
        {
            positionIndex += 1;
            transform.position = positionList[positionIndex];
        }
    }
}
