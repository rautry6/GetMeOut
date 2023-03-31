using System;
using DG.Tweening;
using UnityEngine;



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

    public void LoadSpecificScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
