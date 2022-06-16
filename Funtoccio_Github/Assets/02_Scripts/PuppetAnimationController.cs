using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuppetAnimationController : MonoBehaviour
{
    public bool testdancing;
    public bool testthrowing;
    public bool testeating;

    public int startAngle;

    [Header("__Please Implement manually")]
    public Transform HandControllerLeft;
    public Transform HandControllerRight;
    public Transform HeadController;
    public GameObject Funtoccio;
   private GameObject Ogre;

    public Transform Cross;

    [Header("__Dance Attributes")]
    public Vector2 danceMovements;
    public float danceSpeed;
    public float danceWaitTime;
    public Vector2 winkel;
    public float bounceY;
    public float turnY;
    [HideInInspector] public bool TestOutDancing;

    [Header("__Eat Attributes")]
    public GameObject eatingCrumbs;
    [Range(0, 10)] public int eatMovements;
    [Range(0, 5)] public float eatSpeed;
    [Range(0, 2)] public float eatWaitTime;
    [Range(0,45)]public int eatWinkelY;
    [Range(0, 10)] public float crumbsFallingHeight;

    [Header("__AppearOgre Attributes")]
    public Vector2 startPos;
    public Vector2 endPos;
    public Transform sugarHousePos;
    public float jumpHeight;
    public float appearSpeed;
    public float ogreAppearWaitTime;

    [Header("__DisappearOgre Attributes")]
    public float dancingTimeBeforeLeaving;

    [Header("__ThrowFuntoccio Attributes")]
    public bool isOgre;
    public float waitTimeBeforeOgreThrows;
    public Transform houseThrowPos;
    public float houseThrowTime;

    [Header("__Shake Attributes")]
    public Vector2 randomRangeWinkelBody;
    public Vector2 randomRangeWinkelHead;
    public float shakeSpeed;
    public int shakeLength;
    public float waitBetweenShakes;

    [Header("__SugarRush")]
    public Vector2 sugarRushBorderRight;
    public Vector2 sugarRushBorderLeft;

    [Header("___FuntoccioThrowingOgre Attributes")]
    public float waitBeforeGettingThrown;
    public float throwSpeed;

    [HideInInspector] public bool isFacingRight;
    public GreenLightSceneTransitions greenlightScript;


    private void Start()
    {
        //turn player to left
        Cross.forward = new Vector3(1, 0, 0);
        Cross.rotation = Quaternion.Euler(0, startAngle, 0);
        isFacingRight = true;

        Ogre = GameObject.Find("Ogre");
        Funtoccio = GameObject.Find("PUPPET_funtoccio");
    }

    private void Update()
    {
        if (testdancing)
        {
            testdancing = false;
            TriggerDance(); 
        }

        if (testeating)
        {
            testeating = false;
            StartCoroutine(Eat());
        }

        if (testthrowing)
        {
            testthrowing = false;
            Throw();
        }

    }
    IEnumerator ThrowOgre()
    {
        yield return new WaitForSeconds(waitBeforeGettingThrown);
        transform.DOMove(houseThrowPos.position, throwSpeed).SetEase(Ease.OutCubic);
        Debug.Log("move ogre do tween");
        yield return new WaitForSeconds(throwSpeed+ 1f);

        greenlightScript.ActivateFuneral();
    }

    IEnumerator AppearOgre()
    {
        transform.position = startPos;
      
        yield return new WaitForSeconds(ogreAppearWaitTime);
        transform.DOMove(new Vector3(endPos.x, endPos.y, transform.position.z), appearSpeed).SetEase(Ease.InOutQuart);
       // transform.position = new Vector3(endPos.x, transform.position.y, transform.position.z);
        Debug.Log("letOGreAppear!");
    }

    IEnumerator DisappearOgre(bool isDancing)
    {
        Debug.Log(gameObject + "is dancing!");
        if (isDancing)
        {
            TriggerDance();
            yield return new WaitForSeconds(dancingTimeBeforeLeaving);
        }

        transform.DOMove(new Vector3(startPos.x, endPos.y, transform.position.z), appearSpeed).SetEase(Ease.InCubic);
    }

    #region  //Trigger Functions//

    public void TriggerDance()
    {
        StartCoroutine(Dance());
    }

    public void TriggerAppearOgre()
    {
        StartCoroutine(AppearOgre());
    }

    public void TriggerDisappearOgre(bool isDance)
    {
        StartCoroutine(DisappearOgre(isDance));
    }

    public void TriggerEat()
    {
        StartCoroutine(Eat());
    }

    public void TriggerThrow()
    {
        Throw();
    }

    public void TriggerOgreThrow()
    {
        StartCoroutine(ThrowFuntoccio());
    }

    public void TriggerSugarRush()
    {
        randomRangeWinkelBody = new Vector2(-30,30);
        randomRangeWinkelHead = new Vector2(-10,10);

        StartCoroutine(SugarRush(25));
    }

    public void TriggerOgreGetsThrown()
    {
        StartCoroutine(ThrowOgre());
        Debug.Log("ogre gets thrown");
    }

    #endregion


    //____
    IEnumerator Dance()
    {
        //set funtoccio towards camera
        Cross.rotation = Quaternion.Euler(0, 180, 0);

        //initialize all needed values for dancing loop
        float multiplier = 1;
        int counter = 0;
        int danceMovementsNew = Random.Range((int)danceMovements.x, (int)danceMovements.y);
        float oldPosY = Cross.position.y;

        //while still in time, rotate player in different directions (immer abwechselnd durch multiplier)
        while (counter < danceMovementsNew)
        {
            multiplier = -multiplier;
            Vector3 movementRotation = new Vector3(Random.Range(winkel.x, winkel.y), 0, 0);
            float ZPos = Random.Range(10, 20);

            Cross.DORotate(new Vector3(movementRotation.x * multiplier, 180f - turnY * -multiplier, 0 + ZPos - ZPos * multiplier), danceSpeed).SetEase(Ease.InOutQuad);
            Cross.DOMove(new Vector3(transform.position.x, oldPosY - bounceY * multiplier, Cross.position.z), danceSpeed + 0.1f).SetEase(Ease.InCubic);
            //wait after each rotation for some extra time
            yield return new WaitForSeconds(danceSpeed + danceWaitTime);
            counter++;
        }

        //if finished, return to old position
        yield return new WaitForSeconds(danceWaitTime);
        Cross.transform.rotation = Quaternion.Euler(0, 180, 0);
        Cross.DOMove(new Vector3(transform.position.x, oldPosY, Cross.position.z), danceSpeed + 0.1f).SetEase(Ease.InCubic);

    }

    void Throw()
    {
        Transform currentTransform = HandControllerLeft;

        //setting base position - if turning left, throw with arm in back.
        if (!isFacingRight)
        {
            currentTransform = HandControllerRight;
        }

        Cross.DORotate(new Vector3(Cross.localRotation.eulerAngles.x - 30f, Cross.localRotation.eulerAngles.y, Cross.localRotation.z), 0.1f).SetEase(Ease.OutCirc);
        currentTransform.DORotate(new Vector3(Cross.localRotation.eulerAngles.x + 50, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z), 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            Cross.DORotate(new Vector3(Cross.localRotation.eulerAngles.x - 10, Cross.localRotation.eulerAngles.y, Cross.localRotation.z), 0.1f).SetEase(Ease.OutCirc);
            currentTransform.DORotate(new Vector3(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.z), 0.1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                currentTransform.rotation = Quaternion.Euler(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z);
                Cross.DORotate(new Vector3(0, Cross.localRotation.eulerAngles.y, Cross.localRotation.z), 0.1f).SetEase(Ease.OutCirc);
            });
        });

    }

    IEnumerator ThrowFuntoccio()
    {
        //turn ogre to funtoccio
        Cross.rotation = Quaternion.Euler(0, startAngle, 0);
        isFacingRight = false;

        //wait before funtoccio punched you
        StartCoroutine(Shake(20));
        yield return new WaitForSeconds(waitTimeBeforeOgreThrows);

        Transform currentTransform = HandControllerLeft;
        Cross.rotation = Quaternion.Euler(0, 225, 0);

        //throw him away
        if (!isFacingRight)
        {
            currentTransform = HandControllerRight;
        }

        Cross.DORotate(new Vector3(Cross.localRotation.eulerAngles.x - 30f, Cross.localRotation.eulerAngles.y, Cross.localRotation.z), 0.1f).SetEase(Ease.OutCirc);
        currentTransform.DORotate(new Vector3(Cross.localRotation.eulerAngles.x + 50, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z), 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            Cross.DORotate(new Vector3(Cross.localRotation.eulerAngles.x - 10, Cross.localRotation.eulerAngles.y, Cross.localRotation.z), 0.1f).SetEase(Ease.OutCirc);
            currentTransform.DORotate(new Vector3(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.z), 0.1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                currentTransform.rotation = Quaternion.Euler(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z);
                Cross.DORotate(new Vector3(0, Cross.localRotation.eulerAngles.y, Cross.localRotation.z), 0.1f).SetEase(Ease.OutCirc);
                Funtoccio.transform.DOMove(houseThrowPos.position,houseThrowTime).SetEase(Ease.OutCubic);
                greenlightScript.ActivateCandyShop(0.5f);
            });
        });

    }

    IEnumerator Shake(int shakeTime)
    {
        Cross.rotation = Quaternion.Euler(0, 180, 0);

        float multiplier = 1;
        float counter = 0;
        Vector3 oldRot = new Vector3(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z);

        while (counter < shakeTime)
        {
            multiplier = -multiplier;
            //turn a bit, head bop to back
            HeadController.DORotate(new Vector3(Cross.localRotation.eulerAngles.x - Random.Range(randomRangeWinkelHead.x, randomRangeWinkelHead.y),  Cross.localRotation.eulerAngles.y - Random.Range(randomRangeWinkelHead.x, randomRangeWinkelHead.y) , Cross.localRotation.z - 30 ), shakeSpeed).SetEase(Ease.OutCirc);
            Cross.DORotate(new Vector3(Random.Range(randomRangeWinkelBody.x, randomRangeWinkelBody.y) * multiplier, oldRot.y + Random.Range(randomRangeWinkelBody.x, randomRangeWinkelBody.y) * multiplier, Random.Range(randomRangeWinkelBody.x, randomRangeWinkelBody.y) * multiplier), shakeSpeed).SetEase(Ease.InOutQuad);

            //wait after each rotation for some extra time
            yield return new WaitForSeconds(shakeSpeed + waitBetweenShakes);
            counter ++;
        }
        yield return new WaitForSeconds(shakeSpeed + waitBetweenShakes);
        Cross.rotation = Quaternion.Euler(0, 180, 0);
        HeadController.rotation = Quaternion.Euler(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z);
    }

    IEnumerator SugarRush(int shakeTime)
    {
        Vector3 oldPos = Cross.transform.position;
        Cross.rotation = Quaternion.Euler(0, 180, 0);

        float multiplier = 1;
        float counter = 0;
        Vector3 oldRot = new Vector3(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z);

        while (counter < shakeTime)
        {
            multiplier = -multiplier;
            //turn a bit, head bop to back
            HeadController.DORotate(new Vector3(Cross.localRotation.eulerAngles.x - Random.Range(randomRangeWinkelHead.x, randomRangeWinkelHead.y), Cross.localRotation.eulerAngles.y - Random.Range(randomRangeWinkelHead.x, randomRangeWinkelHead.y), Cross.localRotation.z - 30), shakeSpeed).SetEase(Ease.OutCirc);
            Cross.DORotate(new Vector3(Random.Range(randomRangeWinkelBody.x, randomRangeWinkelBody.y) * multiplier, oldRot.y + Random.Range(randomRangeWinkelBody.x, randomRangeWinkelBody.y) * multiplier, Random.Range(randomRangeWinkelBody.x, randomRangeWinkelBody.y) * multiplier), shakeSpeed).SetEase(Ease.InOutQuad);
            Cross.DOMove(new Vector3(Random.Range(sugarRushBorderLeft.x, sugarRushBorderRight.x), Random.Range(sugarRushBorderLeft.y, sugarRushBorderRight.y), Cross.transform.position.z), shakeSpeed);
            //wait after each rotation for some extra time
            yield return new WaitForSeconds(shakeSpeed + waitBetweenShakes);
            counter++;
        }
        yield return new WaitForSeconds(shakeSpeed + waitBetweenShakes);
        Cross.rotation = Quaternion.Euler(0, 180, 0);
        HeadController.rotation = Quaternion.Euler(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z);
        Cross.transform.position = oldPos;
    }

    IEnumerator Eat()
    {
        float spiegelMultiplier = 1;
        if (!isFacingRight)
        {
            spiegelMultiplier = -1;
        }

        //initialize all needed values for dancing loop
        float multiplier = 1;
        int counter = 0;
        int currentEatMovements = eatMovements;
        float oldPosY = transform.position.y;

        Vector3 oldRot = new Vector3(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z);


        //while still in time, rotate player in different directions (immer abwechselnd durch multiplier)
        while (counter < currentEatMovements)
        {
            multiplier = -multiplier;
            //turn a bit, head bop to back
            HeadController.DORotate(new Vector3(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y - 10 * spiegelMultiplier, Cross.localRotation.z - 30 * spiegelMultiplier), 0.3f).SetEase(Ease.OutCirc);
            Cross.DORotate(new Vector3(0, oldRot.y + eatWinkelY * multiplier, 0), eatSpeed).SetEase(Ease.InOutQuad);

            //wait after each rotation for some extra time
            yield return new WaitForSeconds(eatSpeed + eatWaitTime / 2);
            //head bop to front
            HeadController.DORotate(new Vector3(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y + 10 * spiegelMultiplier, Cross.localRotation.z + 30 * spiegelMultiplier), 0.3f).SetEase(Ease.OutCirc);
            GameObject currentCrumbs = Instantiate(eatingCrumbs);
            AudioManager.current.Play("EatingSound");
            currentCrumbs.transform.position = new Vector3(HeadController.transform.position.x, HeadController.transform.position.y - crumbsFallingHeight, HeadController.transform.position.z - 0.2f);
            yield return new WaitForSeconds(eatSpeed + eatWaitTime / 2);
            counter++;
        }

        //bring back old pos 
        yield return new WaitForSeconds(eatWaitTime + eatSpeed);

        Cross.rotation = Quaternion.Euler(oldRot);
        HeadController.rotation = Quaternion.Euler(Cross.localRotation.eulerAngles.x, Cross.localRotation.eulerAngles.y, Cross.localRotation.eulerAngles.z);

    }

   

}
