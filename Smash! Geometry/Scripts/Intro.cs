using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] private Animation animation;
    AsyncOperation asyncLoad;
    bool canLoad = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaiteToLoad());

    }



    IEnumerator ActiveScene()
    {
        yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;
    }

    IEnumerator WaiteToLoad()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(LoadScene());

    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        while (!asyncOperation.isDone)
        {

            if (asyncOperation.progress >= 1 && canLoad)
            {
                Debug.Log("FadeOut");
                animation.Play("FadeOut");
                yield return new WaitForSeconds(0.4f);
                asyncOperation.allowSceneActivation = true;
                canLoad = false;
            }

            yield return null;
        }
    }
}
