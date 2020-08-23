using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public BallControl ball;
    private Rigidbody2D ballRigidbody;

    public float speed = 10f;
    public float yBoundary = 9f;
    private Rigidbody2D rigidBody2D;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 velocity = rigidBody2D.velocity;

        if (ballRigidbody.velocity.x >= 0)
        {
            if (ball.transform.position.y < transform.position.y)
            {
                velocity.y = -speed;
            }
            else if (ball.transform.position.y > transform.position.y)
            {
                velocity.y = speed;
            }
        }
        else velocity.y = 0f;

        rigidBody2D.velocity = velocity;

        Vector3 position = transform.position;

        if (position.y > yBoundary)
        {
            position.y = yBoundary;
        }
        else if (position.y < -yBoundary)
        {
            position.y = -yBoundary;
        }

        transform.position = position;
    }
}
