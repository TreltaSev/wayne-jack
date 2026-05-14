using System;
using System.Collections;
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

        [Header("Turn Timing")]
        [SerializeField] private float dealDelaySeconds = 0.5f;

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

            if (cardCounter) {
                cardCounter.UpdateText(Hand.GetBestValue(dealerCards).ToString());
            }

            OnCardDealt.Invoke(randomCard);


            cardGenerator.Create(randomCard.face, randomCard.glyph, flipped);
        }

        /// <summary>
        /// Runs dealer logic to flip index 1, then hit until 17 or more.
        /// </summary>
        public IEnumerator PlayDealerTurn()
        {
            FlipCardAtIndex(1);

            if (dealDelaySeconds > 0f)
            {
                yield return new WaitForSeconds(dealDelaySeconds);
            }

            while (GetHandValue() < 17)
            {
                DealCard();

                if (dealDelaySeconds > 0f)
                {
                    yield return new WaitForSeconds(dealDelaySeconds);
                }
            }

            int handValue = GetHandValue();

            if (handValue > 21)
            {
                Debug.Log($"Dealer busts with {handValue}");
                OnDealerBust.Invoke(handValue);
                yield break;
            }

            Debug.Log($"Dealer stands with {handValue}");
            OnDealerStand.Invoke(handValue);
        }

        private void FlipCardAtIndex(int cardIndex)
        {
            if (cardIndex < 0 || cardIndex >= dealerCards.Length || dealerCards[cardIndex] == null)
            {
                return;
            }

            if (dealerCards[cardIndex].flipped)
            {
                dealerCards[cardIndex].Flip();
            }

            if (cardGenerator == null || cardGenerator.targetParent == null)
            {
                return;
            }

            if (cardIndex < cardGenerator.targetParent.childCount)
            {
                Transform visualCardTransform = cardGenerator.targetParent.GetChild(cardIndex);
                Card visualCard = visualCardTransform.GetComponent<Card>();
                if (visualCard != null && visualCard.flipped)
                {
                    visualCard.Flip();
                }
            }

            if (cardCounter) {
                cardCounter.UpdateText(Hand.GetBestValue(dealerCards).ToString());
            }
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