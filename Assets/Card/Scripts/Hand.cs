using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HandView))]
public class Hand : MonoBehaviour
{

    // ! === Player ===
    [SerializeField] private bool hookToPlayer = true;
    private Player player;

    // ! === Hand View ===
    HandView handView;

    public int total = 0;
    public int alt_total = 0;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardParent;

    [SerializeField] private CardCounter cardCounter;

    public UnityEvent OnCardChange = new();

    void Awake()
    {

        // ? --- Player ---
        if (hookToPlayer)
        {
            player = FindFirstObjectByType<Player>();
        }

        // ? --- Hand View ---
        handView = GetComponent<HandView>();

        OnCardChange.AddListener(CalculateTotal);
        OnCardChange.AddListener(UpdateCounter);

    }

    void Start()
    {
        OnCardChange.Invoke();
    }

    public void CalculateTotal()
    {
        this.total = 0;
        this.alt_total = 0;
        Card[] cards = this.GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            int cardValue = card.GetValue();

            if (card.IsAce())
            {
                this.alt_total += 11;
            }
            else
            {
                this.alt_total += cardValue;
            }

            this.total += cardValue;
        }



    }

    /// <summary>
    /// Returns the best blackjack value for the given cards, treating aces as 1 or 11.
    /// </summary>
    public static int GetBestValue(Card[] cards)
    {
        if (cards == null || cards.Length == 0)
        {
            return 0;
        }

        int total = 0;
        int aceCount = 0;

        foreach (var card in cards)
        {
            if (card == null)
            {
                continue;
            }



            total += card.GetValue();

            if (card.IsAce())
            {
                aceCount++;
            }
        }

        while (aceCount > 0 && total + 10 <= 21)
        {
            total += 10;
            aceCount--;
        }

        return total;
    }

    private void UpdateCounter()
    {
        Debug.Log("Should be uupdating");
        if (!cardCounter) return;
        Debug.Log("Passed conditional");

        // Update cardCounter
        string new_text = $"{this.total}";
        if (this.alt_total != this.total)
        {
            new_text += $"/{this.alt_total}";
        }

        Debug.Log("Called uupdate text");

        cardCounter.UpdateText(new_text);
    }

    public void AddCard(Face face, Glyph glyph)
    {

        Debug.Log($"Adding Card {face} {glyph}");
        if (cardPrefab == null)
        {
            Debug.LogWarning("Hand.AddCard: cardPrefab is not assigned");
            return;
        }

        Transform parent = cardParent != null ? cardParent : this.transform;
        GameObject cardObject = Instantiate(cardPrefab, parent);

        Debug.Log(cardObject);

        Card card = cardObject.GetComponent<Card>();
        card.face = face;
        card.glyph = glyph;
        card.RefreshSelf();
        this.OnCardChange.Invoke();
        handView.LoadCards();
    }

    void CardDealtHook(Card card, int handValue)
    {
        this.AddCard(card.face, card.glyph);
    }


    // ! === Base Listeners ===
    void OnEnable()
    {
        if (hookToPlayer && player != null)
        {
            player.OnCardDealt += CardDealtHook; // Add Hook
        }
    }

    void OnDisable()
    {
        if (hookToPlayer && player != null)
        {
            player.OnCardDealt -= CardDealtHook; // Remove Hook
        }
    }
}
