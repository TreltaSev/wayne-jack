using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;
using Game;

public class Player : MonoBehaviour
{

    // Player Balance
    public float balance;
    public int pendingBet;


    // ? --- References --- //
    public CardCounter cardCounter;
    public CardGenerator cardGenerator;

    // Currency Change Events

    [Tooltip("Add events that are called whenever the balance changes")]
    public UnityEvent<float, float> e_balance_change;

    [Tooltip("GameObject containing a TextMeshProUGUI that displays the player's balance")]
    public GameObject balanceTextObject;

    public GameObject pendingBetTextObject;

    public Card[] playerCards = Array.Empty<Card>();
    public event Action<Card, int> OnCardDealt;
    public event Action<int> OnPlayerBust;
    public event Action<int> OnPlayerStand;
    
    // Goal tracking
    public float goal = 0f;
    public event Action<float> OnGoalReached;
    [Tooltip("Invoked when the player's balance reaches or exceeds the goal")]
    public UnityEvent<float> OnGoalReachedEvent;
    private bool goalReached = false;

    void Start()
    {
        e_balance_change.AddListener(OnBalanceChange);
    }

    public void OnPendingBetChange(float result)
    {
        if (pendingBetTextObject != null)
        {
            var tmp = pendingBetTextObject.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = result.ToString();
            }
        }
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
        // Goal handling: fire once when crossing or reaching the goal
        if (goal > 0f)
        {
            if (!goalReached && this.balance >= goal)
            {
                goalReached = true;
                OnGoalReached?.Invoke(this.balance);
                OnGoalReachedEvent?.Invoke(this.balance);
            }
            else if (goalReached && this.balance < goal)
            {
                goalReached = false;
            }
        }
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

        if (balanceTextObject != null)
        {
            var tmp = balanceTextObject.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = result.ToString();
            }
        }

        return;
    }

    

    public void TestEvent(float x, float y)
    {
        Debug.Log($"[TestEvent] x: {x} y:{y}");
    }

    /// <summary>
    /// Deals one random card to the player hand.
    /// </summary>
    /// <returns></returns>
    public Card DealCard()
    {
        Card card = Card.GenerateRandom();
        Array.Resize(ref playerCards, playerCards.Length + 1);
        playerCards[playerCards.Length - 1] = card;

        int handValue = GetHandValue();        

        Debug.Log($"Player drew: {card.face} {card.glyph} (hand value: {handValue})");
        OnCardDealt?.Invoke(card, handValue);

        cardCounter.UpdateText(handValue.ToString());

        cardGenerator.Create(card.face, card.glyph);

        if (handValue > 21)
        {
            Debug.Log($"Player busts with {handValue}");
            OnPlayerBust?.Invoke(handValue);
        }

        return card;
    }

    /// <summary>
    /// Calculates the player's current hand value.
    /// </summary>
    /// <returns></returns>
    public int GetHandValue()
    {
        return Hand.GetBestValue(playerCards);
    }

    /// <summary>
    /// Returns true if the player's hand value is greater than 21.
    /// </summary>
    /// <returns></returns>
    public bool IsBust()
    {
        return GetHandValue() > 21;
    }

    /// <summary>
    /// Triggers a stand event with the current hand value.
    /// </summary>
    public void Stand()
    {
        int handValue = GetHandValue();
        Debug.Log($"Player stands with {handValue}");
        OnPlayerStand?.Invoke(handValue);
    }

    /// <summary>
    /// Resets player hand state and destroys generated card objects.
    /// </summary>
    public void ResetAll()
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (playerCards[i] != null)
            {
                Destroy(playerCards[i].gameObject);
            }
        }

        playerCards = Array.Empty<Card>();
        cardGenerator.DeleteAll();
        Debug.Log("Player reset complete");
    }
}
