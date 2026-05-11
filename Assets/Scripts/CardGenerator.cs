using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class CardGenerator : MonoBehaviour
    {

        // ! === Dynamic Properties --- //
        public Transform targetParent;

        // ? --- Prefabs --- //
        public GameObject cardPrefab;       

        // ! === Events === //
        public UnityEvent<Card> OnCreate = new();
        public UnityEvent OnDelete = new();

        
        /// <summary>
        /// Adds a card prefab into the target parent
        /// </summary>
        public void Create(Face face, Glyph glyph, bool flipped = false)
        {
            Debug.Log($"Creating New Card of face {face} and glyph {glyph} on {targetParent}");
            GameObject newObject = Instantiate(cardPrefab, targetParent);

            Card newCard = newObject.GetComponent<Card>();
            newCard.face = face;
            newCard.glyph = glyph;
            newCard.flipped = flipped;
            newCard.RefreshSelf();

            this.OnCreate.Invoke(newCard);
        }

        public void DeleteAll()
        {
            Transform parent = targetParent != null ? targetParent : this.transform;
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                GameObject child = parent.GetChild(i).gameObject;
                if (Application.isPlaying)
                    Destroy(child);
                else
                    DestroyImmediate(child);
            }

            OnDelete.Invoke();
            
        }
    }
}
