using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;

public class Border : MonoBehaviour
{
    #region Fields
    public int NumberOfFigures;
    public AnimationCurve difficulty;

    [SerializeField] private float _angel;
    [SerializeField] public int _angels;
    [SerializeField] public int _rotateSide;
    [SerializeField] public float scale;

    [SerializeField] private Animation _animation;
    [SerializeField] private GameObject _playerParts;
    [SerializeField] private GameObject _wallParts;
    [SerializeField] private GameObject _eventSystem;
    [SerializeField] public GameObject LuckyJumpEffect;
    [SerializeField] private Vibration _vibrator;
    [SerializeField] private GameObject _player;
    [SerializeField] public bool IsFirst;
    [SerializeField] public PolygonCollider2D Triger;
    [SerializeField] private GameObject _borderTrigger;
    [SerializeField] public bool _activeDestroy;
    [SerializeField] public bool _activeAchive;
    [SerializeField] public bool borderDestring;
    [SerializeField] public bool _playerAlive = true;
    [SerializeField] public bool reviveShown = false;
    [SerializeField] private GameObject[] _goodSound;
    [SerializeField] private AudioSource _badSound;
    [SerializeField] private GameController _gameController;
    [SerializeField] private GameObject _heatWave;
    [SerializeField] private GameObject particleSystem;
    [SerializeField] private GameObject sparkle;
    [SerializeField] private GameObject explosion;
    public ParticleSystem part;

    private SpriteRenderer base1;
    private SpriteRenderer base2;
    private SpriteRenderer base3;
    private SpriteRenderer breaker1;
    private SpriteRenderer breaker2;
     private bool IsDestroing;
     private bool IsIncreasing;
    [SerializeField] private float animWait;
    [SerializeField] private float achiveDown;
    [SerializeField] private AudioSource achiveSound;
    [SerializeField] private Analitics analitcs;

    [SerializeField] GameObject breaker;
    [SerializeField] GameObject figureAchiveComponent;
    [SerializeField] GameObject Achive;
    [SerializeField] GameObject camera;
    [SerializeField] GameObject superPower;
    //

    [SerializeField] public Vector3 _oldPlayerPos;
    private Player _playerClass;
    private GameObject _currentAchive;
    private SpriteRenderer superPowerSpriteRenderer;
    public List<Transform> anglePoints;

    private float colorMinus;
    private float colorPlus;


    #endregion

    private void Awake()
    {
        if (SuperPower.SuperPowerInstance != null)
            Destroy(SuperPower.SuperPowerInstance);
    }

    private void Start()
    {
        camera = GameObject.Find("Main Camera");
        _player = GameObject.Find("Player");
        _playerClass = _player.GetComponent<Player>();
        part = particleSystem.gameObject.GetComponent<ParticleSystem>();
        _goodSound = GameObject.FindGameObjectsWithTag("GoodSound");
        _badSound = GameObject.Find("BadSound").GetComponent<AudioSource>();
        achiveSound = GameObject.Find("AchiveSound").GetComponent<AudioSource>();
        var position = _player.transform.position;
        transform.position = new Vector3(position.x, position.y, 10);
        Timing.RunCoroutine(SpawnAnim());
        _eventSystem = GameObject.Find("EventSystem");
        _gameController = _eventSystem.GetComponent<GameController>();
        analitcs = _eventSystem.GetComponent<Analitics>();
        foreach(Transform child in transform.GetChild(3))
        {
            anglePoints.Add(child);
        }

        //var BorderRes = Resources.Load<FigureInfo>("Resources/Border.asset");
        //colorMinus = BorderRes.ColorMinus;
        //colorPlus = BorderRes.ColorPlus;

        colorMinus = 0.02f;
        colorPlus = 0.04f;
        //Debug.Log("colorPlis - " + colorPlus);
        SetRotationAngle();
        SetVariablesToDestoyAnim();
        CheckFigureAchive();
        Timing.RunCoroutine(InstantinateSuperPower());
        Timing.RunCoroutine( CheckLuckyJump());
    }
    private void FixedUpdate()
    {
        if (_playerClass._isMoving && _playerAlive)
            Triger.isTrigger = true;
        
            DestroyAnim();
        if (_gameController.canDestroyWindow && borderDestring)
        {
            //Debug.Log("1");
            DestroyWindow();
            _gameController.canDestroyWindow = false;
            borderDestring = false;
        }
        if (IsIncreasing && superPowerSpriteRenderer)
        {
            DoTransparancyIncrease(superPowerSpriteRenderer);
        }
    }


