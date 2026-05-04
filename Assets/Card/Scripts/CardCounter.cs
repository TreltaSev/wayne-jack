using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CardCounter : MonoBehaviour
{

    private TextMeshProUGUI textMeshProUGUI;

    void Awake()
    {
        this.textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(string value)
    {
        textMeshProUGUI.text = value;
    }
}
