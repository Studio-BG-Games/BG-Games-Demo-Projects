using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;
using GoogleMobileAds.Api;
using GoogleMobileAds;

//using UnityEngine.iOS;


public class GameController : MonoBehaviour
{
    #region fields
    public static GameController gameController;
    [SerializeField] private GameObject _startUI;
    [SerializeField] public GameObject _player;
    [SerializeField] private GameObject _camera;
    [HideInInspector] private CameraController _cameraController;
    [SerializeField] public GameObject _border;
    [SerializeField] public GameObject onDeathBorder;
    [SerializeField] private GameObject _restartGame;
    [SerializeField] private Text _recordScore;
    [SerializeField] private Text smallRecordScore;
    [SerializeField] private Text highScore;
    [SerializeField] public GameObject _boardToDestroy;
    [SerializeField] public GameObject HeatWave;
    [SerializeField] private GameObject _ray;
    [SerializeField] private GameObject _backGround;
    [SerializeField] private GameObject restartLeaders;
    [SerializeField] private GameObject Achive;
    [SerializeField] private GameObject leaders;
    [SerializeField] private GameObject noAds;
    [SerializeField] public GameObject LuckyJumpScore;
    [SerializeField] private GameObject bannerObject;
    [HideInInspector] public GameObject _currentBorder;

    //buttons
    [SerializeField] private Button _restart;
    [SerializeField] private Button _vibr;
    [SerializeField] private Button _sound;
    [SerializeField] private Button _music;
    [SerializeField] private Button _clicableZone;
    //skins
    [SerializeField] private GameObject[] _vibrSkins;
    [SerializeField] private GameObject[] _soundSkins;
    [SerializeField] private GameObject[] _musicSkins;
    [SerializeField] public Image[] _backGrounds;
    
    [SerializeField] public Text _scoreText;
    [SerializeField] public int _currentScore;
    [SerializeField] public int _record;
    [SerializeField] public int onDeathBorderAngles;
    [HideInInspector] public int scoreMuliplier;

    //sound bool
    [SerializeField] public bool _vibrMute;
    [SerializeField] public bool _soundMute;
    [SerializeField] public bool _musicMute;
    [SerializeField] public bool _musicPlay;
    [SerializeField] public bool _breakerDirection;
    [HideInInspector] public bool _death = false;
    [HideInInspector] public bool dead ;
    [HideInInspector] private bool _recordbeated = false;
    [SerializeField] public bool _waitForFirstFigure;
    [HideInInspector] private bool firstGame;
    [SerializeField] public bool canDestroyWindow;
    [SerializeField] public bool NoAdsBought;
    [SerializeField] public AudioSource _musicSound;
    [SerializeField] private AudioSource recordSound;
    [HideInInspector] private Player playerClass;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject ReviveObject;
    [SerializeField] private GameObject RateObject;
    [HideInInspector] private ProgeressBar progressBarClass;
    [SerializeField] private RectTransform _clicableTransform;
    [SerializeField] private float timeScale;
    [SerializeField] private float fixedDeltaTime;
    [SerializeField] public bool changeBackground;
    //[SerializeField] private LayerMask layerMask;
    [HideInInspector] public bool failetoLoadRewardedAd;
    private Vector3 _rayPosition;
    private int _frameRate = 60;
    private int bitmask = 1<<9;
    public bool firstFigure = true;
    [HideInInspector] public bool menu;

    //sound prefs
    private int _vibrPref;
    private int _soundPref;
    private int _musicPref;
    private bool  reviveShown;
    public InterAdd interAdd;
    public Banner banner;
    public PlayGames leaderboard;
    public event Action OnDeath;
    public StateMachine stateMachine;
    public State superPowerState;
    public State normalState;
    public bool isSuperPowerState;

    #endregion

    #region UnityCallbacks
    private void Awake()
    {
        gameController = this;
        //PlayerPrefs.DeleteAll();
        MobileAds.Initialize(initStatus => { });
        bannerObject = GameObject.Find("Banner");
        banner = bannerObject.GetComponent<Banner>();
        ShowBanner();
        SetOptimization();
        playerClass = _player.GetComponent<Player>();
        SetFirstFigure();
        HeatWave = GameObject.Find("Detonator-Heatwave(Clone)");
        var borderSpawner = Instantiate(onDeathBorder, new Vector3(0, 0, 10), Quaternion.identity);
        borderSpawner.GetComponent<Border>().IsFirst = true;
        _currentBorder = borderSpawner;
        if(PlayGames.playGamesInstance!=null)
            leaderboard = PlayGames.playGamesInstance;
        stateMachine = new StateMachine();
        superPowerState = new SuperPowerState(_player, stateMachine, this);
        normalState = new NormalState(_player, stateMachine, this);
        stateMachine.Initialize(normalState);
        SoundCheck();
    }

