using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class CharacterSelectionController : BaseController
    {
        #region Fields

        private PlayerProfile _playerProfile;
        private Context _context;

        private CharacterSelectionTurnTableView _turnTableView;
        private CharacterSelectionUiView _uiView;

        private int _currentCharacter;
        
        private float _angle;

        private const float RADIUS = 1.5f;
        private const float CHARACTER_ANGLE_OFFSET = -25;
        private const float TURN_TABLE_ANGLE_OFFSET = 180;

        #endregion


        #region ClassLifeCycles

        public CharacterSelectionController(PlayerProfile pLayerProfile)
        {
            Camera.main.orthographic = false;

            _playerProfile = pLayerProfile;
            _context = ServiceLocator.GetService<Context>();

            InstantiatePrefab<CharacterSelectionTurnTableView>(_context.SelectionBasePrefab, null, InitTurnTableView);
            InstantiatePrefab<CharacterSelectionUiView>(_context.UIPrefabsData.CharacterSelectionUI, _context.UiHolder, InitUiView);

            LoadCharacterModels();
        }

        #endregion


        #region Methods

        private void InitTurnTableView(CharacterSelectionTurnTableView view)
        {
            _turnTableView = view;
        }

        private void InitUiView(CharacterSelectionUiView view)
        {
            _uiView = view;

            _uiView.StartButton.onClick.AddListener(StartGame);
            _uiView.BackButton.onClick.AddListener(BackToMenu);
            _uiView.NextButton.onClick.AddListener(() => ChangeCharacter(true));
            _uiView.PrevButton.onClick.AddListener(() => ChangeCharacter(false));
        }

        private void LoadCharacterModels()
        {
            int amount = _context.CustomizationElementsConfig.CharacterModelPrefabs.Length;
            
            _angle = 360 / amount;

            for (int i = 0; i < amount; i++)
            {
                var characterModel = Object.Instantiate(_context.CustomizationElementsConfig.CharacterModelPrefabs[i]);

                AddGameObject(characterModel);

                characterModel.transform.parent = _turnTableView.TurnTable;

                characterModel.transform.localPosition = new Vector3(RADIUS * Mathf.Sin(_angle * i * Mathf.Deg2Rad), 0, RADIUS * Mathf.Cos(_angle* i * Mathf.Deg2Rad));
                characterModel.transform.localRotation = Quaternion.Euler(new Vector3(0, _angle * i + CHARACTER_ANGLE_OFFSET, 0));

                _turnTableView.TurnTable.localRotation = Quaternion.Euler(new Vector3(0, TURN_TABLE_ANGLE_OFFSET, 0));
            }
        }

        private void ChangeCharacter(bool doIncrement)
        {
            if (doIncrement)
            {
                _currentCharacter = _currentCharacter + 1 < _context.CustomizationElementsConfig.CharacterModelPrefabs.Length ?
                    _currentCharacter + 1 : 0;

                _turnTableView.RotateByAngle(-_angle);
            }
            else
            {
                _currentCharacter = _currentCharacter - 1 >= 0 ?
                    _currentCharacter - 1 : _context.CustomizationElementsConfig.CharacterModelPrefabs.Length - 1;
                _turnTableView.RotateByAngle(_angle);
            }
        }

        private void BackToMenu()
        {
            _playerProfile.CurrentState.Value = GameState.Menu;
        }

        private void StartGame()
        {
            var customizationInfo = _playerProfile.CustomizationInfo;
            customizationInfo.ModelIndex = _currentCharacter;
            _playerProfile.CustomizationInfo = customizationInfo;

            if (_context.CharacterOptions.IsCharacterCustomizeable)
                _playerProfile.CurrentState.Value = GameState.Customization;
            else
                _playerProfile.CurrentState.Value = GameState.Game;
        }

        #endregion


        #region IDisposeable

        protected override void OnDispose()
        {
            _uiView.StartButton.onClick.RemoveAllListeners();
            _uiView.BackButton.onClick.RemoveAllListeners();
            _uiView.NextButton.onClick.RemoveAllListeners();
            _uiView.PrevButton.onClick.RemoveAllListeners();

            base.OnDispose();
        }

        #endregion
    }
}