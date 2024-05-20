using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 70f;
    public AudioClip shootSound; // Ateş etme sesi
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource bileşenini al
        audioSource = GetComponent<AudioSource>();

        // Eğer AudioSource yoksa ekle
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        // Mouse pozisyonunu dünya koordinatlarına çevir
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z eksenini sıfırla (2D oyun olduğu için)

        // Mermi instantiate et
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Mermiye doğru bir yön hesapla
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Mermiye kuvvet uygula
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;

        // Mermi sprite'ının yönünü ayarla
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Ateş etme sesini çal
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
