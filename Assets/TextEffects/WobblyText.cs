
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]

// yoinked this from google -- eldin 
public class WobblyText : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("How long the shake lasts in seconds.")]
    public float shakeDuration = 1f;
    [Tooltip("How far the characters move. Adjust based on your canvas scale.")]
    public float shakeIntensity = 5f;
    [Tooltip("If true, the shake smoothly fades out over the duration.")]
    public bool fadeOutShake = true;

    private TMP_Text textComponent;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        // Get the TextMeshPro component (works for both UI and World Space)
        textComponent = GetComponent<TMP_Text>();

        if (textComponent)
        {
            AddScoreAndShake();
        }
    }

    /// <summary>
    /// commented out the the text stuff since it'll just be used for the text shaking only
    /// </summary>
    public void AddScoreAndShake() // int newScoreValue
    {
        // 1. Update the text string (format to 5 digits, e.g., 00150)
        //textComponent.text = newScoreValue.ToString("D5");

        // 2. Start the shake effect
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCharactersRoutine());
    }

    private IEnumerator ShakeCharactersRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate current intensity (fades to 0 if fadeOutShake is true)
            float currentIntensity = shakeIntensity;
            if (fadeOutShake)
            {
                currentIntensity = Mathf.Lerp(shakeIntensity, 0f, elapsedTime / shakeDuration);
            }

            // Force TMP to update its mesh so we have a clean slate of vertices to work with
            textComponent.ForceMeshUpdate();
            TMP_TextInfo textInfo = textComponent.textInfo;

            // Loop through each character in the text
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                // Skip spaces or invisible characters
                if (!charInfo.isVisible) continue;

                // Get the index of the material and vertices for this specific character
                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                // Generate a random 2D offset for this specific character
                Vector3 randomOffset = (Vector3)Random.insideUnitCircle * currentIntensity;

                // Apply the offset to all 4 vertices of the character's quad
                Vector3[] sourceVertices = textInfo.meshInfo[materialIndex].vertices;
                sourceVertices[vertexIndex + 0] += randomOffset;
                sourceVertices[vertexIndex + 1] += randomOffset;
                sourceVertices[vertexIndex + 2] += randomOffset;
                sourceVertices[vertexIndex + 3] += randomOffset;
            }

            // Push the modified vertices back to the TextMeshPro component
            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            // Wait for the next frame
            yield return null;
        }

        // Once the shake is done, force one final update to snap everything perfectly back into place
        textComponent.ForceMeshUpdate();
    }
}




