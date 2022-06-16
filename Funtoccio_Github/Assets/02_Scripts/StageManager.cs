using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public Animator curtainAnimator;
    public float camPanDuration;
    public bool isCurtainOpen;
    public int startingCamPos;
    public Transform camGroup;

    public GameObject PauseMenu;
       
    public CamPosition[] camPositions;

    public void Start()
    {
        SetCameraPos(startingCamPos);
    }

    public void SetCameraPos(int CamPos)
    {
        camGroup.DOMove(camPositions[CamPos].camPosition, camPositions[CamPos].panDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            //close or open curtain
            if (camPositions[CamPos].camPosName != "level")
            {
                isCurtainOpen = false;
            }

            else
            {
                isCurtainOpen = true;
            }

            OpenCurtain(isCurtainOpen);
            PauseMenu.SetActive(false);

        });

      

    }

    public void OpenCurtain(bool willOpen)
    {
        curtainAnimator.SetBool("isOpen", willOpen);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit game!");
    }


    public void OnPauseMenu()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
        OpenCurtain(!PauseMenu.activeSelf);
    }


}
