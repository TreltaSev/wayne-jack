using UnityEngine;

namespace Game
{

    [RequireComponent(typeof(Interactable))]
    public class Hand : MonoBehaviour
    {
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

                if (card.flipped) {
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
    }
}
