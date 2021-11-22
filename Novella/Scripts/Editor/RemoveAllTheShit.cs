using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

public class RemoveAllTheShit : MonoBehaviour
{

    // Update is called once per frame
    [MenuItem("Tools/Delete All The Shit")]
    public static void DeleteAllTheShit()
    {
        ES3.DeleteDirectory(Application.persistentDataPath);
        PlayerPrefs.DeleteAll();
    }
}