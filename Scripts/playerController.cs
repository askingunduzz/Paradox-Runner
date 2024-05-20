using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 0.3f;
    public AudioClip jumpSound; // Zıplama sesi
    public AudioClip hizlanSound; // Hizlan sesi
    public AudioClip yavaslaSound; // Yavasla sesi
    public AudioClip gerialSound; // GeriAl sesi

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isRewinding = false;
    private AudioSource audioSource;

    private List<Vector2> positionHistory = new List<Vector2>();
    private List<Vector2> velocityHistory = new List<Vector2>();
    private int historyLimit = 500; // 5 seconds of history at 0.01 seconds per record
private Vector3 originalScale;

    private CameraController cameraController;

    void Start()
{
    rb = GetComponent<Rigidbody2D>();
    cameraController = Camera.main.GetComponent<CameraController>();
    audioSource = GetComponent<AudioSource>(); // AudioSource bileşenini al

    if (audioSource == null)
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Eğer yoksa, yeni bir AudioSource bileşeni ekle
    }

    StartCoroutine(RecordMovement());

    // Başlangıç ölçeğini sakla
    originalScale = transform.localScale;
}

   void Update()
{
    if (isRewinding) return;

    // Yatay hareket
    float moveInput = Input.GetAxis("Horizontal");
    rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

    // Karakterin yönünü değiştir
    if (moveInput < 0)
    {
        transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }
    else if (moveInput > 0)
    {
        transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
    }

    // Zıplama
    if (Input.GetButtonDown("Jump") && isGrounded)
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        // Zıplama sesini çal
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }
}



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Hizlan"))
        {
            Destroy(collision.gameObject);
            cameraController.SpeedUp(2f, 5f); // Kameranın hızını 2 kat artır

            // Hizlan sesini çal
            if (hizlanSound != null)
            {
                audioSource.PlayOneShot(hizlanSound);
            }
        }
        else if (collision.gameObject.CompareTag("Yavasla"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(Yavaslatma());

            // Yavasla sesini çal
            if (yavaslaSound != null)
            {
                audioSource.PlayOneShot(yavaslaSound);
            }
        }
        else if (collision.gameObject.CompareTag("GeriAl"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(RewindMovement());

            // GeriAl sesini çal
            if (gerialSound != null)
            {
                audioSource.PlayOneShot(gerialSound);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    IEnumerator RecordMovement()
    {
        while (true)
        {
            if (!isRewinding)
            {
                positionHistory.Insert(0, transform.position);
                velocityHistory.Insert(0, rb.velocity);

                if (positionHistory.Count > historyLimit)
                {
                    positionHistory.RemoveAt(positionHistory.Count - 1);
                    velocityHistory.RemoveAt(velocityHistory.Count - 1);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator RewindMovement()
    {
        isRewinding = true;
        rb.velocity = Vector2.zero;

        cameraController.Rewind(5f); // Kamera 5 saniye boyunca geri gider

        while (positionHistory.Count > 0)
        {
            transform.position = positionHistory[0];
            rb.velocity = velocityHistory[0];

            positionHistory.RemoveAt(0);
            velocityHistory.RemoveAt(0);

            yield return new WaitForSeconds(0.005f); // 2 kat hızlı geri alma
        }

        rb.velocity = Vector2.zero;
        isRewinding = false;
    }

    IEnumerator Yavaslatma()
    {
        // Hızı 5 saniyeliğine yarıya indir
        moveSpeed /= 2f;
        yield return new WaitForSeconds(5f);
        moveSpeed *= 2f; // Hızı eski değerine geri döndür
    }
}
