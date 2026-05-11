using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode] // Enables preview in Editor
public class HueSlider : MonoBehaviour
{
    public Image targetImage;
    [Range(0f, 1f)]
    public float hue;
    [Range(0f, 1f)]
    public float saturation = 1f;
    [Range(0f, 1f)]
    public float value = 1f;

    void Update()
    {
        if (targetImage != null)
        {
            targetImage.color = Color.HSVToRGB(hue, saturation, value);
        }
    }
}
