using System;
using System.Collections.Generic;
using DG.Tweening;
using Mechanics.GameLevel.Stages.ElectroStageParts.Machines;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.SketchBook
{
    public class Book : MonoBehaviour
    {
        public Transform ParentForPage => _parentForPage;

        [SerializeField] private Button _nextPageButton;
        [SerializeField] private Button _prevPageButton;
        [SerializeField] private FactoryPage _factoryPage;
        [SerializeField] private Transform _parentForPage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _durationMove;
        [SerializeField] private float _durationFade;
        
        private List<Page> _pages;
        private int _currentPageIndex = 0;

        private void Awake()
        {
            _nextPageButton.onClick.AddListener(OnNextPage);
            _prevPageButton.onClick.AddListener(OnPrevPage);
        }
        
        private void Start()
        {
            _pages = _factoryPage.CreatePage(this);
            _currentPageIndex = 0;
            _pages[_currentPageIndex].Show(0.0001f);
            SetActiveButtonByIndex();
        }

        public void SetInteractable(bool isActive) => _canvasGroup.interactable = isActive;

        private void OnPrevPage()
        {
            if (_currentPageIndex - 1 < 0) return;
            ChangePage(_currentPageIndex - 1, TypeSwipe.ToRight);
            _currentPageIndex--;
        }

        private void OnNextPage()
        {
            if (_currentPageIndex + 1 >= _pages.Count) return;
            ChangePage(_currentPageIndex+1, TypeSwipe.ToLeft);
            _currentPageIndex++;
        }

        private void ChangePage(int newPage, TypeSwipe type)
        {
            ChangeInteractableButtonTo(false);
            if (type == TypeSwipe.ToLeft)
            {
                MovePage(
                    _pages[_currentPageIndex], 
                    CenterPoint(), 
                    LeftPoint(), 
                    () => _pages[_currentPageIndex].Hide(_durationFade));
                MovePage(
                    _pages[newPage], 
                    RightPoint(), 
                    CenterPoint(), 
                    () => _pages[newPage].Show(_durationFade), 
                    () => ChangeInteractableButtonTo(true));
            }
            else
            {
                MovePage(
                    _pages[_currentPageIndex], 
                    CenterPoint(), 
                    RightPoint(), 
                    () => _pages[_currentPageIndex].Hide(_durationFade));
                MovePage(
                    _pages[newPage], 
                    LeftPoint(), 
                    CenterPoint(), 
                    () => _pages[newPage].Show(_durationFade), 
                    () => ChangeInteractableButtonTo(true));
            }
        }

        private void MovePage(Page page, Vector2 startPoint, Vector2 endPoint, Action actionOnProcess, Action callback = null)
        {
            page.RectTransform.anchoredPosition = startPoint;
            actionOnProcess?.Invoke();
            page.RectTransform.DOAnchorPos(endPoint, _durationMove).OnComplete(()=>callback?.Invoke());
        }
        
        private Vector2 RightPoint() => new Vector2((float) (Screen.width*1.5),0);

        private Vector2 LeftPoint() => new Vector2((float) (Screen.width*-1.5), 0);

        private Vector2 CenterPoint() => new Vector2(0, 0);

        private void ChangeInteractableButtonTo(bool isActive)
        {
            _nextPageButton.interactable = _prevPageButton.interactable = isActive;
            SetActiveButtonByIndex();
        }

        private void SetActiveButtonByIndex()
        {
            if (_currentPageIndex == 0)
                _prevPageButton.interactable = false;
            if (_currentPageIndex == _pages.Count-1)
                _nextPageButton.interactable = false;
        }
        
        private enum TypeSwipe
        {
            ToLeft, ToRight
        }
    }
}