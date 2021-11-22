using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassPaint : MonoBehaviour
{
    public static GameObject glassInstance;
    GameController gameController;
    List<GameObject> gameObjects;

    private void Awake()
    {
            glassInstance = gameObject;
        gameObjects = new List<GameObject>();
        gameController = GameController.gameController;
        foreach (Transform child in transform)
        {
            gameObjects.Add(child.gameObject);
        }
    }
    void Start()
    {
        
    }

    public void PaintGlass()
    {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.GetComponent<SpriteRenderer>().color = gameController._currentBorder.transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            }
    }

}
