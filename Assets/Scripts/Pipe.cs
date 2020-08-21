using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private Bird bird;
    [SerializeField] private float speed = 1f;

    private void Update()
    {
        if (!bird.IsDead())
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bird bird = collision.gameObject.GetComponent<Bird>();

        if (bird)
        {
            Collider2D collider = GetComponent<Collider2D>();

            if (collider)
            {
                collider.enabled = false;
            }

            bird.Dead();
        }
    }
}
