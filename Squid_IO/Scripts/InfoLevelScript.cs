using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoLevelScript : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Text levelName;
    [SerializeField] Text info;
    [SerializeField] Image screenshot;
    [Space]
    [Header("Components")]
    [SerializeField] string[] levelNames;
    [SerializeField] string[] infos;
    [SerializeField] Sprite[] screenshots;

    int rnd;

    private void Awake()
    {
        levelName.text = levelNames[(int)MenuScript.level];
        info.text = infos[(int)MenuScript.level];
        screenshot.sprite = screenshots[(int)MenuScript.level];
    }

    public void ButtonOKPressed()
    {
        switch (MenuScript.level)
        {
            case MenuScript.Level.runner:
                rnd = Random.Range(1, 4);
                SceneManager.LoadScene("Level1_" + rnd.ToString());
                break;
            case MenuScript.Level.hexes:
                SceneManager.LoadScene("Level2");
                break;
            case MenuScript.Level.fight:
                rnd = Random.Range(1, 4);
                SceneManager.LoadScene("Level3_" + rnd.ToString());
                break;
            default:
                break;
        }
    }
}
