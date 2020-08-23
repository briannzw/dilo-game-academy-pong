using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerControl player1;
    private Rigidbody2D player1Rigidbody;

    public PlayerControl player2;
    private Rigidbody2D player2Rigidbody;
    private EnemyAI player2AI;

    public BallControl ball;
    private Rigidbody2D ballRigidbody;
    private CircleCollider2D ballCollider;

    public int maxScore;

    private bool isDebugWindowShown = false;
    public Trajectory trajectory;

    public FireBall fireBall;
    public PowerUp powerUp;

    private void Start()
    {
        player1Rigidbody = player1.GetComponent<Rigidbody2D>();
        player2Rigidbody = player2.GetComponent<Rigidbody2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
        player2AI = player2.GetComponent<EnemyAI>();

        RestartFireball();
        RestartPowerUp();
    }

    public void RestartFireball()
    {
        CancelInvoke("SpawnFireball");
        fireBall.gameObject.SetActive(false);
        Invoke("SpawnFireball", Random.Range(3, 7));
    }

    public void RestartPowerUp()
    {
        CancelInvoke("SpawnPowerUp");
        powerUp.gameObject.SetActive(false);
        Invoke("SpawnPowerUp", Random.Range(2, 5));
    }

    private void SpawnFireball()
    {
        fireBall.transform.position = Vector2.zero;
        fireBall.gameObject.SetActive(true);
        fireBall.PushBall();
    }

    private void SpawnPowerUp()
    {
        powerUp.transform.position = Vector2.zero;
        powerUp.gameObject.SetActive(true);
        powerUp.PushBall();
    }

    public void ResetPlayerScale()
    {
        player1.ResetScale();
        player2.ResetScale();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + player1.Score);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2.Score);

        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
        {
            player1.ResetScore();
            player2.ResetScore();

            RestartFireball();
            RestartPowerUp();
            ball.SendMessage("RestartGame", .5f, SendMessageOptions.RequireReceiver);
        }

        if (player1.Score == maxScore)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 2000, 1000), "PLAYER ONE WINS");

            CancelInvoke("SpawnFireball");
            CancelInvoke("SpawnPowerUp");
            fireBall.gameObject.SetActive(false);
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }
        else if (player2.Score == maxScore)
        {
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER TWO WINS");

            CancelInvoke("SpawnFireball");
            CancelInvoke("SpawnPowerUp");
            fireBall.gameObject.SetActive(false);
            ball.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        }

        if (isDebugWindowShown)
        {
            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;

            float ballMass = ballRigidbody.mass;
            Vector2 ballVelocity = ballRigidbody.velocity;
            float ballSpeed = ballRigidbody.velocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity;
            float ballFriction = ballCollider.friction;

            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;

            string debugText =
                "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n" +
                "To turn on enemy AI, press Z button. For better AI press X button\n" +
                "To turn on player 2 control, press C button";

            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 150), debugText, guiStyle);

            GUI.backgroundColor = oldColor;
        }

        if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 73, 120, 53), "TOOGLE\nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
            trajectory.enabled = !trajectory.enabled;
        }
    }

    public Vector2 offsetPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            player2.enabled = false;
            player2Rigidbody.velocity = Vector2.zero;
            player2AI.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Advanced AI hanya dapat dijalankan ketika Trajectory aktif.");
            player2.enabled = false;
            player2Rigidbody.velocity = Vector2.zero;
            player2AI.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            player2.enabled = true;
            player2Rigidbody.velocity = Vector2.zero;
            player2AI.enabled = false;
        }

        if(!player2.enabled && !player2AI.enabled) AdvanceAI();
    }

    private void AdvanceAI()
    {
        if(ballRigidbody.velocity.x > 0)
        {
            float yPoint = Mathf.MoveTowards(player2.transform.position.y, offsetPoint.y, player2.speed * Time.deltaTime);
            player2.transform.position = new Vector2(player2.transform.position.x, yPoint);

            if (yPoint > player2.yBoundary) player2.transform.position = new Vector2(player2.transform.position.x, player2.yBoundary);
            else if (yPoint < -player2.yBoundary) player2.transform.position = new Vector2(player2.transform.position.x, -player2.yBoundary);
        }
    }
}
