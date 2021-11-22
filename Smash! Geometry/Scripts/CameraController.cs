using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 offset = new Vector2(2f, 1f);
    [SerializeField] public float _size;
    [SerializeField] public float speed = 1.5f;
    [SerializeField] public bool isLeft;
    [SerializeField] private Transform player;
    [SerializeField] private int lastX;
    [SerializeField] private GameObject _cameraPosition;
    private Camera _cameraComponent;
    private GameController gameController;
    public bool FirstFigure;

    private void Awake()
    {
        FirstFigure = true;
    }
    private void Start()
    {
        
        _cameraComponent = gameObject.GetComponent<Camera>();
        _size = gameObject.GetComponent<Camera>().orthographicSize;
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FindPlayer(isLeft);
        gameController = GameObject.Find("EventSystem").gameObject.GetComponent<GameController>();
    }

    private void FindPlayer(bool playerIsLeft)
    {
        /*if (!FirstFigure)
        {*/
            player = GameObject.Find("Player").transform;
            var position = player.position;
            lastX = Mathf.RoundToInt(position.x);
            if (playerIsLeft)
            {
                var cameraTransform = transform;
                cameraTransform.position = new Vector3(position.x - offset.x, position.y - offset.y, cameraTransform.position.z);
            }
            else
            {
                var cameraTransform = transform;
                cameraTransform.position = new Vector3(position.x + offset.x, position.y + offset.y, cameraTransform.position.z);
            }
        
    }
    public void Update()
    {
        if (!FirstFigure)
        {
            _cameraComponent.orthographicSize = _size;
            if (!player) return;
            var currentX = Mathf.RoundToInt(player.position.x);
            if (currentX > lastX) isLeft = false; else if (currentX < lastX) isLeft = true;
            lastX = Mathf.RoundToInt(player.position.x);

            var position = transform.position;
            var target = new Vector3(offset.x, offset.y, position.z);
            Vector3 currentPosition = Vector3.Lerp(position, target, speed * Time.deltaTime);
            position = currentPosition;
            transform.position = position;
        }
    }
}
