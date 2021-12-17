using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public static float botAmount = 19;
    [SerializeField] Text text;

    public static Level level;

    public void StartGame()
    {
        level = Level.runner;
        SceneManager.LoadScene("InfoLevel");
    }

    public void Level2()
    {
        level = Level.hexes;
        SceneManager.LoadScene("InfoLevel");
    }

    public void OnValueChanged(float value) 
    {
        botAmount = value;
        text.text = botAmount.ToString();
    }

    public void Quit()
    {
        Application.Quit();
    }


    public enum Level
    { 
        runner,
        hexes,
        fight
    }
}
