using UnityEngine;

public class Chip : MonoBehaviour
{   
    // ! === Properties ===
    [Tooltip("Denomination of the specified chip")]
    [SerializeField] public int denomination;

    [Tooltip("Minimum amount the player should have at any point")]
    [SerializeField] public int min;

    /// <summary>
    /// ID of the chip
    /// </summary>
    public string Id
    {
        get => $"chip-{denomination}";
    }
}
