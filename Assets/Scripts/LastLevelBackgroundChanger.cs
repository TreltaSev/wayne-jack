using UnityEngine;
using UnityEngine.UI;


namespace Game
{

    public class LastLevelBackgroundChanger : MonoBehaviour
    {
        private Player player;

        public GameObject backgroundObject;

        public Sprite spriteCrackedOne;
        public Sprite spriteCrackedTwo;
        public Sprite spriteCrackedThree;

        void Awake()
        {
            this.player = FindFirstObjectByType<Player>();
            this.player.e_balance_change.AddListener(OnBalanceChange);
        }


        /// <summary>
        /// We just want to listen for certain triggers here... honestly nothing too crazy
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="result"></param>
        void OnBalanceChange(float offset, float result)
        {

            Image backgroundImage = backgroundObject.GetComponent<Image>();

            if (result > 110000)
            {
                backgroundImage.sprite = spriteCrackedOne;
            }
            else if (result > 130000)
            {
                backgroundImage.sprite = spriteCrackedTwo;

            }
            else if (result > 140000)
            {
                backgroundImage.sprite = spriteCrackedThree;
            }
        }
    }

}