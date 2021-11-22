using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsList : MonoBehaviour
{
    [SerializeField] private GameObject _sceneObj;
    [SerializeField] private GameObject _buttonsObj;
    [SerializeField] private Transform _groupItems;
    [SerializeField] private List<LevelItem> _levelItems = new List<LevelItem>();
    private int _levelUnlock;

    void Start()
    {
        //Обработать каждый элемент LevelItem
        for (int i = 0; i < _groupItems.childCount; i++)
        {
            _levelItems.Add(_groupItems.GetChild(i).GetComponent<LevelItem>());
            _levelItems[i].PutNumber(i + 1);
        }

        _levelUnlock = PlayerPrefs.GetInt("LevelsPassed", 0) + 1;

        for (int i = 0; i < _levelUnlock; i++)
        {
            _levelItems[i].MarkLevelAsPassed();
        }
    }
    

    public void OnClickButtonClose()
    {
        _sceneObj.SetActive(true);
        _buttonsObj.transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
