using UnityEngine;


public class StartVideo : MonoBehaviour
{

    [SerializeField] private GameObject _startButton;

    public void StartGame()
    {
        _startButton.SetActive(false);

    }


}
