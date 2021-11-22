using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Serializables.Story;
using UnityEngine;

public class OptionWindow : MonoBehaviour
{
    public event Action<int> OnOptionSelected;
    [SerializeField] private GameObject optionButtonPrefab;
    [SerializeField] private GameObject chainButtonPrefab;
    [SerializeField] private Transform timerContainer;
    [SerializeField] private GameObject timerPrefab;
    private List<OptionButton> _optionButtons = new List<OptionButton>();
    private List<ChainButton> _chainButtons = new List<ChainButton>();
    private int _badOptionIndex;
    private Timer _currentTimer;

    public void CreateOptionButton(Option option, int index, bool manyOptions)
    {
        var optionButtonGameObject = Instantiate(optionButtonPrefab, transform);
        if (manyOptions) optionButtonGameObject.transform.localScale = new Vector2(0.8f, 0.8f);
        var optionButton = optionButtonGameObject.GetComponent<OptionButton>();
        optionButton.SetOptionStuff(option, index);
        _optionButtons.Add(optionButton);
        if (option.timer > 0) _badOptionIndex = index;
        optionButton.OnOptionSelected += SelectOption;
    }

    public ChainButton CreateChainButton()
    {
        var chainButtonGameObject = Instantiate(chainButtonPrefab, transform);
        var chainButton = chainButtonGameObject.GetComponent<ChainButton>();
        _chainButtons.Add(chainButton);
        return chainButton;
    }

    public void DeleteAllChainButtons()
    {
        if (_chainButtons.Count == 0) return;
        foreach (var chainButton in _chainButtons)
        {
            Destroy(chainButton.gameObject);
        }

        _chainButtons.Clear();
    }

    public void DeleteAllOptionButtons()
    {
        if (_optionButtons.Count == 0) return;
        foreach (var optionButton in _optionButtons)
        {
            optionButton.OnOptionSelected -= SelectOption;
            Destroy(optionButton.gameObject);
        }

        _optionButtons.Clear();
    }

    public bool ChainButtonCompeteCheck()
    {
        foreach (var chainButton in _chainButtons)
        {
            if (!chainButton.isComplete)
            {
                return false;
            }
        }

        return true;
    }

    public void CreateTimer(float timerDuration)
    {
        var timerObj = Instantiate(timerPrefab, timerContainer);
        var timer = timerObj.GetComponent<Timer>();
        _currentTimer = timer;
        timer.SetTimerDuration(timerDuration);
        timer.ActivateTimer();
        timer.OnTimerEnd += () => SelectOption(_badOptionIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            CreateTimer(10);
        }
    }

    private void SelectOption(int index)
    {
        if (_currentTimer != null) Destroy(_currentTimer.gameObject);
        OnOptionSelected?.Invoke(index);
        DeleteAllOptionButtons();
    }
}