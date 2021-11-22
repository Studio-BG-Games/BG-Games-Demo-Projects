using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageSettings", menuName = "ScriptableObjects/LanguageSettings", order = 0)]
public class LanguageSettings : ScriptableObject
{
    public event Action OnValueChanged;

    private const string RusLanguageKey = "Rus";
    private const string EngLanguageKey = "Eng";

    private const string LanguagePlayerPrefKey = "Language";

    public Languages DefaultLanguage;

    private string defaultLanguage => DefaultLanguage == Languages.Rus ? RusLanguageKey : EngLanguageKey;

    public enum Languages
    {
        Rus = 0,
        Eng = 1
    }

    public Languages currentLanguage
    {
        get => PlayerPrefs.GetString(LanguagePlayerPrefKey, defaultLanguage) == RusLanguageKey
            ? Languages.Rus
            : Languages.Eng;
        private set =>
            PlayerPrefs.SetString(LanguagePlayerPrefKey, value == Languages.Eng ? EngLanguageKey : RusLanguageKey);
    }

    public void ChooseLanguage(Languages language)
    {
        currentLanguage = language;
        OnValueChanged?.Invoke();
    }
}