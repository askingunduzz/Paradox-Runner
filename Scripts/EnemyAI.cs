using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform target;
    private int currentWaypoint = 0;
    private Rigidbody2D rb;
    private Vector2 movement;
    private UnityEngine.SceneManagement.Scene _scene;

    private int health = 2; // Enemy health
    private bool isDead = false;

    private void Awake()
    {
        _scene = SceneManager.GetActiveScene();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Move();
    }

    void Update()
    {
        if (!isDead)
        {
            Move();
        }
    }

    void Move()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        // Add death animation or effect here
        Destroy(gameObject, 0); // Delay destroy to show death animation
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
        }
    }
}
