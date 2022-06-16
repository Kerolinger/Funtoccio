using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GreenLightSceneTransitions : MonoBehaviour
{

    public GameObject funeralScreen;
    public GameObject candyshopScreen;
    public StageManager stageManager;
    public GameObject EnvironmentAsset;

    public void ActivateFuneral()
    {
        StartCoroutine(Funeral());
    }

    public void ActivateCandyShop(float timeuntillwechsel)
    {
        StartCoroutine(CandyShop(timeuntillwechsel));
    }

    IEnumerator Funeral()
    {
        stageManager.OpenCurtain(false);
        yield return new WaitForSeconds(2f);
        stageManager.OpenCurtain(true);
        funeralScreen.SetActive(true);
        EnvironmentAsset.SetActive(false);
    }

    IEnumerator CandyShop(float timeuntillwechsel)
    {
        yield return new WaitForSeconds(timeuntillwechsel);
        stageManager.OpenCurtain(false);
        yield return new WaitForSeconds(2f);
        stageManager.OpenCurtain(true);
        candyshopScreen.SetActive(true);
        EnvironmentAsset.SetActive(false);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
