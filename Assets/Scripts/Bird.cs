using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Bird : MonoBehaviour
{
    [SerializeField] private float upForce = 100f;
    [SerializeField] private bool isDead = false;
    [SerializeField] private UnityEvent OnJump, OnDead, OnShoot, OnFailedShoot;

    [SerializeField] private int score;
    [SerializeField] private UnityEvent OnAddPoint;

    [SerializeField] private Text scoreText;

    [SerializeField] private Bullet bulletRef;

    private Rigidbody2D rigidBody2d;

    private Animator animator;

    void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!isDead)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (score > 0)
                {
                    Shoot();
                }
                else
                {
                    if (OnFailedShoot != null) OnFailedShoot.Invoke();
                }
            }
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void Dead()
    {
        if(!isDead && OnDead != null)
        {
            OnDead.Invoke();
        }

        isDead = true;
    }

    void Jump()
    {
        if (rigidBody2d)
        {
            rigidBody2d.velocity = Vector2.zero;
            rigidBody2d.AddForce(new Vector2(0, upForce));
        }

        if(OnJump != null)
        {
            OnJump.Invoke();
        }
    }

    void Shoot()
    {
        Bullet bullet = Instantiate(bulletRef, bulletRef.transform.position, Quaternion.Euler(0f, 0f, -90f));
        bullet.gameObject.SetActive(true);

        score--;
        scoreText.text = score.ToString();

        if(OnShoot != null)
        {
            OnShoot.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.enabled = false;
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();

        if(OnAddPoint != null)
        {
            OnAddPoint.Invoke();
        }
    }
}
