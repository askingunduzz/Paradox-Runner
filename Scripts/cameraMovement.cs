using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public float cameraSpeed = 4f;
    public bool isSpeedingUp = false;
    public bool isRewinding = false;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);

        if (!isRewinding)
        {
            // Kamerayý saða doðru hareket ettir
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
        cameraSpeed *= multiplier; // Kameranýn hýzýný artýr
        yield return new WaitForSeconds(duration); // Belirtilen süre kadar bekle
        cameraSpeed /= multiplier; // Kameranýn hýzýný eski haline getir
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
