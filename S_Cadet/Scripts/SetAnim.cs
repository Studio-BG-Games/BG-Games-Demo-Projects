
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetAnim : MonoBehaviour
{

    public CursantMovie Cursant;
    public Animator Dorohin;
    [SerializeField] private Text _txt;
    [SerializeField] private GameObject _canv;


    [Space]
    public AudioSource Audio_A;
    public AudioClip GetBiletAudio;




    public void PlayAudio(AudioClip music)
    {
        Audio_A.PlayOneShot(music);

    }

    public void Salut()
    {
        Cursant.m_Animator.SetTrigger("Salute");
        Dorohin.SetTrigger("Salute");
        Cursant.StopSteps();
    }

    public void Stay()
    {
        Cursant.m_Animator.SetTrigger("Stay");
        Cursant.StopSteps();
    }

    public void StandUp()
    {
        Cursant.m_Animator.SetTrigger("StandUp");
        Cursant.StopSteps();
    }

    public void SitDown()
    {
        Cursant.m_Animator.SetTrigger("SitDown");
        Cursant.StopSteps();
    }

    public void Walk()
    {
        Cursant.m_Animator.SetTrigger("Walk");
        Cursant.StartSteps();
    }    
    
    public void SceneBilet()
    {
       
        _canv.SetActive(true);
        _txt.text = "\n Товарищ подполковник, курсант " + PlayerPrefs.GetString("lastName") + "  для сдачи экзамена по огневой подготовке прибыл!";
        Time.timeScale = 0;
    }    
    
    public void SceneBiletOff()
    {
        _canv.SetActive(false);
        Time.timeScale = 1;
        PlayAudio(GetBiletAudio);
    }

    public void LoadSceneBilet() {
        SceneManager.LoadScene("Bilet");
    }

    public void LoadSceneStrike() {
        SceneManager.LoadScene("Strike");
    }    
    
    public void LipDorohin() {
        Dorohin.SetBool("Talk", true);
    }   
    
    public void LipDorohinOff() {
        Dorohin.SetBool("Talk", false);
    }


}
