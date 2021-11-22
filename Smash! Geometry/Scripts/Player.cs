using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;


public class Player : MonoBehaviour
{
    [SerializeField] public GameObject _border;
    
    [SerializeField] public GameObject[] _borders;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _cameraPosition;
    [SerializeField] private ProgeressBar progressBar;
    
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private TrailRenderer[] trails;

    [SerializeField] public int _angels;
    [SerializeField] public int figureCount = 0;
    [SerializeField] public int figureIndex = 0;
    [SerializeField] private float _rotateAngel;
    [SerializeField] private float _speedRotate;
    [SerializeField] public float _moveSpeed;
    [SerializeField] public float _moveDinstace;
    private float progress = 0;
    public bool _isMoving;
    public bool _isWaite;

    [SerializeField] private float _cameraDistance;
    [SerializeField] public float _angel;
    [SerializeField] public int _index;
    [SerializeField] private float _pauseBetweenRotations;
    [SerializeField] private float _speed;
    [NonSerialized] public int[] varible = { -1, 1 };
    [NonSerialized] public float[] borderScale = {0,0.66f,0.55f,0.47f,0.41f,0.36f,0.32f,0.29f};
    [SerializeField] private GameObject _eventSystem;
    [SerializeField] private float trueSpeed;
    [SerializeField] private float variable;
    [SerializeField] private float playerJumpAngle;
    [SerializeField] private  Vector2 valocity;
    public GameObject[] _boarderSkis;
    private Rigidbody2D _PlayerRigidbody2D;
    public bool borderSpawning;
    public bool isLuckyJump;
    public bool IsOnStartRotation;
    public bool AnimateLuckJumpComlete;
    public bool MenuScreen = true;
    public float step;
    private CameraController _cameraController;
    private LuckyJump luckyJump;
    private GameController _gameController;
    [SerializeField] public Button _clicableZone;
    [SerializeField] private RectTransform _clicableTransform;
    [SerializeField] public GameObject LuckyJump;
    [SerializeField] public GameObject _backgrounds;

    public AnimationCurve difficulty;
    public int numberOfFigures = 9;
    public Sprite superPowerSkin;
    public Sprite normalSkin;


    private void Start()
    {
            numberOfFigures = _borders[0].GetComponent<Border>().NumberOfFigures;
            _PlayerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _trail = gameObject.GetComponent<TrailRenderer>();
            _cameraController = _camera.GetComponent<CameraController>();
            _PlayerRigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _index = Random.Range(0, 2);
            _camera = GameObject.Find("Main Camera");
            _cameraPosition = GameObject.Find("CameraPosition");
            _gameController = GameObject.Find("EventSystem").GetComponent<GameController>();
            _rotateAngel = 360 / _angels * varible[_index];
            _clicableZone.onClick.AddListener(Clicablezone);
            luckyJump = GetComponent<LuckyJump>();
            _gameController.OnDeath += OnDeath;
        
    }

    public void CustomStart()
    {
        if (!MenuScreen)
        {
            numberOfFigures = _borders[0].GetComponent<Border>().NumberOfFigures;
            _PlayerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _trail = gameObject.GetComponent<TrailRenderer>();
            _cameraController = _camera.GetComponent<CameraController>();
            _PlayerRigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _index = Random.Range(0, 2);
            _camera = GameObject.Find("Main Camera");
            _cameraPosition = GameObject.Find("CameraPosition");
            _gameController = GameObject.Find("EventSystem").GetComponent<GameController>();
            _rotateAngel = 360 / _angels * varible[_index];
            _clicableZone.onClick.AddListener(Clicablezone);
            luckyJump = GetComponent<LuckyJump>();
        }
    }
    private void FixedUpdate()
    {
        
        if (_isWaite)
            _isMoving = false;
        
        if (_isMoving )
        {
            Move();
            _cameraController.speed = 6.5f;
        }
 
    }

    private void Clicablezone()
    {
        if (!_isWaite) 
            Timing.KillCoroutines("rotate"); 
        _border.GetComponent<Border>()._oldPlayerPos = gameObject.transform.position;
    }
    
    private void Move()
    {
        if (!borderSpawning)
        {
            _PlayerRigidbody2D.velocity = (transform.right * _moveDinstace) / variable;
            _cameraPosition.transform.position = transform.position;
            progress += _moveSpeed * trueSpeed;
            _cameraController.offset = transform.position;          
        }


        if (progress >= 1)
        {
            _gameController.menu = false;
            ChangeFigure();
            _isMoving = false;
            Timing.RunCoroutine(Waite());
            borderSpawning = true;   
            _PlayerRigidbody2D.velocity = Vector2.zero;
            progress = 0;
            _index = Random.Range(0, 2);
            Timing.RunCoroutine(SpawnBorder());
            Timing.KillCoroutines("rotate");

            _rotateAngel = 360 / _angels * varible[_index];

        }
        gameObject.SetActive(true);
    }


