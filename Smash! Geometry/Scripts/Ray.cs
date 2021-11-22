using UnityEngine;

public class Ray : MonoBehaviour
{
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject directionObject;
    [SerializeField] private GameObject player;

    public bool _breakAvaible;
    private void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
    }

    private void FixedUpdate()
    {
        /*var position = directionObject.transform.position;
        transform.position = position;
        transform.rotation = player.transform.rotation;*/
    }
    private void OnTriggerEnter2D(Collider2D borderCollision2D)
    {
        if (borderCollision2D.GetComponent<Transform>().name == "Breaker")
        {
            _breakAvaible = true;
        }
        else
        {
            if(borderCollision2D.GetComponent<Glass>()==null)
                _breakAvaible = false;
        }
    }
    
}