using UnityEngine;


public class VideoComponent : MonoBehaviour
{
    [SerializeField] private GameObject _buttoNEnd;
    [SerializeField] private GameObject _audio;
    [SerializeField] private GameObject _result;

    [SerializeField] private GameObject[] _videos;

    private int _resultBalls;

    public void PlayVideo()
    {
            _buttoNEnd.SetActive(false);
            _audio.SetActive(false);
            _result.SetActive(false);
            _videos[_resultBalls].SetActive(true);  
    }

    public void SetResultBalls(int i) {
        _resultBalls = i;
    }
}