    private void Start()
    {
        _cameraController = _camera.GetComponent<CameraController>();
        _restart.onClick.AddListener(StartRestart);
        _record = PlayerPrefs.GetInt("_record");
        _recordScore.text = _record.ToString();
        scoreMuliplier = 1;
        menu = true;
        progressBarClass = progressBar.GetComponent<ProgeressBar>();
        _player.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        Timing.RunCoroutine(WaitForFirstFigure());
        if (_record == 0)
            firstGame = true;
        if (PlayerPrefs.GetInt("NoAddsBougth") == 1)
            GameObject.Find("NoAds").SetActive(false);
    }

    private void Update()
    {
        if (_frameRate != Application.targetFrameRate)
    			Application.targetFrameRate = _frameRate;
        
    }
    #endregion

    private void SetFirstFigure()
    {
        try {
            onDeathBorderAngles = PlayerPrefs.GetInt("OnDeathBorderAngles");
            onDeathBorder = playerClass._borders[onDeathBorderAngles - 3];
            playerClass.ChangeBackground(0, onDeathBorderAngles-3);
            changeBackground = true;
        }
        catch (Exception)
        {
            onDeathBorder = _border;
        }
    }
    
    private void SoundCheck()
    {
        _vibrPref = PlayerPrefs.GetInt("_vibrPref");
        _soundPref = PlayerPrefs.GetInt("_soundPref");
        _musicPref = PlayerPrefs.GetInt("_musicPref");
        if (_vibrPref == 1)
        {
            _vibrMute = false;
            VibrMute();
        }
        else
        {
            _vibrMute = true;
            VibrMute();
        }
        if (_soundPref == 1)
        {
            _soundMute = false;
            SoundMute();
        }
        else
        {
            _soundMute = true;
            SoundMute();
        }
        if (_musicPref == 1)
        {
            _musicMute = false;
            MusicMute();
        }
        else
        {
            _musicMute = true;
            MusicMute();
        }
        _sound.onClick.AddListener(SoundMute);
        _vibr.onClick.AddListener(VibrMute);
        _music.onClick.AddListener(MusicMute);
    }

    private void SetOptimization()
    {
        QualitySettings.vSyncCount = 0;
    }
    
    public  void ClickZone()
    {            
        _breakerDirection = _ray.GetComponent<Ray>()._breakAvaible;
        _currentBorder.transform.GetChild(1).transform.GetChild(3).gameObject.SetActive(false);

        if (  _player.GetComponent<Player>()._isWaite == false  && _waitForFirstFigure)
        {
            CheckSuperpowerHit();
            stateMachine.CurrentState.clickZone();
            StartCoroutine(nameof(Destroywindow));
            _player.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            _cameraController.FirstFigure = false;
            _clicableTransform.sizeDelta = new Vector2(400f, 720.86f);
            //Timing.RunCoroutine(WaitForFirstFigure());
            _waitForFirstFigure = false;
            _breakerDirection = false;
            Break();
                       
        }
        else if (  playerClass._isWaite == false && _waitForFirstFigure)
        {
        _player.gameObject.GetComponent<PolygonCollider2D>().enabled = true;

            _cameraController.FirstFigure = false;
            //Timing.RunCoroutine(WaitForFirstFigure());
            Break();
        }
    }

    private void CheckSuperpowerHit()
    {
        var hit = Physics2D.Raycast(_player.transform.position, _player.transform.right, 10, bitmask);
        if (hit.collider != null)
        {
            if (hit.collider.name == "SuperPowerObject")
            {
                Debug.Log("Do superpower");
                stateMachine.ChangeState(superPowerState);
            }
        }
    }

    public void ChangeProgressbar()
    {
        try
        {
            progressBarClass.ChangeProgress();
        }
        catch (Exception)
        {

        }
    }

    public void Break()
    {
       //Debug.Break();
        if (!_musicPlay && !_musicMute)
        {
            _musicPlay = true;
            _musicSound.gameObject.SetActive(true);
            _musicSound.Play();
        }
        _startUI.gameObject.SetActive(false);
        _scoreText.gameObject.SetActive(true);
        _recordScore.gameObject.SetActive(false);
        leaders.gameObject.SetActive(false);
        noAds.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(true);
        _cameraController.enabled = true;
        try
        {
            if (LuckyJumpAnim.animInstanse)
                LuckyJumpAnim.animInstanse.GetComponent<LuckyJumpAnim>().increase = true;
        }
        catch (Exception) { }
       // playerClass.CustomStart();
        playerClass.MenuScreen = false;
        playerClass.enabled = true;
        playerClass._isMoving = true;
        if (changeBackground)
        {
            try
            {
                playerClass.ChangeBackground(onDeathBorderAngles - 3, 0);
                changeBackground = false;
            }
            catch(Exception)
            {
                changeBackground = false;
            }
        }
    }

