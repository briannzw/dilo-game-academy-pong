using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Bird bird;
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        if (!bird.IsDead())
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
        else Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Pipe>())
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
