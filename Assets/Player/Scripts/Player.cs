using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    // Player Balance
    public float balance;

    // Currency Change Events

    [Tooltip("Add events that are called whenever the balance changes")]
    public UnityEvent<float, float> e_balance_change;

    void Start()
    {
        e_balance_change.AddListener(OnBalanceChange);
    }

    /// <summary>
    /// Offset's the player's balance by a certain amount. If the result is a negative number,
    /// nothing changes.
    /// </summary>
    /// <param name="offset">
    /// How much to offset the balance by
    /// </param>b
    /// <returns>
    /// The new balance or null if the result becomes negative
    /// </returns>
    public object OffsetBalance(float offset)
    {
        float result = this.balance + offset;
        if (result < 0) return null;
        this.balance = result;
        e_balance_change.Invoke(offset, result);
        return result;
    }

    public void OtherTestFunction()
    {
        Debug.Log("...l;l");
        this.OffsetBalance(200);
    }

    void OnBalanceChange(float offset, float result)
    {
        Debug.Log($"[OnBalanceChange] offset:{offset} result:{result}");
        return;
    }

    public void TestEvent(float x, float y)
    {
        Debug.Log($"[TestEvent] x: {x} y:{y}");
    }
}
