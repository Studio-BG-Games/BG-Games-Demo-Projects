using UnityEngine;
using UnityEngine.SceneManagement;


public class StartVideoFin : MonoBehaviour
{

    public void LoadSceneStart() {
        //SceneManager.LoadScene("StartVideo");
        Application.ExternalEval("window.open('http://game.lenpeh.ru/game.html', '_self')");


    }

}
