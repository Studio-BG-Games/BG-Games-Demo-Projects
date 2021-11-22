using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BiletController : MonoBehaviour
{
    [SerializeField] private int _minBilet;
    [SerializeField] private int _maxBilet;
    public int biletNum;
    public int score;

    private bool _isTouched;
    public GameObject bilet;

    public LayerMask layerMask;

    public TextAsset textJSON;
    public GameObject msg;

    public Color _color;
    public Color _colorError;

    [Space]
    public AudioSource Audio_A;
    public AudioClip[] ResultAudio;
    public AudioClip StrikeAreayGo;
    [Space]

    public GameObject rightAnswerField;
    public Text textRightAnswer;



    /// <summary>
    [SerializeField] private Text _numBilet;
    [SerializeField] private Text _questions_1;
    [SerializeField] private Text[] _answer_1;
    [SerializeField] private Text _questions_2;
    [SerializeField] private Text[] _answer_2;
    [SerializeField] private Text _questions_3;
    [SerializeField] private Text[] _answer_3;    
    [SerializeField] private Text _questions_4;  //**
    [SerializeField] private Text[] _answer_4;  //**

    [SerializeField] private Dropdown Dropdown_question1;
    [SerializeField] private Dropdown Dropdown_question2;
    [SerializeField] private Dropdown Dropdown_question3;  
    [SerializeField] private Dropdown Dropdown_question4;  //**
    
    [SerializeField] private Text question1;
    [SerializeField] private Text question2;
    [SerializeField] private Text question3;
    [SerializeField] private Text question4; //**

    [SerializeField] private GameObject ButtonSend;
    [SerializeField] private GameObject ButtonEndGame;

    [SerializeField] private GameObject ButtonNExtLevel;

    [SerializeField] private Transform[] _variants;

    [SerializeField] private ButtonsArea _allButtons;
    /// </summary>
    [System.Serializable] public class Bilets
    {
        public int biletNumber;
        public Questions[] questions;
        public string[] rightAnswer;
    }   

    ///
    [System.Serializable] public class ButtonsArea
    {
        public Buttons[] _buttonsArea;
    }    
    
    [System.Serializable] public class Buttons
    {
        public Button[] _buttons;
    }
    /// 


    [System.Serializable] public class BiletsList
    {
        public Bilets[] bilets;
    }

    [System.Serializable] public class Questions
    {
        public string question;
        public string[] ansewer;
    }

    public BiletsList myBiletList = new BiletsList();

    void Start()
    {
        myBiletList = JsonUtility.FromJson<BiletsList>(textJSON.text);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !_isTouched)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 50, layerMask))
            {
                SetBilet(hit);
            }

        }
    }


    private void SetBilet(RaycastHit hit) {

        

        biletNum = Random.Range(_minBilet, _maxBilet + 1);
        foreach (var item in myBiletList.bilets)
        {
            if (item.biletNumber == biletNum)
            {
                _numBilet.text = "Билет " + item.biletNumber;
                _questions_1.text = "1. " + item.questions[0].question;
                _questions_2.text = "2. " + item.questions[1].question;
                _questions_3.text = "3. " + item.questions[2].question;
                _questions_4.text = "4. " + item.questions[3].question;

                for (int i = 0; i < item.questions[0].ansewer.Length; i++)
                {
                    _answer_1[i].text = "" + item.questions[0].ansewer[i];
                }
                for (int i = 0; i < item.questions[1].ansewer.Length; i++)
                {
                    _answer_2[i].text = "" + item.questions[1].ansewer[i];
                }
                for (int i = 0; i < item.questions[2].ansewer.Length; i++)
                {
                    _answer_3[i].text = "" + item.questions[2].ansewer[i];
                }
                for (int i = 0; i < item.questions[3].ansewer.Length; i++)
                {
                    _answer_4[i].text = "" + item.questions[3].ansewer[i];
                }
             
            }
        }

        Dropdown_question1.value = 0;
        Dropdown_question2.value = 0;
        Dropdown_question3.value = 0;
        Dropdown_question4.value = 0;

        hit.collider.gameObject.GetComponent<Bilet>().MoveToHand();
        bilet.SetActive(true);

        _isTouched = true;
    }

    [ContextMenu("ResetBilet")]
    public void ResetBilet() {
        _isTouched = false ;
        bilet.SetActive(false);
        score = 0;


        foreach (Transform select in _variants)
        {
            for (int i = 0; i < select.childCount; i++)
            {
                Destroy(select.GetChild(i).gameObject);
            }
        }
    }


    public void ButtonSelected(Button button)
    {
       // button.
        for (int i = 0; i< button.transform.parent.GetComponent<Transform>().childCount; i++) {
            ColorBlock cb = button.transform.parent.GetComponent<Transform>().GetChild(i).GetComponent<Button>().colors;
            cb.normalColor = Color.white; ;
            cb.selectedColor = Color.white; ;
            button.transform.parent.GetComponent<Transform>().GetChild(i).GetComponent<Button>().colors = cb;
        };

        ColorBlock cbm = button.GetComponent<Button>().colors;
        cbm.normalColor = _color;
        cbm.selectedColor = _color;
        button.GetComponent<Button>().colors = cbm;

        button.GetComponent<AnswerVariant>().dropdown.value = ReturnerValueQuestion(button.GetComponent<AnswerVariant>().value);
    }    
    
    
    public void ButtonSuccess(Button button)
    {
        ColorBlock cbm = button.GetComponent<Button>().colors;
        cbm.normalColor = _color;
        cbm.selectedColor = _color;
        button.GetComponent<Button>().colors = cbm;
        button.GetComponent<AnswerVariant>().dropdown.value = ReturnerValueQuestion(button.GetComponent<AnswerVariant>().value);
    }    
    
    public void ButtonError(Button button)
    {
        ColorBlock cbm = button.GetComponent<Button>().colors;
        cbm.normalColor = _colorError;
        cbm.selectedColor = _colorError;
        button.GetComponent<Button>().colors = cbm;
        button.GetComponent<AnswerVariant>().dropdown.value = ReturnerValueQuestion(button.GetComponent<AnswerVariant>().value);
    }

    public void FindRightAnswer(string rightAnswer, int numButtons) {
        switch (rightAnswer)
        {
            case "А":
                ButtonSuccess(_allButtons._buttonsArea[numButtons]._buttons[0]);
                break;
            case "Б":
                ButtonSuccess(_allButtons._buttonsArea[numButtons]._buttons[1]);
                break;
            case "В":
                ButtonSuccess(_allButtons._buttonsArea[numButtons]._buttons[2]);
                break;
            case "Г":
                ButtonSuccess(_allButtons._buttonsArea[numButtons]._buttons[3]);
                break;
            default:
                return;
                break;
        }
    }    
    
    public void SetErrorAnswer(string userAnswer, int numButtons) {

        switch (userAnswer)
        {
            case "А":
                ButtonError(_allButtons._buttonsArea[numButtons]._buttons[0]);
                break;
            case "Б":
                ButtonError(_allButtons._buttonsArea[numButtons]._buttons[1]);
                break;
            case "В":
                ButtonError(_allButtons._buttonsArea[numButtons]._buttons[2]);
                break;
            case "Г":
                ButtonError(_allButtons._buttonsArea[numButtons]._buttons[3]);
                break;
            default:
                return;
                break;
        }
    }

    public int ReturnerValueQuestion(string value) {

        switch (value)
        {
            case "А":
                return 1;
                break;
            case "Б":
                return 2;
                break;
            case "В":
                return 3;
                break;
            case "Г":
                return 4;
                break;
            default:
                return 0;
        }

    }
        void CheckTest() {
        
        foreach (var item in myBiletList.bilets)
        {
            if (item.biletNumber == biletNum)
            {

                score = 0;

                if (question1.text.ToUpper() == item.rightAnswer[0].ToUpper())
                {
                    Debug.Log(question1.text.ToUpper());
                    score++;
                    Debug.Log(score);
                }
                else 
                {
                    SetErrorAnswer(question1.text.ToUpper(), 0);
                    FindRightAnswer(item.rightAnswer[0].ToUpper(), 0);
                    
                   
                }

                if (question2.text.ToUpper() == item.rightAnswer[1].ToUpper())
                {
                    score++;
                    Debug.Log(score);
                }
                else
                {
                    SetErrorAnswer(question2.text.ToUpper(), 1);
                    FindRightAnswer(item.rightAnswer[1].ToUpper(), 1);
                    

                }

                if (question3.text.ToUpper() == item.rightAnswer[2].ToUpper())
                {
                    score++;
                    Debug.Log(score);
                }
                else
                {
                    SetErrorAnswer(question3.text.ToUpper(), 2);
                    FindRightAnswer(item.rightAnswer[2].ToUpper(), 2);
                   

                }

                if (question4.text.ToUpper() == item.rightAnswer[3].ToUpper())
                {
                    score++;
                    Debug.Log(score);
                }
                else
                {
                    SetErrorAnswer(question4.text.ToUpper(), 3);
                    FindRightAnswer(item.rightAnswer[3].ToUpper(), 3);
                }

                PlayerPrefs.SetInt("TestScore", score);
                rightAnswerField.SetActive(true);
                textRightAnswer.text = "" + $"Правильных ответов {score} из 4";
            }
        }




    }
    public void SendData()
    {
        StartCoroutine(Send());
    }

    public IEnumerator Send()
    {
        if (question1.text.Length > 0 && question2.text.Length > 0 && question3.text.Length > 0 && question4.text.Length > 0)
        {
            CheckTest();
            Debug.Log(score);

            WWWForm form = new WWWForm();
            form.AddField("TestScore", score);
            form.AddField("GeneralScore", score);
            form.AddField("identifier", PlayerPrefs.GetInt("identifier"));

            WWW www = new WWW("http://game.lenpeh.ru/UpdateTestScoreDB.php", form);

            yield return www;
            ButtonSend.SetActive(false);
            NextLvl();

        }
    }

    public void NextLvl() {
        StartCoroutine(ResultBilet());
   
    }

    private IEnumerator ResultBilet()
    {

        switch (score)
        {
            case 4:
                PlayAudio(ResultAudio[4]);
                break;
            case 3:
                PlayAudio(ResultAudio[3]);
                break;
            case 2:
                PlayAudio(ResultAudio[2]);
                break;
            case 1:
                PlayAudio(ResultAudio[1]);
                break;
            default:
                PlayAudio(ResultAudio[0]);
                break;            
        }


       
        yield return IsEndPlaySound();
        bilet.SetActive(false);
        msg.SetActive(true);
        ButtonNExtLevel.SetActive(true);
        
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

    public void LoadSceneStrike() {
        SceneManager.LoadScene("GoToStrike");
    }
}