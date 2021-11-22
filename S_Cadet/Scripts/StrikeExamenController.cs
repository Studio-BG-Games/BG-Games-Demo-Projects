using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StrikeExamenController : MonoBehaviour
{
    public void SendData()
    {
        StartCoroutine(Send());
    }

    public IEnumerator Send()
    {
            WWWForm form = new WWWForm();
            form.AddField("StrikeScore", PlayerPrefs.GetInt("strikeScore"));
            form.AddField("GeneralScore", PlayerPrefs.GetInt("strikeScore") + PlayerPrefs.GetInt("TestScore"));
            form.AddField("identifier", PlayerPrefs.GetInt("identifier"));

            WWW www = new WWW("http://game.lenpeh.ru/UpdateStrikeScoreDB.php", form);

            yield return www;

            print(www.text);

            SceneManager.LoadScene("TableResult");
        }
    
}
