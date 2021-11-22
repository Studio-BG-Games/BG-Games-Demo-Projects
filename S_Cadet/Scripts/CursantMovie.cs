
using UnityEngine;

public class CursantMovie : MonoBehaviour
{

   public Animator m_Animator;
   [SerializeField] private AudioSource _steps;

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }


    public void StopSteps() {
        _steps.Stop();
    }
    
    public void StartSteps() {
        _steps.Play();
    }

}
