using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [Header("Характеристики")]
    [SerializeField] private float _moveSpeed;          //Скорость движения
    [SerializeField] private float _moveClimping;       //Скорость поднятия по лестнице
    [SerializeField] private float _moveDescent;        //Скорость поднятия по лестнице
    [SerializeField] private float _moveFlight;         //Скорость полёта на шарике
    [SerializeField] private float _jumpForce;          //Сила прыжка 

    [Header("Компоненты")]
    public Flashlight Flashlight;
    [SerializeField] private LayerMask _layerSolid;
    [SerializeField] private Transform _transformCheckGround;
    [SerializeField] private AudioSource _audioStep;
    [SerializeField] private AudioClip _audioStepClip;

    private bool _isGrounded = false;                  //Стоит ли на земле?
    private float _timeReloadJump = 0.2f;              //Время перарядки прыжка
    private float _timeFall;                           //Время падения
    private bool _jumpLoaded = true;                   //Прыжок заряжен
    private Vector3 _lastPosition;                     //Последняя позиция игрока

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private CapsuleCollider2D _collider;
    private Transform _transform;
    private PlayerAnimStates _animStates;


    [HideInInspector] public Vector3 Velocity;                          //Скорость игрока
    [HideInInspector] public int CountJumps { private set; get; }
    [HideInInspector] public bool InClimpbingZone;
    [HideInInspector] public bool IsClipmbing;
    [HideInInspector] public bool IsFlightToBalloon;

    public enum DirectionMove
    {
        right = 1,
        left = -1
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponent<CapsuleCollider2D>();
        _transform = GetComponent<Transform>();
        _animStates = GetComponent<PlayerAnimStates>();
    }

    private void FixedUpdate()
    {
        Velocity = (transform.position - _lastPosition) / Time.fixedDeltaTime;

        CheckGround();
        if(InClimpbingZone)
        {
            MoveClimpbing();
        }
        if (IsFlightToBalloon)
        {
            MoveBalloon();
        }
        else
        {
            if (_isGrounded || Mathf.Abs(Velocity.y) < 0.1)
            {
                _animStates.State = PlayerAnimStates.States.idle;
                _animStates.State = PlayerAnimStates.States.idle;
            }
            else if (Velocity.y > 0.1)
            {
                _animStates.State = PlayerAnimStates.States.jump;
            }
            else if (Velocity.y < -0.1)
            {
                _animStates.State = PlayerAnimStates.States.fall;
            }
        }

        //Начисление время падения
        if (_animStates.State == PlayerAnimStates.States.fall)
            _timeFall += Time.fixedDeltaTime;
        else
            _timeFall = 0.0f;

        _lastPosition = transform.position;
    }



    public void Run(DirectionMove direction)
    {
        if (_isGrounded)
        {
            _animStates.State = PlayerAnimStates.States.move;
        }

        Vector3 dir = Vector3.right * (int)direction;

        Rotate(direction);

        _transform.position = Vector3.Lerp(_transform.position, _transform.position + dir, _moveSpeed * Time.deltaTime);
    }

    private void MakeStep()
    {
        _audioStep.pitch = Random.Range(0.8f, 1f);
        _audioStep.PlayOneShot(_audioStepClip);
    }

    public void AccelerationMove(Vector3 acceleration)
    {
        Rotate(acceleration.x > 0f ? DirectionMove.right : DirectionMove.left);
        _transform.position = Vector3.Lerp(_transform.position, _transform.position + acceleration, _moveSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        if (_isGrounded && _jumpLoaded)
        {
            _animStates.State = PlayerAnimStates.States.jump;
            _rb.velocity = new Vector2(0, _jumpForce);
            StartCoroutine("ReloadJump");
            CountJumps += 1;
        }
    }

    public void MoveClimpbing()
    {
        Vector3 dir = Vector3.up * (IsClipmbing ? 1 : -1);
        _transform.position = Vector3.Lerp(_transform.position, _transform.position + dir, (IsClipmbing ? _moveClimping : _moveDescent) * Time.deltaTime);
    }

    public void MoveBalloon()
    {
        _transform.position = Vector3.Lerp(_transform.position, _transform.position + Vector3.up, _moveFlight* Time.deltaTime);
        _animStates.State = PlayerAnimStates.States.flight; 
    }

    public void Rotate(DirectionMove direction)
    {
        if (direction == DirectionMove.left)
        {
            _transform.rotation = Quaternion.AngleAxis(180.0f, _transform.up);
        }
        else
        {
            _transform.rotation = Quaternion.AngleAxis(0, _transform.up);
        }
    }

    public void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_transformCheckGround.position, new Vector3(0.9f, 0.6f, 1.0f), 0f, _layerSolid);
        _isGrounded = colliders.Length > 0;
    }

    public void OnEnterClimpingZone()
    {
        InClimpbingZone = true;
        _rb.velocity = new Vector2(0, 0);
        _rb.gravityScale = 0;
    }
    public void OnExitClimpingZone()
    {
        InClimpbingZone = false;
        _rb.gravityScale = 5;
    }


    public void OnStartBallonMove()
    {
        IsFlightToBalloon = true;
        _rb.velocity = new Vector2(0, 0);
        _rb.gravityScale = 0;
    }
    public void OnEndBallonMove()
    {
        IsFlightToBalloon = false;
        _rb.gravityScale = 5;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_transformCheckGround.position, new Vector3(0.9f, 0.6f, 1.0f));
    }

    IEnumerator ReloadJump()
    {
        _jumpLoaded = false;
        yield return new WaitForSeconds(_timeReloadJump);
        _jumpLoaded = true;
    }
}
