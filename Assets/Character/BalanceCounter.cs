using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BalanceCounter : MonoBehaviour
{

    private readonly TextMeshProUGUI textMeshProUGUI;

    public void UpdateBalance(int value)
    {
        textMeshProUGUI.text = $"${value}";
    }
}
