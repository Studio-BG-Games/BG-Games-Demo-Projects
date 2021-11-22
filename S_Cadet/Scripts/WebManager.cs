using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WebManager : MonoBehaviour
{
    public Text name;
    public Text lastname;
    public Text old;
    public Text rota;
    public Text vus;
    private int _identifier;


    public void SendData()
    {
        StartCoroutine(Send());
    }

    public IEnumerator Send() 
    {
        if (name.text.Length > 1 && lastname.text.Length > 1)
        {
            _identifier = Random.Range(1000,100000000);
            PlayerPrefs.SetInt("identifier", _identifier);
            
            WWWForm form = new WWWForm();
            form.AddField("name", name.text);
            form.AddField("lastname", lastname.text);          
            form.AddField("old", old.text);
            form.AddField("rota", rota.text);
            form.AddField("vus", vus.text);
            form.AddField("identifier", _identifier);

            WWW www = new WWW("http://game.lenpeh.ru/sendDB.php", form);

            yield return www;

            print(www.text);

            PlayerPrefs.SetString("lastName", lastname.text);
            PlayerPrefs.SetString("fullName", "" + name.text + " " + lastname.text);
            SceneManager.LoadScene("Game");
        }
    }
}
