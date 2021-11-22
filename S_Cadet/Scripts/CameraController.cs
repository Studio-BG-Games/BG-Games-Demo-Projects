using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CharacterController _myPownBody;
    [SerializeField] private float _moveY, _moveX;

    public float SensX, SensY;

    public Vector2 MinMax_Y;
    public Vector2 MinMax_X;

    public bool isStart;
    public bool isEndStrike;

    private void Start()
    {
        Cursor.visible = false;
        _myPownBody = this.GetComponent<CharacterController>();
    
    }

    static float ClampAngle(float angle, float min, float max) {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max );
    }


   public void IsEndStrike() {
        Cursor.visible = true;
        isEndStrike = true;
    }   
    
    public void IsStartStrike() {
        isStart = true;
    }


    private void Update()
    {

    
        if (_myPownBody != null && !isEndStrike && isStart) {
            if (Input.GetAxis("Mouse Y") != 0) {
                _moveY -= Input.GetAxis("Mouse Y") * SensY;
            } else
            {
                _moveY -= Input.GetAxis("Vertical");
            }
            _moveY = ClampAngle(_moveY, MinMax_Y.x, MinMax_Y.y);

          
            if (Input.GetAxis("Mouse X") != 0) {
                _moveX += Input.GetAxis("Mouse X") * SensX;
            } else {
                _moveX += Input.GetAxis("Horizontal");
            }
            _moveX = ClampAngle(_moveX, MinMax_X.x, MinMax_X.y);
           
            _myPownBody.transform.localRotation = Quaternion.Euler(_moveY, _moveX, 0);
        } 


        if (isEndStrike) _myPownBody.transform.localRotation = Quaternion.Slerp(_myPownBody.transform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime);

    }
}
