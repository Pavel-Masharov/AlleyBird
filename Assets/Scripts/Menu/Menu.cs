using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject loading;
    [SerializeField] private Slider bar;
    public void Play()
    {
        loading.SetActive(true);
        StartCoroutine(LadingSceneGame());
    }
    IEnumerator LadingSceneGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        while (!asyncLoad.isDone)
        {
            bar.value = asyncLoad.progress;
            yield return null;
        }
    }
}
