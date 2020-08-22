using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;

    public float xInitialForce;
    public float yInitialForce;

    public GameManager gameManager;
    public Vector3 scaleUp;
    public float time;

    public BallControl ball;

    public void PushBall()
    {
        float yRandomInitialForce = Random.Range(-yInitialForce, yInitialForce);
        float randomDirection = Random.Range(0, 2);

        float speed = new Vector2(xInitialForce, yInitialForce).magnitude;

        if(randomDirection < 1f)
        {
            Vector2 direction = new Vector2(-xInitialForce, yRandomInitialForce).normalized;
            rigidBody2D.AddForce(direction * speed);
        }
        else
        {
            Vector2 direction = new Vector2(xInitialForce, yRandomInitialForce).normalized;
            rigidBody2D.AddForce(direction * speed);
        }
    }

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControl>())
        {
            collision.gameObject.GetComponent<PlayerControl>().PowerUp(scaleUp, time);
            gameManager.RestartPowerUp();
        }
    }
}
