using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    public PlayerControl player;
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Ball")
        {
            gameManager.RestartFireball();
            gameManager.RestartPowerUp();
            player.IncrementScore();

            if (player.Score < gameManager.maxScore)
            {
                collision.gameObject.SendMessage("RestartGame", 2f, SendMessageOptions.RequireReceiver);
            }
        }
        if(collision.name == "FireBall")
        {
            gameManager.RestartFireball();
            collision.gameObject.SetActive(false);
        }
        if(collision.name == "PowerUp")
        {
            gameManager.RestartPowerUp();
            collision.gameObject.SetActive(false);
        }
    }
}
