using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{

    public class Dealer : MonoBehaviour
    {

        // ! === References === //
        public GameObject dealerHand;

        public CardGenerator cardGenerator;
        public CardCounter cardCounter;

        // ! === Cards === //
        public Card[] dealerCards = Array.Empty<Card>();

        // ! === Events === //
        public UnityEvent<Card> OnCardDealt = new();
        public UnityEvent<int> OnDealerBust = new();
        public UnityEvent<int> OnDealerStand = new();

        public void DealOpeningHand()
        {
            DealCard();
            DealCard(flipped: true);
            Debug.Log($"Dealer opening hand value: {GetHandValue()}");
        }

        public void DealCard(bool flipped = false)
        {
            Card randomCard = Card.GenerateRandom();
            Array.Resize(ref dealerCards, dealerCards.Length + 1);
            dealerCards[dealerCards.Length - 1] = randomCard;

            cardCounter.UpdateText(Hand.GetBestValue(dealerCards).ToString());

            OnCardDealt.Invoke(randomCard);


            cardGenerator.Create(randomCard.face, randomCard.glyph, flipped);
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

        public int GetHandValue()
        {
            return Hand.GetBestValue(dealerCards);
        }
    }

}