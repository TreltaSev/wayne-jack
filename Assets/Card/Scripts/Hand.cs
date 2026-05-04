using System;
using UnityEngine;
using UnityEngine.Events;

public class Hand : MonoBehaviour
{
    public int total = 0;
    public int alt_total = 0;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardParent;

    [SerializeField] private CardCounter cardCounter;

    public UnityEvent OnCardChange = new();

    void Awake()
    {
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
        GameObject cardObject = Instantiate(cardPrefab, cardParent);

        Card card = cardObject.GetComponent<Card>();
        card.face = face;
        card.glyph = glyph;
        card.RefreshSelf();
        this.OnCardChange.Invoke();
    }
}
