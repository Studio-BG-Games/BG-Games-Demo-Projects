using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LuckyJumpAnim : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float increaseSpeed;
    [SerializeField] private float increaseScale;
    [SerializeField] private float colorfadeOut;
    [SerializeField] private GameObject backLight;
    [SerializeField] private Transform maskTransform;

    public bool increase;
    public bool IsMultyX;
    public bool increaseOnFail;
    public int figureAngles;
    private GameController gameController;
    private bool backLightInstantinated;
    private float angel;
    private float reduce;
    private float transparancyReduce;
    private float maskScaleChange;
    float needToReduce;
    float needToReduceMask;
    private int turnsAmount;
    private bool initialized;
    private SpriteRenderer mainObjectSprite;
    private SpriteRenderer backLightSprite;

    Vector3 scale;

    public static GameObject animInstanse;


    private void Awake()
    {
        if(animInstanse == null)
        {
            animInstanse = this.gameObject;
        }
        else
        {
            Destroy(animInstanse);
            animInstanse = this.gameObject;
        }
    }

    void Start()
    {
        gameController = GameObject.Find("EventSystem").GetComponent<GameController>();
        if (gameController.scoreMuliplier > 1)
            IsMultyX = true;
        player = GameObject.Find("Player").GetComponent<Player>();
        maskTransform = transform.GetChild(0).GetChild(1);
        backLight = transform.GetChild(0).GetChild(0).gameObject;
        scale = transform.GetChild(0).transform.localScale;
        player.IsOnStartRotation = true;
        mainObjectSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        backLightSprite = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        mainObjectSprite.color = new Color(mainObjectSprite.color.r, mainObjectSprite.color.g, mainObjectSprite.color.b, 0.15f);
        CalculateRedusing();
        ReduseOnStart();
    }

    private void  CalculateRedusing()
    {
        angel = player._angel+1;
        turnsAmount = (int)angel;
        needToReduce = scale.x-0.85f;
        needToReduceMask = maskTransform.localScale.x-1.03f;
        reduce = needToReduce / (angel);
        maskScaleChange = needToReduceMask / (angel-1);
        transparancyReduce = (0.7f-0.15f) / (angel-1);

    }

    private void ReduseOnStart() 
    {
        float reduseMultiplier = figureAngles - (angel -1)-1;
        float startReduse = (needToReduce / (figureAngles - 1)) * reduseMultiplier;
        float startReduseMask = (needToReduceMask / (figureAngles - 1)) * reduseMultiplier;
        float startReduseTranparancy = ((0.7f - 0.15f) / (figureAngles - 1)) * reduseMultiplier;
        if (reduseMultiplier == 0)
            return;
        Vector3 maskScale = maskTransform.localScale;
        mainObjectSprite.color = new Color(mainObjectSprite.color.r, mainObjectSprite.color.g, mainObjectSprite.color.b, mainObjectSprite.color.a + (transparancyReduce * startReduseTranparancy));
        scale.x -= startReduse;
        scale.y -= startReduse;
        maskScale.x -= maskScaleChange * reduseMultiplier;
        maskScale.y -= maskScaleChange * reduseMultiplier;
        transform.GetChild(0).localScale = scale;
        maskTransform.localScale = maskScale;
        CalculateRedusing();

    }


    private void DoIcreaseAnimation(bool success, float maxScale, float speed, float fadeout)
    {
        if(!backLightInstantinated)
        {
            backLight.SetActive(success);
            backLightInstantinated = true;
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;

        }
        mainObjectSprite.color = new Color(mainObjectSprite.color.r, mainObjectSprite.color.g, mainObjectSprite.color.b, mainObjectSprite.color.a - fadeout);
        backLightSprite.color = new Color(backLightSprite.color.r, backLightSprite.color.g, backLightSprite.color.b, mainObjectSprite.color.a - fadeout);
        CheckOnEndEvent();
        scale.x += speed;
        scale.y += speed;
        transform.GetChild(0).localScale = scale;

         void CheckOnEndEvent()
        {
            if (scale.x >= maxScale)
            {
                Destroy(gameObject);
                increase = false;
                increaseOnFail = false;
                return;
            }
        }
    }

    
    private void SetAnimParamenrs()
    {
        mainObjectSprite.color = new Color(mainObjectSprite.color.r, mainObjectSprite.color.g, mainObjectSprite.color.b, 1f);
        maskTransform.localScale = new Vector3(1.04f, 1.04f, 1);
    }
    public IEnumerator<float> ReduseAmount()
    {
        Vector3 maskScale = maskTransform.localScale;
        for (int i = 0; i<5; i++)
        {
            scale.x -= reduce / 5.0f;
            scale.y -= reduce / 5.0f;
            maskScale.x -= maskScaleChange / 5.0f;
            maskScale.y -= maskScaleChange / 5.0f;
            transform.GetChild(0).localScale = scale;
            maskTransform.localScale = maskScale;
            mainObjectSprite.color = new Color(mainObjectSprite.color.r, mainObjectSprite.color.g, mainObjectSprite.color.b, mainObjectSprite.color.a + (transparancyReduce/5));
            yield return Timing.WaitForSeconds(0f);
        }
        angel -= reduce;
        turnsAmount--;
        if (turnsAmount == 0)
        {
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            SetAnimParamenrs();
        }
        yield break;
    }

    void FixedUpdate()
    {
        if (player.IsOnStartRotation && turnsAmount>0)
        {
            Timing.RunCoroutine(ReduseAmount().CancelWith(gameObject));
            player.IsOnStartRotation = false;
        }
        if(player.isLuckyJump && turnsAmount == 0 && increase)
        {
            DoIcreaseAnimation(true, increaseScale, increaseSpeed,colorfadeOut);
        }
        if (increaseOnFail)
        {
            DoIcreaseAnimation(false, 2.3f,increaseSpeed+0.04f, colorfadeOut-0.01f);
        }
        if ( gameController._death )
        {
            Destroy(gameObject);
            Timing.KillCoroutines("ChangeTrasparancy");
        }

    }
}