    public void RewriteScore()
    {
        _currentScore += 10 * scoreMuliplier;
        _scoreText.text = _currentScore.ToString();
        playerClass.CheckLuckyJump();
        CheckRecord();
    }

    private void CheckRecord()
    {
        if (firstGame)
        {
            _record += 10;
            _recordbeated = true;
            PlayerPrefs.SetInt("_record", _record);
        }
        if (_currentScore <= _record)
            return;
        if (!_recordbeated)
        {
            //recordSound.Play();
            StartCoroutine(nameof(RecordBeat));
            //StartCoroutine(nameof(DoSlowMotion));
            Timing.RunCoroutine(ResizeScore(_scoreText,0.30f));
        }
        _record = _currentScore;
        PlayerPrefs.SetInt("_record", _record);
    }

    public void VibrMute()
    {
        _vibrMute = !_vibrMute;
        switch (_vibrMute)
        {
            case true:
                PlayerPrefs.SetInt("_vibrPref", 1);
                _vibrSkins[0].SetActive(true);
                _vibrSkins[1].SetActive(false);
                break;
            case false:
                PlayerPrefs.SetInt("_vibrPref", 0);
                _vibrSkins[1].SetActive(true);
                _vibrSkins[0].SetActive(false);
                break;
        }
    }

    private void SoundMute()
    {
        _soundMute = !_soundMute;
        switch (_soundMute)
        {
            case true:
                PlayerPrefs.SetInt("_soundPref", 1);
                _soundSkins[0].SetActive(true);
                _soundSkins[1].SetActive(false);
                break;
            case false:
                PlayerPrefs.SetInt("_soundPref", 0);
                _soundSkins[1].SetActive(true);
                _soundSkins[0].SetActive(false);
                break;
        }
    }

    private void MusicMute()
    {
        _musicMute = !_musicMute;
        switch (_musicMute)
        {
            case true:
                PlayerPrefs.SetInt("_musicPref", 1);
                _musicSound.mute = true;
                _musicSkins[0].SetActive(true);
                _musicSkins[1].SetActive(false);
                break;
            case false:
                PlayerPrefs.SetInt("_musicPref", 0);
                _musicSound.mute = false;
                _musicSkins[0].SetActive(false);
                _musicSkins[1].SetActive(true);
                break;
        }
    }

    public void ShowOnDeathAdd()
    {
        if (PlayerPrefs.GetInt("deathCount") >= 3  && PlayerPrefs.GetInt("NoAddsBougth") != 1)
        {

            PlayerPrefs.SetInt("deathCount", 0);
            ShowAdd();
        }
    }

    public void InvokeDeath()
    {
        Debug.Log(SuperPower.SuperPowerInstance);
        OnDeath?.Invoke();
        if (SuperPower.SuperPowerInstance != null)
        {
            Destroy(SuperPower.SuperPowerInstance);
            Debug.Log("InDestroy");
        }
    }

    private  void StartRestart()
     {
            Destroy(interAdd);
            SceneManager.LoadScene(0);

     }

    public void ShowAdd()
    {
        PauseMusic();        
        interAdd.ShowAd();
    }

    private void ShowBanner()
    {
        if (PlayerPrefs.GetInt("First2Games") == 1 && PlayerPrefs.GetInt("NoAddsBougth") !=1)
        {
            if (!banner.isLoaded)
            {
                banner.RequestBunnerAd();

            }
        }

    }

    public void PauseMusic()
    {
        Timing.PauseCoroutines("music");
        _musicSound.Pause();
    }

    public void ResumeMusic()
    {
        Timing.ResumeCoroutines("music");
        _musicSound.Play();
    }

    public void ContinueGame()
    {
        _currentBorder.SetActive(true);
        _currentBorder.gameObject.GetComponent<Border>().reviveShown = false;
        _player.SetActive(true);
        playerClass._isMoving = false;
        Timing.RunCoroutine(playerClass.Rotate());
        //_player.gameObject.GetComponent<Player>().enabled = false;
        _player.transform.position = _currentBorder.transform.position;
        _player.transform.rotation = _currentBorder.transform.rotation;
        _player.GetComponent<Player>()._clicableZone.gameObject.SetActive(true);
        _currentBorder.gameObject.GetComponent<Border>()._playerAlive = true;
        _currentBorder.gameObject.GetComponent<Border>().Triger.isTrigger = true;
        _death = false;
        dead = false ;
        _boardToDestroy = _currentBorder;
        _musicSound.gameObject.SetActive(true);
        _musicSound.Play();
        _currentBorder.transform.GetChild(1).transform.GetChild(3).gameObject.SetActive(false);
        Timing.RunCoroutine(WaitForFirstFigure());
        ResumeMusic();
    }