    private void DestroyAnim()
    {
        if (IsDestroing)
        {
            DoTransparancyReduse(base1);
            DoTransparancyReduse(base2);
            DoTransparancyReduse(base3);
            if (_gameController.isSuperPowerState)
            {
                DoTransparancyReduse(breaker1);
                DoTransparancyReduse(breaker2);
            }
        }
    }

    public void DoTransparancyReduse(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - colorMinus);
        if (base1.color.a <= 0)
            IsDestroing = false;
    }

    public void DoTransparancyIncrease(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + colorPlus);
        if (spriteRenderer.color.a <= 0)
            IsIncreasing = false;
    }

    private void CalculateSuperPower()
    {
        GameObject superpower;
        int chance = Random.Range(0, 3);
        int firstPoint;
        int secondPoint;
        firstPoint = Random.Range(0, anglePoints.Count);

        if (firstPoint == anglePoints.Count - 1)
            secondPoint = 0;
        else
            secondPoint = firstPoint + 1;

        Vector3 minimum = anglePoints[firstPoint].position;
        Vector3 maximum = anglePoints[secondPoint].position;
        Vector3 difference = maximum - minimum;
        Vector3 new_difference = difference * Random.Range(0.0f, 1.0f);

        Vector3 position = minimum + new_difference;
        position = new Vector3(position.x, position.y, 8);
        
        if (chance == 2 && !_gameController.firstFigure)
        {
            if (PlayerPrefs.GetInt("superPowerUnlock") != 1)
            {
                Debug.Log("first superpower");
                firstPoint = anglePoints.Count - 1;
                secondPoint = 0;
                position = new Vector3((anglePoints[firstPoint].position.x + anglePoints[secondPoint].position.x) / 2,
                    (anglePoints[firstPoint].position.y + anglePoints[secondPoint].position.y) / 2, 8);
                PlayerPrefs.SetInt("superPowerUnlock", 1);
            }
            superpower = Instantiate(superPower, position, anglePoints[secondPoint].rotation);
            superPowerSpriteRenderer = superpower.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            superPowerSpriteRenderer.color = new Color(superPowerSpriteRenderer.color.r, superPowerSpriteRenderer.color.g, superPowerSpriteRenderer.color.b, 0);
            IsIncreasing = true;
        }
    }

    private void SetRotationAngle()
    {
        _player.GetComponent<Player>()._angels = _angels;
        if (_angels > 4)
            _angel = Random.Range(1, 5);
        else
            _angel = Random.Range(1, _angels);
        if (!IsFirst)
        {
            transform.Rotate(new Vector3(0, 0, (_player.transform.localRotation.eulerAngles.z) + _angel * _rotateSide * (360 / _angels)));
        }
        _player.gameObject.GetComponent<Player>()._angel = _angel;
    }

    private void SetVariablesToDestoyAnim()
    {
        base1 = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
        base2 = transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>();
        base3 = transform.GetChild(1).GetChild(2).GetComponent<SpriteRenderer>();
        breaker1 = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        breaker2 = transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>();

    }
     
    public void DestroyWindow()
    {
        transform.GetChild(2).gameObject.SetActive(false);       
        InstantinateGlassAndEffects(transform.GetChild(0).transform.position);
        transform.GetChild(0).gameObject.SetActive(false);
        Timing.RunCoroutine(Destroy().CancelWith(gameObject));
        _playerClass._angel = _angel;
        _gameController.RewriteScore();
        GetComponent<PolygonCollider2D>().enabled = false;

    }

    public void InstantinateGlassAndEffects(Vector3 position)
    {
        transform.GetChild(2).gameObject.SetActive(false);
        var brk = Instantiate(breaker, position, Quaternion.Euler(0, 0, _player.transform.rotation.eulerAngles.z + 270));
        brk.transform.localScale = new Vector3(scale, brk.transform.localScale.y, brk.transform.localScale.z);
        Instantiate(particleSystem, position, _player.transform.rotation);
        Instantiate(_heatWave, position, Quaternion.identity);
        Instantiate(explosion, position, Quaternion.Euler(0, 0, 90));
        if (!_gameController._soundMute)
            _goodSound[Random.Range(0, _goodSound.Length)].GetComponent<AudioSource>().Play();
    }

    
    private void CheckFigureAchive()
    {
        if(PlayerPrefs.GetInt(gameObject.name+"AchiveAnimation") == 0)
        {
            AnimateFigureAchive();
            PlayerPrefs.SetInt(gameObject.name+"AchiveAnimation", 1);
        }
    }

    public void AnimateFigureAchive()
    {

        if (gameObject.name != "BorderX3(Clone)")
        {      
            Timing.RunCoroutine(InstatninateFigureAchive());
            Timing.RunCoroutine(InstanitnatePopUp());
        }
    }

    private IEnumerator<float> InstanitnatePopUp()
    {
        yield return Timing.WaitForSeconds(0);
        Timing.RunCoroutine(_gameController.HidePogressBar());
        achiveSound.Play();
        Instantiate(Achive, new Vector3(camera.transform.position.x, camera.transform.position.y + 4.7f, 6), Quaternion.identity, GameObject.Find("UI").transform);
        Timing.RunCoroutine(InstanitnatePopUpTime());
        yield break;
    }

    private IEnumerator<float> InstantinateSuperPower()
    {
        yield return Timing.WaitForSeconds(0.37f);
        if (!_gameController._death)
            CalculateSuperPower();
        yield break;
    }

    private IEnumerator<float> CheckLuckyJump()
    {
        
        if (!_gameController.menu)
        {
            //Debug.Log("In");
            yield return Timing.WaitForSeconds(0.2f);
            var LJ = Instantiate(gameObject.GetComponent<Border>().LuckyJumpEffect, gameObject.transform.position, gameObject.transform.rotation);
            LJ.GetComponent<LuckyJumpAnim>().figureAngles = _angels;
            _playerClass.isLuckyJump = true;
        }
        yield break;
    }

    private IEnumerator<float> InstanitnatePopUpTime()
    {
        yield return Timing.WaitForSeconds(1.8f);
        _activeAchive = false;
        yield break;
    }

    private IEnumerator<float> InstatninateFigureAchive()
    {
        yield return Timing.WaitForSeconds(0.6f);
        Instantiate(figureAchiveComponent, _player.transform.position, gameObject.transform.rotation);     
        yield break;
    }

    private IEnumerator<float> SpawnAnim()
    {
        if (IsFirst)
            _animation.Play("Spawn1");
        else
            _animation.Play("Spawn");
        yield return Timing.WaitForSeconds(1f);
        
            _animation.Play("Blick");
        yield break;
    }

    
    public IEnumerator<float> Destroy()
    {

        IsDestroing = true;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        var colloders = gameObject.GetComponents<PolygonCollider2D>();
        colloders[0].enabled = false;
        colloders[1].enabled = false;
        yield return Timing.WaitForSeconds(1.5f);
        
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null && !_gameController.firstFigure)
        {
            Die(collision);
        }
    }

    private void Die(Collision2D collision)
    {
        _playerClass.LuckyJump.SetActive(false);
        _gameController._currentBorder = gameObject;
        if (IsDestroing)
            transform.GetChild(0).gameObject.SetActive(true);
        var position = collision.gameObject.transform.position;
        var spawnPos = new Vector3((position.x + _oldPlayerPos.x) / 2, (position.y + _oldPlayerPos.y) / 2, position.z);
        _player.GetComponent<Player>()._clicableZone.gameObject.SetActive(false);
        if (!_gameController._soundMute)
            _badSound.Play();
        _playerAlive = false;
        Triger.isTrigger = false;
        Instantiate(_playerParts, spawnPos, collision.gameObject.transform.rotation);
        if (!_gameController._vibrMute)
            _vibrator.PlayVibration();
        collision.gameObject.SetActive(false);
        _gameController._death = true;
        _gameController.dead = true;
        //if (!reviveShown)
        //{
            Timing.RunCoroutine(_gameController.ReviveGame());
        //    reviveShown = true;
        //}
        
        _gameController._boardToDestroy = gameObject;
        gameObject.transform.GetChild(1).transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        _gameController.PauseMusic();
        analitcs.OnDeathEvent(_angels, _gameController._currentScore);
        _gameController.InvokeDeath();
    }
}
