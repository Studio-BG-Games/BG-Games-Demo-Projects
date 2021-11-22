using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelItem : MonoBehaviour
{
    private int numLevel;
    [SerializeField] private GameObject _soundLevel;
    [SerializeField] private GameObject _buttonObj;
    [SerializeField] private Text _text;
    [SerializeField] private Sprite _buttonOpenSprite;
    [SerializeField] private Image[] _stars;


    //Отметить уровень как пройденный
    public void MarkLevelAsPassed()
    {
        Image buttonImage = _buttonObj.GetComponent<Image>();
        Button buttonCompon = _buttonObj.GetComponent<Button>();
        float timeLevel = PlayerPrefs.GetFloat("TimeLevel_" + numLevel);

        buttonImage.sprite = _buttonOpenSprite;
        buttonCompon.interactable = true;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 255);

        if (timeLevel > 0f)
        {
            if (timeLevel < 20.0f)
                _stars[0].enabled = true;
            if (timeLevel < 15.0f)
                _stars[1].enabled = true;
            if (timeLevel < 10.0f)
                _stars[2].enabled = true;
        }
    }
    
    //Поставить номер уровня
    public void PutNumber(int num)
    {
        numLevel = num;
        _text.text = num.ToString();
    }


    public void OnClickButtonLevel()
    {
        SceneManager.LoadScene("Level_" + numLevel);
        Instantiate(_soundLevel);
    }
}
