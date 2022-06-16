using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTreeManager : MonoBehaviour
{
    public CurrentInteractable currentInteractable;

    // für momo: AudioManager.current.Play(""); 
    #region  //Variables//

    [Header("Animators")]
    [Space]
     public Animator CandyCane;
    public Animator House;
    public Animator Bell;
    public PuppetAnimationController Ogre;
    public PlayerControllerCaro Funtoccio_CharacterController;
    public PuppetAnimationController Funtoccio;
    public Animator Camera;
    public GreenLightSceneTransitions scenetransition;

    [Space]

    [Header("Story States")]
    
    [SerializeField] private bool ogreOnStage = false;

    [SerializeField] private bool isInSugarRush = false;
    [SerializeField] private bool houseIscracked = false;

    [SerializeField] private bool musicIsPlaying = false;

    [SerializeField] private bool bellIsEaten = false;
    [Space]

    public int munchesUntilSugarRush;
    private int currentMunches = 0;

    [Header("Game Events")]
    [Space]

    public GameEvent MashingHouse;
    public GameEvent HouseDances;
    public GameEvent DoorOpens;

    public GameEvent OgreKick;
    public GameEvent OgreLeaves;
    public GameEvent OgreAppears;

    public GameEvent BellFallsDown;
    public GameEvent BellWasEaten;

    public GameEvent EatHouse;
    public GameEvent EatLever;
    public GameEvent CrumblesDisappear;

    private GameObject ogreObject;


    #endregion

    public void OnEatActionExecuted() 
    {
        if (currentInteractable.crumbleCollider == null) 
        {
            switch (currentInteractable.currentObjectName)
            {
                case "House":
                    EatingHouse();
                    break;

                case "Lever":
                    EatingLever();
                    break;

                case "Ogre":
                    EatingOgre();
                    break;

                case "Bell":
                    EatingBell();
                    break;

                default:
                    Debug.Log("incorrect input");
                    break;
            }
        }
        else if (currentInteractable.crumbleCollider != null) 
        {
            EatingCrumbles();
        }
    }

    private void EatingHouse() 
    {
        if (isInSugarRush == false)
        {
            EatHouse.Invoke();
            currentMunches++;
            if(currentMunches >= munchesUntilSugarRush)
            {
                isInSugarRush = true;
         
            }

            Debug.Log("You start eating, crumbles fall down. You get a little shaky");
            House.SetTrigger("eat");
        }
        else
            if (isInSugarRush == true)
        {
            MashingHouse.Invoke();
            Funtoccio.TriggerSugarRush();
            House.SetTrigger("destroy");
            Funtoccio_CharacterController.ChangeTagToPlayer(false);
            Debug.Log("While in sugar rush you escalate and mash the house");

            }
    }
    private void EatingLever() 
    {
        if (isInSugarRush == false)
        {
            currentMunches++;
            if (currentMunches >= munchesUntilSugarRush)
            {
                isInSugarRush = true;
            }

            Debug.Log("You eat from the bell, crumbles appear, bloodsugar level rises");
            EatLever.Invoke();
            CandyCane.SetTrigger("eat");
        }
        else
            if (isInSugarRush == true)
        {
            MashingHouse.Invoke();
            Funtoccio.TriggerSugarRush();
            House.SetTrigger("destroy");
            Funtoccio_CharacterController.ChangeTagToPlayer(false);

            Debug.Log("While in sugar rush you escalate and mash the house");
        };
        
    }
    private void EatingOgre() 
    {
        OgreKick.Invoke();
        Ogre.TriggerOgreThrow();
        Debug.Log("You bite into the ogres foot, ogre gets angry and kicks you out of the screen.");

    }
    private void EatingBell() 
    {
        if (!bellIsEaten) 
        {
            bellIsEaten = true;
            Debug.Log("You ate the bell. it's inside your belly now.");
            Bell.SetTrigger("eat");
            BellWasEaten.Invoke();
            AudioManager.current.Play("BellSoundQuiet");
        }
        
    }
    private void EatingCrumbles() 
    {
        
        CrumblesDisappear.Invoke();
        Debug.Log("You ate all of the crumbles, the floor is clean again");
    }

    public void OnDanceActionExecuted() 
    {
        if (currentInteractable != null) 
        {

            if (currentInteractable.crumbleCollider == null) 
            {
                switch (currentInteractable.currentObjectName)
                {
                    case "House":
                        DancingHouse();
                        break;

                    case "Lever":
                        DancingLever();
                        break;

                    case "Ogre":
                        DancingOgre();
                        break;

                    case "Bell":
                        Debug.Log("Dancing with the bell.");
                        break;

                    default:
                        Debug.Log("incorrect input");
                        break;
                }
            }
            else if (currentInteractable.crumbleCollider != null) 
            {
                Debug.Log("Dancing with crumbles.");
            }

        }
    }

    private void DancingHouse() 
    {
        if (!bellIsEaten)
        {
            if (!houseIscracked)
            {
                Debug.Log("House starts to dance and plays a nice version of caramel dansen");
                musicIsPlaying = true;
                HouseDances.Invoke();
                AudioManager.current.Play("CaramelDansen");
                AudioManager.current.Stop("MainTitleTrack");
            }
            else if(houseIscracked)
            {
                Debug.Log("House starts to dance and plays a shabby version of caramel dansen");
                musicIsPlaying = true;
                HouseDances.Invoke();
                AudioManager.current.Play("WonkyCaramelDansen");
                AudioManager.current.Stop("MainTitleTrack");
            }

            House.SetTrigger("dance");
        }

        if (bellIsEaten)
        {
            Debug.Log("you jingle as the bell is inside your belly, the door opens");
            AudioManager.current.Play("BellSound");
            DoorOpens.Invoke();
            scenetransition.ActivateCandyShop(2f);
            House.SetTrigger("doorOpen");

        }
    }

    private void DancingLever() 
    {
        if (musicIsPlaying)
        {
            CandyCane.SetBool("isMusicPlaying",true);
        }
        else
        {
            CandyCane.SetTrigger("dance");
        }
    }

    private void DancingOgre() 
    {
        Debug.Log("You start a while dance session with the Ogre. Ogre breaks Lever, bell falls");
        BellFallsDown.Invoke();
        AudioManager.current.Play("FallingDown");
        Ogre.TriggerDisappearOgre(true);
        Camera.SetTrigger("ogre_dance");
        Bell.SetTrigger("ogre_dance");
    }

    public void OnThrowActionExecuted() 
    {
        if (currentInteractable != null)
        {
            if (currentInteractable.crumbleCollider == null) 
            {
                switch (currentInteractable.currentObjectName)
                {
                    case "House":
                        ThrowingHouse();
                        break;

                    case "Lever":
                        ThrowingLever();
                        break;

                    case "Ogre":
                        ThrowingOgre();
                        break;

                    case "Bell":
                        ThrowingBell();
                        break;

                    default:
                        Debug.Log("incorrect input");
                        break;
                }
            }
            else if (currentInteractable.crumbleCollider != null)
            {
                ThrowingCrumbles();
            }
               
        }
    }

    private void ThrowingHouse() 
    {
        houseIscracked = true;
        House.SetTrigger("throw");
    }
    private void ThrowingLever()
    {
        CandyCane.SetTrigger("throw");
        Bell.SetTrigger("throw");
    }
    private void ThrowingOgre() 
    {
        MashingHouse.Invoke();
        Debug.Log("Youre throw the ogre on the house. The house gets smashed.");
        Ogre.TriggerOgreGetsThrown();

        Debug.Log("Youre throw the ogre on the house. The house gets smashed.");
    }
    private void ThrowingBell() 
    {
        Debug.Log("Bell goes up and falls down again");
    }
    private void ThrowingCrumbles() 
    {
        GameObject ogreParent = GameObject.Find("_Interactable");
        bool ogreIsActive = FindThisChild(ogreParent, "Ogre");
       //ogreObject = GetChildWithName(gameObject, "Ogre");//GameObject.Find("Ogre");

        Debug.Log("is the ogre active? answer: " + ogreIsActive);


        if (!ogreIsActive)
        {
            OgreAppears.Invoke();
            AudioManager.current.Play("OgreSound");
            Camera.SetTrigger("ogre_appear");
            Ogre.TriggerAppearOgre();
            Debug.Log("you throw the crumbles out of stage. after a small earthquake the ogre appears.");

        }
        else if (ogreIsActive)
        {
            OgreLeaves.Invoke();
            Ogre.TriggerDisappearOgre(false);
            Debug.Log("you throw the crumbles out of stage, the ogre follows and is gone.");
        }

        CrumblesDisappear.Invoke();
    }


    bool FindThisChild(GameObject obj, string nameToFind) //checks if the object we are searching for is active
    {
        bool childIsFound = false;

        foreach (Transform eachChild in obj.transform)
        {
            if (eachChild.name == nameToFind)
            {
                //Debug.Log("Child found. Mame: " + eachChild.name);
                if (eachChild.gameObject.activeSelf) 
                {
                    childIsFound = true;
                }
                break;
            }
        }

        Debug.Log(childIsFound);
        return childIsFound;
    }
}
