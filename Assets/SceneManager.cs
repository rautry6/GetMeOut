using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void RestartScene(string sceneName)
    {
        var sceneToLoad = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        //Debug.Log(sceneToLoad.buildIndex);
        if (sceneToLoad.buildIndex < 0)
            throw new Exception(
                "RestartScene was given an invalid scene name to load, double check it has been added to the list of build indexes");
        DOTween.KillAll();
        StopAllCoroutines();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
