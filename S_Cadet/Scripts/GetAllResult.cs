using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GetAllResult : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _scrollPosition;
    [SerializeField] private GameObject _item;
    [SerializeField] private GameObject _itemYou;

    [Space]
    private int iter; // Итерация привязанная к к-ву участников
    private int YouPos; // На какой итерации 
    private float sizeField = 1.07f; // Ширина поля с данными
    private int countField; // Ширина поля с данными

    

    [SerializeField] private float offsetY;

    [SerializeField] private VideoComponent _videoComponent;

    [Space]
    public AudioSource Audio_A;
    public AudioClip[] ResultAudio;
    public bool isResultPlaySound;
    private bool _endFind;

    // public TextAsset textJSdON;
    [System.Serializable]
    public class Users
    {
        public int id;
        public string identifier;
        public string name;
        public string lastname;
        public int TestScore;
        public int StrikeScore;
        public int GeneralScore;

        
    }

    [System.Serializable]
    public class UsersList
    {
        public Users[] users;
    }

    public UsersList userslist = new UsersList();

    private void Start()
    {
        SendData();
    }


    IEnumerator Lis() {

        yield return new WaitForSeconds(1.5f);

        var positionValue = 1 - (sizeField / (float)countField) * YouPos + (sizeField / (float)countField);

        if (positionValue <= 0)
        {
            _scrollPosition.GetComponent<Scrollbar>().value = 0;
        }
        else
        {
            _scrollPosition.GetComponent<Scrollbar>().value = positionValue;
        }

        _content.GetComponent<CanvasGroup>().alpha = 1;
    }
    public void SendData()
    {
        StartCoroutine(Send());
    }
    public void PlayAudio(AudioClip music)
    {
        Audio_A.PlayOneShot(music);
    }
    public IEnumerator Send()
    {
        WWWForm form = new WWWForm();
        form.AddField("identifier", PlayerPrefs.GetInt("identifier"));

        //WWW www = new WWW("http://localhost/cursant.loc/GetAllResult.php", form);
        WWW www = new WWW("http://game.lenpeh.ru/GetAllResult.php", form);
        //WWW www = new WWW("https://ilyaprojectsforme.000webhostapp.com/game/GetAllResult.php", form);

        yield return www;

        userslist = JsonUtility.FromJson<UsersList>(www.text);

        countField = userslist.users.Length;
        foreach (var item in userslist.users)
        {
            iter++;
            string l = item.identifier;
            string k = "" + PlayerPrefs.GetInt("identifier");

            GameObject element;


            int testValue;
            int strikeValue;
            int midlwareValue;



            if (l != k)
            {
                element = Instantiate(_item, _content.transform.position + new Vector3(0, offsetY, 0), Quaternion.identity, _content.transform);
               
 
                
                int StrikeScore = item.StrikeScore;
                int TestScore = item.TestScore;


                bool TS_4 = TestScore >= 4;
                bool TS_3 = TestScore == 3;
                bool TS_2 = TestScore == 2;
                bool TS_1 = TestScore == 1;
                bool TS_0 = TestScore <= 0;

                bool SS_3 = StrikeScore >= 25;
                bool SS_2 = StrikeScore <= 24 && StrikeScore >= 21;
                bool SS_1 = StrikeScore <= 20 && StrikeScore >= 18;
                bool SS_0 = StrikeScore < 18;



                /*              item.TestScore;
                                item.StrikeScore;
                                item.GeneralScore;*/


                if (TS_4)
                {
                    testValue = 5;
                }
                else if (TS_3)
                {
                    testValue = 4;
                }
                else if (TS_2)
                {
                    testValue = 3;
                }
                else if (TS_1)
                {
                    testValue = 3;
                }
                else if (TS_0)
                {
                    testValue = 2;
                }
                else
                {
                    testValue = 2;
                }


                if (SS_3)
                {
                    strikeValue = 5;
                }
                else if (SS_2)
                {
                    strikeValue = 4;
                }
                else if (SS_1)
                {
                    strikeValue = 3;
                }
                else if (SS_0)
                {
                    strikeValue = 2;
                }
                else
                {
                    strikeValue = 2;
                }

    ///////
    //////
    /////
    ////
    ///
    //


                if (testValue == 5 && strikeValue == 5) { // отлично 
                    midlwareValue = 5;
                }
                else if ( // двойка
                    (testValue == 2 && strikeValue == 2)
                    ||
                    (testValue == 3 && strikeValue == 2)
                    ||
                    (testValue == 2 && strikeValue == 3)
                    )
                { 
                    midlwareValue = 2;
                }
                else if (//Удавлетворительно
                    (testValue == 3 && strikeValue == 4)
                    ||
                    (testValue == 4 && strikeValue == 3)
                    ||
                    (testValue == 3 && strikeValue == 3)
                    ||
                    (testValue == 2 && strikeValue == 4)
                    ||
                    (testValue == 4 && strikeValue == 2)
                    )
                {
 
                    midlwareValue = 3;
                }
                else if (//Хорошо
                    (testValue == 4 && strikeValue == 4) 
                    || 
                    (testValue == 3 && strikeValue == 5) 
                    ||
                    (testValue == 5 && strikeValue == 3) 
                    ||
                    (testValue == 5 && strikeValue == 4) 
                    ||
                    (testValue == 4 && strikeValue == 5)
                    )
                {

                    midlwareValue = 4;
                }
                else
                {
 
                    midlwareValue = 3;
                }
                
            }
            else
            {
               element = Instantiate(_itemYou, _content.transform.position + new Vector3(0, offsetY, 0), Quaternion.identity, _content.transform);
               YouPos = iter;
                Debug.Log(YouPos);
                int StrikeScore = item.StrikeScore;
                int TestScore = item.TestScore;
           

                bool TS_4 = TestScore >= 4;
                bool TS_3 = TestScore == 3;
                bool TS_2 = TestScore == 2;
                bool TS_1 = TestScore == 1;
                bool TS_0 = TestScore <= 0;

                bool SS_3 = StrikeScore >= 25;
                bool SS_2 = StrikeScore <= 24 && StrikeScore >= 21;
                bool SS_1 = StrikeScore <= 20 && StrikeScore >= 18;
                bool SS_0 = StrikeScore < 18;
                
  

/*              item.TestScore;
                item.StrikeScore;
                item.GeneralScore;*/


                if (TS_4)
                {
                    testValue = 5;
                }
                else if (TS_3)
                {
                    testValue = 4;
                }
                else if (TS_2)
                {
                    testValue = 3;
                }
                else if (TS_1)
                {
                    testValue = 3;
                }
                else if (TS_0)
                {
                    testValue = 2;
                }
                else
                {
                    testValue = 2;
                }

                if (SS_3)
                {
                    strikeValue = 5;
                }
                else if (SS_2)
                {
                    strikeValue = 4;
                }
                else if (SS_1)
                {
                    strikeValue = 3;
                }
                else if (SS_0)
                {
                    strikeValue = 2;
                }
                else
                {
                    strikeValue = 2;
                }



              
                if (testValue == 5 && strikeValue == 5)
                { // отлично 
                    PlayAudio(ResultAudio[3]);
                    _videoComponent.SetResultBalls(3);
                    midlwareValue = 5;
                }
                else if ( // двойка
                    (testValue == 2 && strikeValue == 2)
                    ||
                    (testValue == 3 && strikeValue == 2)
                    ||
                    (testValue == 2 && strikeValue == 3)
                    )
                { // двойка
                    PlayAudio(ResultAudio[0]);
                    _videoComponent.SetResultBalls(0);
                    midlwareValue = 2;
                }
                else if (//Удавлетворительно
                    (testValue == 3 && strikeValue == 4)
                    ||
                    (testValue == 4 && strikeValue == 3)
                    ||
                    (testValue == 3 && strikeValue == 3)
                    ||
                    (testValue == 2 && strikeValue == 4)
                    ||
                    (testValue == 2 && strikeValue == 5)
                    ||
                    (testValue == 5 && strikeValue == 2)
                    ||
                    (testValue == 4 && strikeValue == 2)
                    )
                {
                    PlayAudio(ResultAudio[1]);
                     midlwareValue = 3;
                    _videoComponent.SetResultBalls(1);
                   
                }
                else if (//Хорошо
                    (testValue == 4 && strikeValue == 4)
                    ||
                    (testValue == 3 && strikeValue == 5)
                    ||
                    (testValue == 5 && strikeValue == 3)
                    ||
                    (testValue == 5 && strikeValue == 4)
                    ||
                    (testValue == 4 && strikeValue == 5)
                    )
                {
                    PlayAudio(ResultAudio[2]);
                    _videoComponent.SetResultBalls(2);
                    midlwareValue = 4;
                }
                else
                {
                    PlayAudio(ResultAudio[1]);
                    _videoComponent.SetResultBalls(1);
                    midlwareValue = 3;
                }
               
            }

            offsetY -= 50;


            element.GetComponent<UserControllerRating>()._nameText.text = "" + item.name + " " + item.lastname;
            element.GetComponent<UserControllerRating>()._res1.text = "" + testValue;
            element.GetComponent<UserControllerRating>()._res2.text = "" + strikeValue;
            element.GetComponent<UserControllerRating>()._res3.text = "" + midlwareValue;

            
        }

        StartCoroutine(Lis());

    }



}

//{"users":"[{\"id\":\"145\",\"identifier\":\"56066394\",\"name\":\"Илья\",\"lastname\":\"Тестов\",\"TestScore\":\"3\",\"StrikeScore\":\"21\",\"GeneralScore\":\"24\"},{\"id\":\"146\",\"identifier\":\"12333\",\"name\":\"123\",\"lastname\":\"123\",\"TestScore\":\"123\",\"StrikeScore\":\"123\",\"GeneralScore\":\"123\"},{\"id\":\"147\",\"identifier\":\"12333\",\"name\":\"123\",\"lastname\":\"123\",\"TestScore\":\"123\",\"StrikeScore\":\"123\",\"GeneralScore\":\"123\"}]"}
