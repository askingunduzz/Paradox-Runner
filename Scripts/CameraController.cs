using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
     public float cameraSpeed = 2f; // Kameranın başlangıç hızı
    private bool isSpeedingUp = false;
    private bool isRewinding = false;

    void Update()
    {
        if (!isRewinding)
        {
            // Kamerayı sağa doğru hareket ettir
            transform.position += Vector3.right * cameraSpeed * Time.deltaTime;
        }
    }

    public void SpeedUp(float multiplier, float duration)
    {
        if (!isSpeedingUp)
        {
            StartCoroutine(SpeedUpCoroutine(multiplier, duration));
        }
    }

    IEnumerator SpeedUpCoroutine(float multiplier, float duration)
    {
        isSpeedingUp = true;
        cameraSpeed *= multiplier; // Kameranın hızını artır
        yield return new WaitForSeconds(duration); // Belirtilen süre kadar bekle
        cameraSpeed /= multiplier; // Kameranın hızını eski haline getir
        isSpeedingUp = false;
    }

    public void Rewind(float duration)
    {
        if (!isRewinding)
        {
            StartCoroutine(RewindCoroutine(duration));
        }
    }

    IEnumerator RewindCoroutine(float duration)
    {
        isRewinding = true;
        float rewindSpeed = -cameraSpeed;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.position += Vector3.right * rewindSpeed * Time.deltaTime;
            yield return null;
        }

        isRewinding = false;
    }
}