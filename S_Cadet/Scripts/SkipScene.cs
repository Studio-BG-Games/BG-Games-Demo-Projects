
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SkipScene : MonoBehaviour
{
    [SerializeField] private UnityEvent _gameObject;
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)  && Input.GetKey(KeyCode.F1))
        {
            SceneManager.LoadScene("StartVideo");
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)  && Input.GetKey(KeyCode.F2))
        {
            SceneManager.LoadScene("Start");
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)  && Input.GetKey(KeyCode.F3))
        {
            SceneManager.LoadScene("Game");
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)  && Input.GetKey(KeyCode.F4))
        {
            SceneManager.LoadScene("Bilet");
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)  && Input.GetKey(KeyCode.F5))
        {
            SceneManager.LoadScene("GoToStrike");
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)  && Input.GetKey(KeyCode.F6))
        {
            SceneManager.LoadScene("Strike");
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)  && Input.GetKey(KeyCode.F7))
        {
            SceneManager.LoadScene("TableResult");
        }        
        
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift)  && Input.GetKey(KeyCode.RightArrow))
        {
            _gameObject.Invoke();
        }

        
    }
}
