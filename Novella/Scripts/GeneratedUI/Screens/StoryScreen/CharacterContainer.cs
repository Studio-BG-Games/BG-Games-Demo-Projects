using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Scripts.Managers;
using Scripts.Serializables.Story;
using Scripts.Serializables.User;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterContainer : MonoBehaviour
{
    [SerializeField] private List<Character> characters = new List<Character>();

    private Character Hero => characters[0];
    private Character Npc => characters[1];

    /*
     * 0 - Hero
     * 1 - Npc
     */

    [Serializable]
    public class Character
    {
        public enum CharacterType
        {
            Hero = 0,
            Npc = 1
        }

        public CharacterType characterType;

        [SerializeField] private RectTransform container;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private List<Image> characterParts = new List<Image>();

        [SerializeField] private float _hidePosition = 30;
        [SerializeField] private float _showPosition = 30;
        [SerializeField] private float _showMoveSpeed = 0.3f;
        [SerializeField] private float _showFadeSpeed = 0.4f;
        [SerializeField] private float _hideMoveSpeed = 0.3f;
        [SerializeField] private float _hideFadeSpeed = 0.4f;

        private Image _imageBody;
        private Image _imageDress;
        private Image _imageEmotion;
        private Image _imageHair;
        private Image _imageMask;
        private float startPosYofNps;
        private PlayerItemBase PlayerItemBase => GameManager.Instance.playerInformation.playerItemBase;

        private NpcBase NpcBase => GameManager.Instance.scriptableBase.npcBase;

        private void Start() 
        {
            if (characterType == CharacterType.Hero) //if (characterType != CharacterType.Npc)
            {
                StoryProgress progress = null;
                Sprite dressSprite;
                Sprite hairSprite;

                var user = GameManager.Instance.userData;
                var selectedStory = GameManager.Instance.selectedStory;
                progress = user.progress.Find(value => value.storyId == selectedStory.id);
                
                dressSprite = progress.HeroDress.sprite;
                hairSprite = progress.HeroHair.sprite;
            }
        }
        public void ParsingBodyParts()
        {
            _imageBody = characterParts.Find(value => value.name.StartsWith("Body"));
            _imageDress = characterParts.Find(value => value.name.StartsWith("Dress"));
            _imageEmotion = characterParts.Find(value => value.name.StartsWith("Emotion"));
            _imageHair = characterParts.Find(value => value.name.StartsWith("Hair"));
            _imageMask = characterParts.Find(value => value.name.StartsWith("Mask"));
        }

        private void BodyPartsNullCheck()
        {
            foreach (var bodyPart in characterParts)
            {
                if (bodyPart.name != "Emotion")
                    bodyPart.gameObject.SetActive(bodyPart.sprite != null);
                else bodyPart.gameObject.SetActive(false);
            }
        }

        private void SetModificator(string[] modifications, NpcDescription npc, StoryProgress progress)
        {
            //npc mod check
            if (characterType == CharacterType.Npc)
            {
                //dress modifications
                foreach (var mod in modifications)
                {
                    if (mod == "hidden" || mod == "mask" || mod == "silhouette") continue;

                    if(npc.name == "max")
                    {
                        if (mod == "naked" || mod == "normal")
                            _imageBody.sprite = npc.GetDressByKey("normal_2");
                        if (mod == "swimsuit")
                        {
                            _imageBody.sprite = npc.GetDressByKey("normal_2");
                            _imageDress.sprite = npc.GetDressByKey(mod);
                            break;
                        }
                    }

                    _imageDress.sprite = npc.GetDressByKey(mod);
                    if (mod == "")
                    {
                        _imageDress.sprite = npc.GetDressByKey("default");
                    }
                }

                foreach (var mod in modifications)
                {

                    //mask modifications
                    if (mod == "mask")
                    {
                        _imageMask.sprite = npc.GetMaskSprite();
                    }
                    //silhouette modifications
                    if (mod == "silhouette")
                    {
                        _imageBody.color = Color.black;
                        _imageEmotion.color = Color.black;
                        _imageHair.color = Color.black;
                        _imageDress.color = Color.black;                  
                    }

                    if(mod == "mirror")
                    {
                        /*_imageBody.transform.Rotate(Vector3.up);
                        _imageBody.transform.position = new Vector2(_imageBody.transform.position.x-100f, 
                                                                    _imageBody.transform.position.y);
                        _imageDress.transform.Rotate(Vector3.up);
                        _imageDress.transform.position = new Vector2(_imageDress.transform.position.x - 100f,
                                                                     _imageDress.transform.position.y);*/

                        _imageDress.sprite = npc.GetDressByKey("default");
                    }
                }

            }
            //hero mod check
            else
            {
                /*foreach (var mod in modifications)
                {
                    if (mod != "hidden" && mod != "mask")
                    {
                        _imageDress.sprite = GameManager.Instance.playerInformation.playerItemBase.GetDresByKey(mod).sprite;
                    }
                }*/

                foreach (var mod in modifications)
                {
                    if (mod == "dress12" || mod == "use:partydress" || mod == "set:partydress") continue;
                    _imageDress.sprite = PlayerItemBase.GetDressByKey(mod).sprite;
                }


                foreach (var mod in modifications)
                {
                    //mask modifications
                    if (mod == "dress12")
                    {
                        _imageMask.sprite = progress.Mask.sprite;
                    }
                }

                if(modifications.Length == 2 && modifications[0] == "naked2")
                {
                    _imageDress.sprite = PlayerItemBase.GetDressByKey("naked2,plaid").sprite;
                }
            }
        }

        public void UpdateCharacterSprites(Record record)
        {
            NpcDescription npc = null;
            StoryProgress progress = null;
            Sprite bodySprite;
            Sprite dressSprite;
            Sprite hairSprite;

            if (characterType == CharacterType.Npc)
            {
                Debug.Log(record.npc);
                npc = NpcBase.FindNpcByKey(record.npc);
                bodySprite = npc.GetBodySprite();
                dressSprite = npc.GetDressByKey("default");
                hairSprite = npc.GetHairSprite();
                if(npc.name=="dog1" || npc.name == "dog2")
                {
                    if (record.background != "street_2_night")
                    {
                        if(Screen.height <= 1920)
                             npc.offsetY = 300;
                        else 
                             npc.offsetY = 540;
                        npc.scale = 1.5f;
                    }
                    else
                    {
                        if (Screen.height <= 1440)
                            npc.offsetY = 400;
                        else if(Screen.height <= 1920)
                            npc.offsetY = 550;
                        else if (Screen.height <= 2160)
                            npc.offsetY = 600;
                        else
                            npc.offsetY = 750;
                        npc.scale = 1.7f;
                    }
                }

                container.localScale = new Vector2(npc.scale,npc.scale);
                container.transform.position = new Vector2(container.transform.position.x + npc.offsetX, startPosYofNps + npc.offsetY);
                _showPosition = npc.offsetX;
            }
            else//hero
            {
                var user = GameManager.Instance.userData;
                var selectedStory = GameManager.Instance.selectedStory;
                progress = user.progress.Find(value => value.storyId == selectedStory.id);

                bodySprite = progress.HeroBody.sprite;
                dressSprite = progress.HeroDress.sprite;
                hairSprite = progress.HeroHair.sprite;
                
                if (record.npcOptions != null && record.npcOptions[0] == "use:partydress")
                {
                    Debug.Log(progress.partydress);
                    progress.heroDressKey = PlayerItemBase.GetDressByKey(progress.partydress).itemKey;
                    dressSprite = PlayerItemBase.GetDressByKey(progress.partydress).sprite;
                }

                _showPosition = -272;
            }

            _imageBody.color = Color.white;
            _imageHair.color = Color.white;
            _imageDress.color = Color.white;
            _imageEmotion.color = Color.white;

            _imageBody.sprite = bodySprite;
            _imageDress.sprite = dressSprite;
            _imageHair.sprite = hairSprite;

            if (record.npcOptions != null)
            {
                SetModificator(record.npcOptions, npc, progress);

                for (int i = 0; i < record.npcOptions.Length; i++)
                {
                    Debug.Log("mod name: " + record.npcOptions[i]);
                }
            }

            BodyPartsNullCheck();
        }

        public void Hide()
        {
            container.DOAnchorPosX(_hidePosition, _hideMoveSpeed);
            canvasGroup.DOFade(0, _hideFadeSpeed);
        }

        public void Show()
        {
            container.DOAnchorPosX(_showPosition, _showMoveSpeed);
            canvasGroup.DOFade(1, _showFadeSpeed);
        }

        public void SetStartPosY()
        {
            startPosYofNps = container.transform.position.y;
        }

        public void ZeroingAlpha()
        {
            canvasGroup.alpha = 0;
        }

        public IEnumerator ChangeEmotion(Record record,string lastNpc, string currentNpc, DialogWindow dialogWindow)
        {           
            NpcDescription npc = null;
            StoryProgress progress = null;
            Sprite emotionSprite;
            float hideMoveSpeed = 0f;
            float wait = 0f;
            bool skipEmotion = false;
            bool canTurnOnEmotion = true;

            if (characterType == CharacterType.Npc)
            {
                Debug.Log(record.npc);
                npc = NpcBase.FindNpcByKey(record.npc);

                if (lastNpc != currentNpc)
                {
                    skipEmotion = true;
                } 

                emotionSprite = npc.GetEmotionByKey(record.emotion);
                if(npc.name=="dog1" || npc.name=="dog2")//нужно нормально что-то дополнительное парсить для определения тела
                {
                    if( record.background != "street_2_night")
                    {
                        _imageBody.sprite = npc.GetDressByKey("normal");
                        if(record.emotion=="joy")
                        {
                            emotionSprite = npc.GetEmotionByKey(record.emotion);
                        }
                    }
                    else
                    {
                        _imageBody.sprite = npc.GetBodySprite();
                        emotionSprite = npc.GetEmotionByKey(record.emotion + "_2");
                    }
                }
                // else
                // {
                //     emotionSprite = npc.GetEmotionByKey(record.emotion);
                // }
                                    
                
                if (record.emotion == "" || record.emotion == null || record.emotion == "normal")
                {
                    skipEmotion = true;
                    canTurnOnEmotion = false;
                }

            }
            else//hero
            {
                var user = GameManager.Instance.userData;
                var selectedStory = GameManager.Instance.selectedStory;
                progress = user.progress.Find(value => value.storyId == selectedStory.id);

                if (lastNpc != currentNpc)
                {               
                    skipEmotion = true;
                    canTurnOnEmotion = true;
                }else
                {
                    canTurnOnEmotion = true;
                }

                emotionSprite = progress.HeroBody.emotions.Find(value => value.name == record.emotion);

                if (record.emotion == "" || record.emotion == null || record.emotion == "normal")
                {
                    skipEmotion = true;
                    canTurnOnEmotion = false;
                }
            }

            if (lastNpc != currentNpc)
            {
                yield return new WaitForSeconds(0.4f);
                hideMoveSpeed = 0.01f;
                wait = 0.005f;
            }
            else
            {
                hideMoveSpeed = 0.3f;
                wait = 0.2f;
            }


            foreach (var bodyPart in characterParts)
            {
                if (bodyPart.name == "Emotion" && !skipEmotion)
                { 
                    bodyPart.gameObject.SetActive(bodyPart.sprite != null); 
                }else if (bodyPart.name == "Emotion" && bodyPart.gameObject.activeSelf)
                {
                    bodyPart.gameObject.SetActive(false);
                }

            }

            _imageEmotion.DOFade(0, hideMoveSpeed);
             yield return new WaitForSeconds(wait);
            _imageEmotion.sprite = emotionSprite;

            if(canTurnOnEmotion)
            {
                foreach (var bodyPart in characterParts)
                {
                    if (bodyPart.name == "Emotion")
                        bodyPart.gameObject.SetActive(bodyPart.sprite != null);
                }
            }

            _imageEmotion.DOFade(1, 0.4f);
            yield return new WaitForSeconds(0.3f);
            if(record.type == "text")
               dialogWindow.SetDialogButtonState(true);
            yield break;
        }

        public void HideEmotion()
        {
            _imageEmotion.DOFade(0, 0.05f);
        }
        public void TurnOffEmotions()
        {
            foreach (var bodyPart in characterParts)
            {
                if (bodyPart.name == "Emotion")
                    bodyPart.gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        InitCharacters();
        Npc.SetStartPosY();
    }

    public void ShowNpc(Record record)
    {
        StartCoroutine(ChangeHeroOnNpc(record));
    }

    public void ShowHero(Record record)
    {
        StartCoroutine(ChangeNpcOnHero(record));
    }

    public void HideAllCharacters()
    {
        StartCoroutine(HideAll());
    }

    public IEnumerator HideAll()
    {
        Npc.HideEmotion();
        Hero.HideEmotion();
        yield return new WaitForSeconds(0.1f);
        Npc.Hide();
        Hero.Hide();
        yield break;
    }

    public IEnumerator ChangeNpcOnHero(Record record)
    {
        Hero.UpdateCharacterSprites(record);
        Npc.HideEmotion();
        yield return new WaitForSeconds(0.1f);
        Npc.Hide();
        Hero.Show();
        yield break;
    }

    public IEnumerator ChangeHeroOnNpc(Record record)
    {
        Npc.ZeroingAlpha();
        Npc.UpdateCharacterSprites(record);
        Hero.HideEmotion();
        Npc.HideEmotion();
        yield return new WaitForSeconds(0.1f);
        Hero.Hide();
        Npc.Show();
        yield break;
    }

    public void ChangeEmotionsHero(Record record, string lastNpc, string currentNpc,DialogWindow dialogWindow)
    {
        StartCoroutine(Hero.ChangeEmotion(record, lastNpc,currentNpc,dialogWindow));
    }
    public void ChangeEmotionsNpc(Record record, string lastNpc, string currentNpc, DialogWindow dialogWindow)
    {
        StartCoroutine(Npc.ChangeEmotion(record, lastNpc,currentNpc, dialogWindow));
    }
    public void TurnOnDialog(DialogWindow dialogWindow, Record record)
    {
        if ((record.backgroundOptions != null && record.backgroundOptions[0] != "poster") || record.backgroundOptions == null)
        {
           StartCoroutine(SetDialog(dialogWindow, record));
        } 

    }
    public IEnumerator SetDialog(DialogWindow dialogWindow, Record record)
    {
        yield return new WaitForSeconds(0.3f);
        if(record.type == "text")
             dialogWindow.SetDialogButtonState(true);
    }
    private void InitCharacters()
    {
        foreach (var character in characters)
        {
            character.ParsingBodyParts();
        }

        HideAllCharacters();
    }

 
}