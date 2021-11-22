using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class PistoletMarker : MonoBehaviour
{
    public bool isZoom;
    [SerializeField] private int _buletCount;

    private int _score;
    private bool _isEndExamen;
    [SerializeField] private GameObject _buttonEnd;
    [SerializeField] private GameObject _flash;
    [SerializeField] private GameObject _buletTrack;
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _animDrag;

    [SerializeField] private AudioSource _sound;

    [SerializeField] private Animator _animPistol;
    [SerializeField] private CameraController _CharacterObserver;


    [Space]
    public AudioSource Audio_A;
    public AudioClip[] ResultAudio;
    public bool isResultPlaySound;



    public GameObject StrikeScoreField;
    public Text textStrikeScore;
    public void DisableFlach ()
    {
        _flash.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)) {

            Shooting();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            isZoom = true;
            ZoomPistolet();
        }
        else {
            isZoom = false;
            ReZoomPistolet();
        }

        if (_buletCount < 1 && !_isEndExamen) {

            _isEndExamen = true;
            PlayerPrefs.SetInt("strikeScore", _score);
            _animDrag.SetBool("IsEnd", true);
            _CharacterObserver.IsEndStrike();
            _anim.SetBool("IsEnd", true);

            if (!isResultPlaySound) {
                isResultPlaySound = true;
                StartCoroutine(ResultStrike());
            }
           
        }
    }


    
    private IEnumerator ResultStrike()
    {

        StrikeScoreField.SetActive(true);
        textStrikeScore.text = "" + $"Набрано очков {_score} из 30";

        if (_score >= 25)
        {
            PlayAudio(ResultAudio[3]);
        }

        if (_score <= 24 && _score >= 21)
        {
            PlayAudio(ResultAudio[2]);
        }

        if (_score <= 20 && _score >= 18)
        {
            PlayAudio(ResultAudio[1]);
        } 


        if (_score < 18 )
        {
            PlayAudio(ResultAudio[0]);
        }
        


        yield return IsEndPlaySound();
       
        _buttonEnd.SetActive(true);
    }

    private IEnumerator IsEndPlaySound()
    {
        while (Audio_A.isPlaying)
        {
            yield return null;
        }
    }

    public void PlayAudio(AudioClip music)
    {
        Audio_A.PlayOneShot(music);
    }


    public void StartMovePistolet()
    {
        _CharacterObserver.IsStartStrike();
    }

    private void ZoomPistolet() {
        _anim.SetBool("Zoom", true);
        _animDrag.SetBool("Slow", true);
    }    
    
    private void ReZoomPistolet() {
        _anim.SetBool("Zoom", false);
        _animDrag.SetBool("Slow", false);
    }

    private void Shooting() {
        if (_buletCount > 0) {
            _sound.Play();
            _flash.SetActive(true);
            _buletCount--;
           
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2 + 5, Screen.height / 2 -5));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.tag == "Enemy")
                {
                    _score += hit.collider.GetComponent<TargetScore>().scoreValue; 
                }
                _animPistol.SetTrigger("Recail");
                Instantiate(_buletTrack, hit.point + new Vector3(-0.1f, 0, 0), Quaternion.identity);
                
            }
        }
        Debug.Log(_score);
    }
   
}
