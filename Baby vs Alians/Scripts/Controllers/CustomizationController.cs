using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class CustomizationController : BaseController
    {
        #region Fields

        private PlayerProfile _playerProfile;
        private Context _context;

        private CustomizationCharacterView _characterView;
        private CustomizationUIView _uiView;

        private CustomizationElementsConfig _customizationElements;

        private Dictionary<GameObject, GameObject> _guns = new Dictionary<GameObject, GameObject>();

        private SubscriptionProperty<int> _currentShirt = new SubscriptionProperty<int>();
        private SubscriptionProperty<int> _currentHair = new SubscriptionProperty<int>();
        private SubscriptionProperty<int> _currentGun = new SubscriptionProperty<int>();

        private List<GameObject> _defaultGuns;
        private bool _isUsingGunsIncludedWithModel;

#endregion


        #region ClassLifeCycles

        public CustomizationController(PlayerProfile playerProfile)
        {
            _playerProfile = playerProfile;
            _context = ServiceLocator.GetService<Context>();
            _customizationElements = _context.CustomizationElementsConfig;

            Camera.main.orthographic = false;

            InstantiatePrefab<CustomizationCharacterView>(_context.CustomizationCharacterPrefab, null, InitCharacterView);
            InstantiatePrefab<CustomizationUIView>(_context.UIPrefabsData.CustomizationUI, _context.UiHolder, InitUiView);

            _currentShirt.Value = _playerProfile.CustomizationInfo.ShirtIndex;
            _currentHair.Value = _playerProfile.CustomizationInfo.HairIndex;
            _currentGun.Value = _playerProfile.CustomizationInfo.GunIndex;

            _currentShirt.SubscribeOnChange(ChangeShirt);
            _currentHair.SubscribeOnChange(ChangeHair);
            _currentGun.SubscribeOnChange(ChangeGun);
        }

        #endregion


        #region Methods

        private void InitCharacterView(CustomizationCharacterView view)
        {
            _characterView = view;

            var modelIndex = _playerProfile.CustomizationInfo.ModelIndex >= 0 &&
                _playerProfile.CustomizationInfo.ModelIndex < _customizationElements.CharacterModelPrefabs.Length ?
                _playerProfile.CustomizationInfo.ModelIndex : 0;

            var modelPrefab = _customizationElements.CharacterModelPrefabs[modelIndex];
            var model = Object.Instantiate(modelPrefab);

            _characterView.InitCharacterModel(model);

            FindDefaultGuns();

            if (!_isUsingGunsIncludedWithModel)
            {
                var shirtMaterial = _customizationElements.ShirtMaterials[_currentShirt.Value];
                var hairtMaterial = _customizationElements.HairMaterials[_currentHair.Value];
                var gun = GetGunByIndex(_currentGun.Value);

                _characterView.Init(gun, shirtMaterial, hairtMaterial);
            }
        }
        private void InitUiView(CustomizationUIView view)
        {
            _uiView = view;

            _uiView.StartButton.onClick.AddListener(StartGame);
            _uiView.BackButton.onClick.AddListener(BackToMenu);
            _uiView.ShirtNextButton.onClick.AddListener(() => ModifyShirtValue(true));
            _uiView.ShirtPrevButton.onClick.AddListener(() => ModifyShirtValue(false));
            _uiView.HairNextButton.onClick.AddListener(() => ModifyHairValue(true));
            _uiView.HairPrevButton.onClick.AddListener(() => ModifyHairValue(false));
            _uiView.GunNextButton.onClick.AddListener(() => ModifyGunValue(true));
            _uiView.GunPrevButton.onClick.AddListener(() => ModifyGunValue(false));
        }

        private void FindDefaultGuns()
        {
            _defaultGuns = new List<GameObject>();

            foreach (var child in _characterView.GetComponentsInChildren<Transform>())
                if (child.CompareTag("DefaultGun"))
                {
                    child.gameObject.SetActive(false);
                    _defaultGuns.Add(child.gameObject);
                }

            if (_defaultGuns.Count > 1)
            {
                _isUsingGunsIncludedWithModel = true;
                _playerProfile.CustomizationInfo.GunIndex =
                    _playerProfile.CustomizationInfo.GunIndex >= 0 && _playerProfile.CustomizationInfo.GunIndex < _defaultGuns.Count ?
                    _playerProfile.CustomizationInfo.GunIndex : 0;

                ChangeGun(_playerProfile.CustomizationInfo.GunIndex);
            }
        }

        private void ModifyShirtValue(bool doIncrement)
        {
            if (doIncrement)
                _currentShirt.Value = _currentShirt.Value + 1 < _customizationElements.ShirtMaterials.Length ?
                    _currentShirt.Value + 1 : 0;
            else
                _currentShirt.Value = _currentShirt.Value - 1 >= 0 ?
                    _currentShirt.Value - 1 : _customizationElements.ShirtMaterials.Length - 1;
        }

        private void ModifyHairValue(bool doIncrement)
        {
            if (doIncrement)
                _currentHair.Value = _currentHair.Value + 1 < _customizationElements.HairMaterials.Length ?
                    _currentHair.Value + 1 : 0;
            else
                _currentHair.Value = _currentHair.Value - 1 >= 0 ?
                    _currentHair.Value - 1 : _customizationElements.HairMaterials.Length - 1;
        }

        private void ModifyGunValue(bool doIncrement)
        {
            var guns = _isUsingGunsIncludedWithModel ? _defaultGuns.ToArray() : _customizationElements.GunPrefabs;

            if (doIncrement)
                _currentGun.Value = _currentGun.Value + 1 < guns.Length ?
                    _currentGun.Value + 1 : 0;
            else
                _currentGun.Value = _currentGun.Value - 1 >= 0 ?
                    _currentGun.Value - 1 : guns.Length - 1;
        }

        private void BackToMenu()
        {
            SetCustomizationInfo();
            _playerProfile.CurrentState.Value = GameState.Menu;
        }

        private void StartGame()
        {
            SetCustomizationInfo();
            _playerProfile.CurrentState.Value = GameState.Game;
        }

        private void ChangeShirt(int newIndex)
        {
            _characterView.SetShirtMaterial(_customizationElements.ShirtMaterials[newIndex]);
        }

        private void ChangeHair(int newIndex)
        {
            _characterView.SetHairMaterial(_customizationElements.HairMaterials[newIndex]);
        }

        private void ChangeGun(int newIndex)
        {
            if (_isUsingGunsIncludedWithModel)
            {
                foreach (var gun in _defaultGuns)
                    gun.SetActive(false);

                _defaultGuns[newIndex].SetActive(true);
            }
            else
            {
                foreach (var kvp in _guns)
                    kvp.Value.SetActive(false);

                var gun = GetGunByIndex(newIndex);
                gun.SetActive(true);

                _characterView.SetGun(gun);
            }
        }

        private GameObject GetGunByIndex(int index)
        {
            index = (index >= 0 && index < _customizationElements.GunPrefabs.Length) ? index : 0;

            var gunPrefab = _customizationElements.GunPrefabs[index];

            if (!_guns.ContainsKey(gunPrefab))
            {
                var gun = Object.Instantiate(gunPrefab);
                _guns.Add(gunPrefab, gun);
                AddGameObject(gun);
            }

            return _guns[gunPrefab];
        }

        private void SetCustomizationInfo()
        {
            var newCustomizationInfo = _playerProfile.CustomizationInfo;
            newCustomizationInfo.GunIndex = _currentGun.Value;
            newCustomizationInfo.HairIndex = _currentHair.Value;
            newCustomizationInfo.ShirtIndex = _currentShirt.Value; 

            _playerProfile.CustomizationInfo = newCustomizationInfo;
        }

        #endregion


        #region IDisposeavle
        protected override void OnDispose()
        {
            _currentShirt.UnSubscribeOnChange(ChangeShirt);
            _currentHair.UnSubscribeOnChange(ChangeHair);
            _currentGun.UnSubscribeOnChange(ChangeGun);

            _uiView.StartButton.onClick.RemoveAllListeners();
            _uiView.BackButton.onClick.RemoveAllListeners();
            _uiView.ShirtNextButton.onClick.RemoveAllListeners();
            _uiView.ShirtPrevButton.onClick.RemoveAllListeners();
            _uiView.HairNextButton.onClick.RemoveAllListeners();
            _uiView.HairPrevButton.onClick.RemoveAllListeners();
            _uiView.GunNextButton.onClick.RemoveAllListeners();
            _uiView.GunPrevButton.onClick.RemoveAllListeners();

            base.OnDispose();
        }

        #endregion
    }
}