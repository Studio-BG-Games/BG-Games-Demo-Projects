using UnityEngine;

public class ResetPrefs : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.E)) {
            Res();
        }
    }

    public void Res()
    {
        Debug.Log("Обнулились");
        PlayerPrefs.SetInt("identifier", 0);
    }
}
