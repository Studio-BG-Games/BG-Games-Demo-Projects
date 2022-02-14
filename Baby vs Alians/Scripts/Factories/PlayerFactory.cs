using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens
{
    public class PlayerFactory : System.IDisposable
    {
        #region Fields

        private PlayerConfig _config;
        private Context _context;

        #endregion


        #region ClassLifeCycles

        public PlayerFactory(Context context)
        {
            _context = context;
            _config = _context.PlayerConfig;
        }

        #endregion


        #region Methods

        public PlayerView GetCharacter(CharacterCustomizationInfo customizationInfo)
        {
            var player = GameObject.Instantiate(_config.PlayerPrefab.gameObject);

            var modelIndex = customizationInfo.ModelIndex >= 0 &&
                customizationInfo.ModelIndex < _context.CustomizationElementsConfig.CharacterModelPrefabs.Length ?
                customizationInfo.ModelIndex : 0;

            var modelPrefab = _context.CustomizationElementsConfig.CharacterModelPrefabs[modelIndex];
            var model = Object.Instantiate(modelPrefab);
            var view = player.GetComponent<PlayerView>();

            view.InitCharacterModel(model);

            if (!_context.CharacterOptions.IsCharacterCustomizeable)
                return view;

            var defaultGuns = FindDefaultGunsIsView(view);
            if (defaultGuns.Count > 1)
            {
                var index = customizationInfo.GunIndex >= 0 && customizationInfo.GunIndex < defaultGuns.Count ?
                    customizationInfo.GunIndex : 0;

                foreach (var defaultGun in defaultGuns)
                    defaultGun.SetActive(false);
                defaultGuns[index].SetActive(true);
                return view;
            }


            var shirtIndex = customizationInfo.ShirtIndex >= 0 &&
                customizationInfo.ShirtIndex < _context.CustomizationElementsConfig.ShirtMaterials.Length ?
                customizationInfo.ShirtIndex : 0;

            var hairIndex = customizationInfo.HairIndex >= 0 &&
                customizationInfo.HairIndex < _context.CustomizationElementsConfig.HairMaterials.Length ?
                customizationInfo.HairIndex : 0;

            var gunIndex = customizationInfo.GunIndex >= 0 &&
                customizationInfo.GunIndex < _context.CustomizationElementsConfig.GunPrefabs.Length ?
                customizationInfo.GunIndex : 0;

            var shirtMaterial = _context.CustomizationElementsConfig.ShirtMaterials[shirtIndex];
            var hairMaterial = _context.CustomizationElementsConfig.HairMaterials[hairIndex];

            var gunPrefab = _context.CustomizationElementsConfig.GunPrefabs[gunIndex];

            var gun = Object.Instantiate(gunPrefab);

            view.InitCustomLooks(gun, shirtMaterial, hairMaterial);

            return view;
        }

        private List<GameObject> FindDefaultGunsIsView(PlayerView _characterView)
        {
            var defaultGuns = new List<GameObject>();

            foreach (var child in _characterView.GetComponentsInChildren<Transform>())
                if (child.CompareTag("DefaultGun"))
                {
                    child.gameObject.SetActive(false);
                    defaultGuns.Add(child.gameObject);
                }

            return defaultGuns;
        }

        #endregion

        #region IDisposable


        public void Dispose()
        {

        }

        #endregion
    }
}