using System;
using Scripts.Serializables.Story;
using Scripts.UISystem;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Scripts.Managers;
using System.Collections;
using UnityEngine.UI;
using Scripts;

public class ShopCurrencyWindow : WindowController
{
    [SerializeField] private CanvasGroup contentContainerCanvasGroup;
    [SerializeField] private float switchContentSpeed = 0.5f;

    private GameManager _gameManager;
    private bool _isFirstTimeOpen = true;
    private WindowController _currentContent;
    private ContentType _currentContentType = ContentType.Nothing;
    private bool _isContentWindowOpening;
    private bool _isShopCurrentClosing;

    [SerializeField] private TextMeshProUGUI cocktails;
    [SerializeField] private TextMeshProUGUI diamonds;
    [SerializeField] private TextMeshProUGUI elixirs;
    [SerializeField] private GameObject leftButton, cocktailButton, rubiesButton, elixirButton;
    private enum ContentType
    {
        Cocktails,
        Rubies,
        Elixir,
        Nothing
    }

    private void Start()
    {        
        _gameManager = GameManager.Instance;
        UpdateCurrencyBar();
        //_gameManager.OnLoadUserDataComplete += UpdateCurrencyBar;//возможно заменить на FirebaseManager.Instance как в MenuScreen, а в целом скоерй это скорей лишний пункт
        leftButton.GetComponent<Button>().enabled = false;
        cocktailButton.GetComponent<Button>().enabled = false;
        rubiesButton.GetComponent<Button>().enabled = false;
        elixirButton.GetComponent<Button>().enabled = false;
        StartCoroutine(DelayTouch());
        ////подписать еще на ивент траты с риплея истории, акта, покупки в магазине самой валюты
    }
    public void OpenNewContent(int typeIndex)
    {
        var contentType = ContentType.Nothing;
        switch (typeIndex)
        {
            case 0:
            {
                contentType = ContentType.Cocktails;
                break;
            }
            case 1:
            {
                contentType = ContentType.Rubies;
                break;
            }
            case 2:
            {
                contentType = ContentType.Elixir;
                break;
            }
        }

        if (_currentContentType != contentType && _isContentWindowOpening == false)
            _currentContentType = contentType;
        else
            return;
        OpenContentAnimation(() =>
        {
            CloseCurrentContent();
            CreateNewContent(contentType);
        });
    }

    public void ClotheWindowSetup()
    {
        _isShopCurrentClosing = true;
        CloseWindow();
    }

    private void CreateNewContent(ContentType contentType)
    {
        if (_isShopCurrentClosing) return;
        WindowController content = null;
        switch (contentType)
        {
            case ContentType.Cocktails:
            {
                content = WindowsManager.Instance.CreateWindow<CocktailContentWindow>();
                break;
            }
            case ContentType.Rubies:
            {
                content = WindowsManager.Instance.CreateWindow<RubiesContentWindow>();
                break;
            }
            case ContentType.Elixir:
            {
                content = WindowsManager.Instance.CreateWindow<ElixirContentWindow>();
                break;
            }
        }

        _currentContent = content;
        if (_currentContent != null) _currentContent.transform.SetParent(contentContainerCanvasGroup.transform);
    }

    private void CloseCurrentContent()
    {
        if (_currentContent != null)
            _currentContent.CloseWindow();
    }

    private void OpenContentAnimation(Action midEVent = null)
    {
        _isContentWindowOpening = true;
        var fadeInSpeed = switchContentSpeed / 2;
        var fadeOutSpeed = switchContentSpeed / 2;
        var sequence = DOTween.Sequence();
        if (_isFirstTimeOpen)
            fadeInSpeed = 0;

        sequence.Append(contentContainerCanvasGroup.DOFade(0, fadeInSpeed));
        sequence.AppendCallback(() => midEVent?.Invoke());
        sequence.Append(contentContainerCanvasGroup.DOFade(1, fadeOutSpeed)).OnComplete(() =>
        {
            _isFirstTimeOpen = false;
            _isContentWindowOpening = false;
        });
    }

    public void UpdateCurrencyBar()
    {
        cocktails.text = _gameManager.userData.currencies.cocktails.ToString();
        diamonds.text = _gameManager.userData.currencies.cash.ToString();
        elixirs.text = _gameManager.userData.currencies.elixirs.ToString();
    }

    IEnumerator DelayTouch()
    {
        yield return new WaitForSeconds(0.8f);
        leftButton.GetComponent<Button>().enabled = true;
        cocktailButton.GetComponent<Button>().enabled = true;
        rubiesButton.GetComponent<Button>().enabled = true;
        elixirButton.GetComponent<Button>().enabled = true;
    }
    private void OnDestroy()
    {
        //_gameManager.OnLoadUserDataComplete -= UpdateCurrencyBar;
    }
} 