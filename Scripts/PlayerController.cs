using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class player : MonoBehaviour
{
    public float playerSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 playerDirection;
    private Vector2 playerPosition;
    private Animator animator;
    private bool isRewinding = false;
    private cameraMovement CameraMovement;
    public AudioClip jumpSound; // Zýplama sesi
    public AudioClip hizlanSound; // Hizlan sesi
    public AudioClip yavaslaSound; // Yavasla sesi
    public AudioClip gerialSound; // GeriAl sesi
    private AudioSource audioSource;

    private List<Vector2> positionHistory = new List<Vector2>();
    private List<Vector2> velocityHistory = new List<Vector2>();
    private int historyLimit = 500; // 5 seconds of history at 0.01 seconds per record
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // AudioSource bileþenini al
        CameraMovement = Camera.main.GetComponent<cameraMovement>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Eðer yoksa, yeni bir AudioSource bileþeni ekle
        }

        StartCoroutine(RecordMovement());

        // Baþlangýç ölçeðini sakla
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float directionY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(0, directionY).normalized;
        float directionX = Input.GetAxisRaw("Horizontal");
        playerPosition = new Vector2(directionX, 0).normalized;

        if (isRewinding) return;

        // Yatay hareket
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * playerSpeed, rb.velocity.y);

        // Karakterin yönünü deðiþtir
        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else if (moveInput > 0)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(playerPosition.x * playerSpeed, playerDirection.y * playerSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hizlan"))
        {
            Destroy(collision.gameObject);
            CameraMovement.SpeedUp(2f, 5f); // Kameranýn hýzýný 2 kat artýr

            // Hizlan sesini çal
            if (hizlanSound != null)
            {
                audioSource.PlayOneShot(hizlanSound);
            }
        }
        /*else if (collision.gameObject.CompareTag("Yavasla"))
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
        }*/
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

        IEnumerator RewindMovement()
        {
            isRewinding = true;
            rb.velocity = Vector2.zero;

            CameraMovement.Rewind(5f); // Kamera 5 saniye boyunca geri gider

            while (positionHistory.Count > 0)
            {
                transform.position = positionHistory[0];
                rb.velocity = velocityHistory[0];

                positionHistory.RemoveAt(0);
                velocityHistory.RemoveAt(0);

                yield return new WaitForSeconds(0.005f); // 2 kat hýzlý geri alma
            }

            rb.velocity = Vector2.zero;
            isRewinding = false;
        }

        IEnumerator Yavaslatma()
        {
            // Hýzý 5 saniyeliðine yarýya indir
            playerSpeed /= 2f;
            yield return new WaitForSeconds(5f);
            playerSpeed *= 2f; // Hýzý eski deðerine geri döndür
        }
    }
}
