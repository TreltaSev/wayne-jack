using System.Collections;
using UnityEngine;

using UnityEngine.UI; // Required for Image component

public class WobblyImage : MonoBehaviour
{
    [Header("Shake Settings")]
    public float duration = 0.5f;
    [Tooltip("duration of shake")]
    public float magnitude = 0.1f;
    [Tooltip("how far image shakes out ")]
    public void TriggerShake() {
        StartCoroutine(Shake());
    }
    void Awake()
    {
        // Get the Image component
        Image imageComponent = GetComponent<Image>();

        if (imageComponent)
        {
            TriggerShake();
        }
    }
    private IEnumerator Shake() {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        float currentMag = magnitude; 

        while (elapsed < duration) {
            // Generate a random offset
            float x = Random.Range(-1f, 1f) * currentMag;
            float y = Random.Range(-1f, 1f) * currentMag;
            currentMag -= (magnitude / duration) * Time.deltaTime; 

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        transform.localPosition = originalPos; // Reset position
    }
}
