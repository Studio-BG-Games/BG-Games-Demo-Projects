using UnityEngine;

public class Bilet : MonoBehaviour
{
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private bool isMove = false;

    void Start()
    {
        endPosition =  new Vector3 (transform.position.x + 5, transform.position.y+1, transform.position.z+1);
    }


    private void Update()
    {
        if (isMove)
        {
             transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime/5);
           
            if (endPosition.x - transform.position.x < 4) {
                isMove = false;
            };
        }
    }

    public void MoveToHand() {
        isMove = true;
    }
}