    public void CheckOnDeathAd()
    {
        PlayerPrefs.SetInt("deathCount", PlayerPrefs.GetInt("deathCount") + 1);
        if (PlayerPrefs.GetInt("ToRateCount") <= 12)
            PlayerPrefs.SetInt("ToRateCount", PlayerPrefs.GetInt("ToRateCount") + 1);
        if (PlayerPrefs.GetInt("deathCount") == 2)
            PlayerPrefs.SetInt("First2Games", 1);

        ShowOnDeathAdd();
    }

    public void NoAddsBougth()
    {
        PlayerPrefs.SetInt("NoAddsBougth", 1);
        GameObject.Find("NoAds").SetActive(false);
        try
        {
            banner.HideBanner();
        }
        catch(Exception)
        {

        }
    }

    private IEnumerator RecordBeat()
    {
        yield return new WaitForSecondsRealtime(1f);
        _recordbeated = true;
    }

    private IEnumerator<float> ResizeScore(Text resizeText, float time)
    {
        int a = resizeText.fontSize;
        int i = resizeText.fontSize+ Convert.ToInt32(resizeText.fontSize * 0.3);
        resizeText.fontSize = i;
        yield return Timing.WaitForSeconds(time);
        _scoreText.fontSize = a;
    }

    private IEnumerator Destroywindow()
    {
        canDestroyWindow = true;
        var  border = _currentBorder;
        _currentBorder.transform.GetChild(0).gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(0.2f);
        if (border.gameObject.GetComponent<Border>().borderDestring && !dead)
        {
            border.gameObject.GetComponent<Border>().DestroyWindow();
        }
    }

    public IEnumerator<float> RestartGame()
     {

        CheckRate();
        Animation animation = _recordScore.GetComponent<Animation>();
        PlayerPrefs.SetInt("RecordBit",1);
        _death = false;
        _clicableZone.gameObject.SetActive(false);
        yield return Timing.WaitForOneFrame;
	    restartLeaders.gameObject.SetActive(true);
        _scoreText.gameObject.SetActive(false);
        progressBar.SetActive(false);
        _recordScore.gameObject.SetActive(true);
        _recordScore.text=_currentScore.ToString();
        smallRecordScore.gameObject.SetActive(true);
        smallRecordScore.text="High Score: "+_record;
        _restartGame.gameObject.SetActive(true);
        Destroy(_boardToDestroy);
        if (_recordbeated)
        {
            leaderboard.AddScoreToLeaderboard(_record);
            highScore.text = "New High Score";
            PlayerPrefs.SetInt("OnDeathBorderAngles", _currentBorder.GetComponent<Border>()._angels);
            animation.Play("RecordB");
        }
        else
        {
            highScore.text = "Score";
            animation.Play("RecordA");
        }
        yield break;
     }

    public IEnumerator<float> ReviveGame()
    {
            interAdd.LoadOnDeathAd();
        interAdd.RequestRewardedVideo();
        yield return Timing.WaitForSeconds(2);
        
        //if (!reviveShown)
        //{
        Instantiate(ReviveObject,GameObject.Find("UI").transform);
        reviveShown = true;
        try
        {
            _currentBorder.SetActive(false);
        }
        catch (Exception)
        {
            //Debug.Log("exeption");
        }
        //}
       // else
        //    Timing.RunCoroutine(RestartGame());
    }

    private void CheckRate()
    {
        if (PlayerPrefs.GetInt("ToRateCount") == 10 && Convert.ToInt32(PlayerPrefs.GetString("unity.player_session_count")) > 3)
            Instantiate(RateObject, GameObject.Find("UI").transform);
            //Device.RequestStoreReview();
    }

    public IEnumerator DoSlowMotion()
    {
        yield return new WaitForFixedUpdate();
        Time.timeScale = timeScale;
        recordSound.gameObject.GetComponent<Animation>().Play("SoundMute");
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        yield break;
    }

    public IEnumerator<float> HidePogressBar()
    {
        yield return Timing.WaitForSeconds(0.17f);
        progressBar.gameObject.SetActive(false);
        _scoreText.gameObject.SetActive(false);
        yield return Timing.WaitForSeconds(1.5f);        
        _scoreText.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(true);
        yield break;
    }

    public IEnumerator<float> WaitForFirstFigure()
    {
        _waitForFirstFigure = false;
        yield return Timing.WaitForSeconds(0.3f);
        _waitForFirstFigure = true;
        yield break;    
    }

    void OnApplicationFocus(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

}

