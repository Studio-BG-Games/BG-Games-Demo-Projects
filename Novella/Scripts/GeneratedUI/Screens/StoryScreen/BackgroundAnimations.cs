using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimations : MonoBehaviour
{
   // public static bool isBedroom = false;

    [SerializeField] private List<GameObject> animations;
    [SerializeField] private GameObject particlesParent;
    [SerializeField] private BackgroundBase backgroundBase;

    private GameObject _spawnedEffect, _pngImage;
    private int _lastAnimIndex = -1, _currentAnimIndex = 0;

    public void UpdateAnimation(string _currentBackground, string[] _backgroundOption)
    {
        foreach (var background in backgroundBase.Backgrounds)
        {
            if(background.name == _currentBackground)
            {
                _currentAnimIndex = background.AnimationKey;

               if (_currentAnimIndex >= 0  && _currentAnimIndex != _lastAnimIndex)
               {
                   if (_spawnedEffect != null)
                   {
                      DestroyEffect(_spawnedEffect);                           
                   }

                   SpawnEffect(background.AnimationKey);
               }
               else if(_currentAnimIndex == _lastAnimIndex && _currentAnimIndex != -1 && _spawnedEffect == null)
               {
                   SpawnEffect(background.AnimationKey);
               }

               if(background.AnimationKey == -1 && _spawnedEffect != null)
               {
                   DestroyEffect(_spawnedEffect);
               }
            }
            else if(_spawnedEffect != null) 
            {
                if( _currentBackground != null && ( _currentBackground == "bedroomX" || 
                (_currentBackground.Length > 8 && _currentBackground.Remove(8, _currentBackground.Length - 8) == "magazine")))
                   DestroyEffect(_spawnedEffect);
            }
        }
        

        for (int imageIndex = 0; imageIndex < animations.Count; imageIndex++)
        {
            if (_backgroundOption != null && _pngImage == null)
            { 
                if (_backgroundOption[0] == animations[imageIndex].name)
                {
                    SpawnImage(imageIndex);
                }
            }

        }

        if (_backgroundOption == null && _pngImage != null)
        {
            DestroyEffect(_pngImage);
        }

    }

    void SpawnEffect(int animationIndex)
    {
        _spawnedEffect = Instantiate
            (animations[animationIndex], 
             transform.position, Quaternion.identity,
             particlesParent.transform);

        _spawnedEffect.transform.localPosition = Vector3.zero;
        _lastAnimIndex = _currentAnimIndex;
    }

    void SpawnImage(int imageIndex)
    {
        _pngImage = Instantiate
            (animations[imageIndex],
             transform.position, Quaternion.identity,
             particlesParent.transform);

        _pngImage.transform.localPosition = Vector3.zero;
    }

    void DestroyEffect(GameObject spawnedEffect)
    {
        Destroy(spawnedEffect);
    }

}
