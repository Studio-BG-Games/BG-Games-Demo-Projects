using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPowerTrigger : MonoBehaviour
{
    GameController gameController;

    private void Start()
    {
        gameController = GameController.gameController;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player" && gameController.isSuperPowerState)
        {
            GetComponent<SpriteMask>().enabled = true;
            gameController._currentBorder.GetComponent<Border>().InstantinateGlassAndEffects(gameController._player.transform.position);
            gameController._currentBorder.transform.GetChild(0).gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            gameController.RewriteScore();
            GlassPaint.glassInstance.GetComponent<GlassPaint>().PaintGlass();
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}