    private void ChangeFigure()
    {
        

        if (figureCount>= numberOfFigures && _gameController._currentScore!=10)
        {
            figureCount = 0;
            figureIndex++;
            numberOfFigures = _borders[figureIndex].gameObject.GetComponent<Border>().NumberOfFigures;
        }
        if (_gameController._currentScore == 10)
        {
            figureIndex = 0;
            numberOfFigures = _borders[0].GetComponent<Border>().NumberOfFigures;
        }
        if (figureIndex > 7)
            figureIndex = 7;
         _gameController.ChangeProgressbar();
        GameObject.Find("EventSystem").GetComponent<Analitics>().OnScreenChange(_angels);
         ChancgeGameParametrs(borderScale[figureIndex], figureIndex, _borders[figureIndex].gameObject.GetComponent<Border>().difficulty[figureCount].value);

       // Debug.Log("difficulty " +_borders[figureIndex].gameObject.GetComponent<Border>().difficulty[figureCount].value);
    }

    private void OnDeath()
    {
        progress = 0;
    }

    public void ChancgeGameParametrs(float breakedScale,int borderIndex,float pauseBetweenRotations)
    {
        figureCount++;
        _border = _borders[borderIndex];
        _gameController._border = _borders[borderIndex];
        _pauseBetweenRotations = pauseBetweenRotations;
        if (borderIndex != 0)
        {
            if(!_gameController.isSuperPowerState)
                _trail.colorGradient = trails[borderIndex].colorGradient;
            _border.GetComponent<Border>().scale = breakedScale;
            ChangeBackground(borderIndex - 1, borderIndex);
        }
    }

    public void SetTrailToSuperPower()
    {
        _trail.colorGradient = trails[9].colorGradient;
    }

    public void SetTrailToNormal()
    {
        _trail.colorGradient = trails[figureIndex].colorGradient;
    }


    public void ChangeBackground(int deactive, int active)
    {
        if (_backgrounds.transform.GetChild(deactive).GetChild(0).gameObject.activeInHierarchy)
            _backgrounds.transform.GetChild(active).GetChild(0).gameObject.SetActive(true);
        _backgrounds.transform.GetChild(deactive).gameObject.SetActive(false);
        _backgrounds.transform.GetChild(active).gameObject.SetActive(true);
        
    }

    public void CheckLuckyJump()
    {
        if (playerJumpAngle < 360 && isLuckyJump)
        {
            luckyJump.SetLuckyJump();
            
        }
        else
        {
            luckyJump.SetTimesToZero();
            
            isLuckyJump = false;
        }
        playerJumpAngle = 0f;
    }

    private void CheckLuckyJumpFail()
    {
        if (playerJumpAngle > (_angel * (360f / _angels) / 2 + (360f / (_angels * 4))))
        {
            if (LuckyJumpAnim.animInstanse)
            {
                if(!LuckyJumpAnim.animInstanse.GetComponent<LuckyJumpAnim>().IsMultyX)
                    Destroy(LuckyJumpAnim.animInstanse);
            }
            _gameController.stateMachine.ChangeState(_gameController.normalState);

            luckyJump.SetTimesToZero();
            isLuckyJump = false;
            
        }
    }

    private void DoRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + step);
        _cameraPosition.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + step);
        _cameraPosition.transform.Translate(Vector3.right * _cameraDistance, Space.Self);
        _cameraController.offset = _cameraPosition.transform.position;
        _cameraPosition.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        _cameraController.speed = 1;
    }

    

    public void SetBackgroundLight(bool a)
    {
        _backgrounds.transform.GetChild(figureIndex).GetChild(0).gameObject.SetActive(a);
        _backgrounds.transform.GetChild(figureIndex).GetComponent<MeshRenderer>().enabled = !a;
    }
    public IEnumerator<float> Rotate()
    {
        _cameraController.speed = _cameraSpeed;
        yield return Timing.WaitForSeconds(_pauseBetweenRotations );
        _rotateAngel = 360 / _angels * varible[_index];
        step = _rotateAngel / 5;
        IsOnStartRotation = true;
        for (int i = 0; i < 5; i++)
        {
            if (_isMoving)
            {
               // CheckLuckyJump();
                yield break;
            }
            yield return Timing.WaitForSeconds(0f);
            DoRotation();
            if (playerJumpAngle < 500)
            {
                playerJumpAngle += Math.Abs(step / 2);
                CheckLuckyJumpFail();
            }
        }

        if (_isMoving)
        {
            //CheckLuckyJump();
            yield break;
            
        }
        Timing.RunCoroutine(Rotate(), Segment.FixedUpdate, "rotate");
        yield break;
    }

    private IEnumerator<float> SpawnBorder()
    {
        _border.GetComponent<Border>()._oldPlayerPos = gameObject.transform.position;
        yield return Timing.WaitForSeconds(0.01f);
        var border = Instantiate(_border, new Vector3(0, 0.17f, 10), new Quaternion(0, 0, 0, 0));
        border.GetComponent<Border>()._rotateSide = varible[_index];
        _gameController._currentBorder = border;
        yield break;
    }

    private IEnumerator<float> Waite()
    {
        Timing.RunCoroutine(StartRotation());
        _isWaite = true;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        yield return Timing.WaitForSeconds(0.5f);
        _gameController._waitForFirstFigure = true;
        _gameController.firstFigure = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        borderSpawning = false;
        _isWaite = false;
        //Timing.RunCoroutine(Rotate(), Segment.FixedUpdate, "rotate");
        yield break;
    }

    private IEnumerator<float> StartRotation()
    {

        yield return Timing.WaitForSeconds(0.1f);
        Timing.RunCoroutine(Rotate(), Segment.FixedUpdate, "rotate");
        yield break;
    }



}