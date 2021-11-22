using UnityEngine;
using UnityEngine.SceneManagement;


public class StartVideoTest : MonoBehaviour
{
    public void EndVideo()
    {
        Debug.Log("Конец воспроизведения");
        SceneManager.LoadScene("Start");
    }
}


