using UnityEngine;
using System;

public class Dealer : MonoBehaviour
{
	[Header("View")]
	public GameObject dealerHand;

	public Card[] dealerCards = Array.Empty<Card>();
	public event Action<Card, int> OnCardDealt;
	public event Action<int> OnDealerBust;
	public event Action<int> OnDealerStand;
    public Game.CardGenerator cardGenerator;


	/// <summary>
	/// Deals the dealer's opening two cards.
	/// </summary>
	public void DealOpeningHand()
	{
		DealCard();
		DealCard();
		Debug.Log($"Dealer opening hand value: {GetHandValue()}");
	}

	/// <summary>
	/// Deals one card to the dealer hand and triggers deal events.
	/// </summary>
	/// <returns></returns>
	public Card DealCard()
	{
		Card card = Card.GenerateRandom();
		Array.Resize(ref dealerCards, dealerCards.Length + 1);
		dealerCards[dealerCards.Length - 1] = card;
		int handValue = GetHandValue();

		Debug.Log($"Dealer drew: {card.face} {card.glyph} (hand value: {handValue})");

		OnCardDealt?.Invoke(card, handValue);

		// Ensure there is a dealer Hand GameObject to receive visual cards.
		if (dealerHand == null)
		{
			// Try to find an existing Hand in the scene (e.g., the player's hand) and clone it.
			Hand template = FindFirstObjectByType<Hand>();
			if (template != null)
			{
				GameObject newHandObj = Instantiate(template.gameObject, this.transform);
				newHandObj.name = "DealerHand";
				dealerHand = newHandObj;
				var newHandComp = newHandObj.GetComponent<Hand>();
				if (newHandComp != null)
				{
					// Prevent the dealer hand from hooking into player events
					var hookField = newHandComp.GetType().GetField("hookToPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
					if (hookField != null) hookField.SetValue(newHandComp, false);
				}
			}
			else
			{
				Debug.LogWarning("No Hand template found in scene to instantiate dealerHand. Assign a dealerHand GameObject in the Inspector.");
			}
		}

		if (dealerHand != null)
		{
			var handComp = dealerHand.GetComponent<Hand>();
			if (handComp != null)
			{
				handComp.AddCard(card.face, card.glyph);
			}
		}

		return card;
	}

	/// <summary>
	/// Calculates the dealer's current hand value.
	/// </summary>
	/// <returns></returns>
	public int GetHandValue()
	{
		return Hand.GetBestValue(dealerCards);
	}

	/// <summary>
	/// Returns true if the dealer hand value is greater than 21.
	/// </summary>
	/// <returns></returns>
	public bool IsBust()
	{
		return GetHandValue() > 21;
	}

	/// <summary>
	/// Runs dealer logic to hit until 17 or more, then bust or stand.
	/// </summary>
	public void PlayDealerTurn()
	{
		while (GetHandValue() < 17)
		{
			DealCard();
		}
		int handValue = GetHandValue();

		if (handValue > 21)
		{
			Debug.Log($"Dealer busts with {handValue}");
			OnDealerBust?.Invoke(handValue);
			return;
		}

		Debug.Log($"Dealer stands with {handValue}");
		OnDealerStand?.Invoke(handValue);
	}

	/// <summary>
	/// Resets dealer state and removes all generated card objects.
	/// </summary>
	public void ResetAll()
	{
		for (int i = 0; i < dealerCards.Length; i++)
		{
			if (dealerCards[i] != null)
			{
				Destroy(dealerCards[i].gameObject);
			}
		}

		dealerCards = Array.Empty<Card>();
		cardGenerator.DeleteAll();
		Debug.Log("Dealer reset complete");
	}
}
