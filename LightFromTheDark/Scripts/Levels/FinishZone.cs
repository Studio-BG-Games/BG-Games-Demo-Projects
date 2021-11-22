using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            string nameLevelTime = "TimeLevel_" + SceneManager.GetActiveScene().buildIndex;
            float oldTimeLevel = PlayerPrefs.GetFloat(nameLevelTime);
            float newTimeLevel = Time.timeSinceLevelLoad;

            if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("LevelsPassed"))
            {
                PlayerPrefs.SetInt("LevelsPassed", SceneManager.GetActiveScene().buildIndex);
            }
            if(newTimeLevel < oldTimeLevel || oldTimeLevel < 0.01f)
            {
                PlayerPrefs.SetFloat(nameLevelTime, newTimeLevel);
            }
            if(SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
            {
                FindObjectOfType<SoundLevel>().GetComponent<SoundLevel>().DestroyObject();
                SceneManager.LoadScene(0);
            }
        }
    }
}
