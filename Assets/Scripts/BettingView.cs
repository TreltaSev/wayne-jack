using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{   

    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(HoverController))]
    public class BettingView : MonoBehaviour
    {

        // ! === Static Properties === //
        public BlackjackRoundManager blackjackRoundManager;

        // ? --- Interactions --- //
        private Interactable interactable;
        private HoverController hoverController;

        // ? --- Chips --- //
        private Chips chips;
        private Chip chip;

        // ? --- Prefabs --- //
        public GameObject chipViewPrefab;

        // ? --- Player --- //
        Player player;

        // ? --- Bet --- //
        private int pendingBet;

        // ! === Dynamic Properties === //

        // ? --- Selection Logic --- //
        private Hoverable hovered;
        private GameObject hoveredObject;
        private int index = 0;

        // ! === Loading Logic === //

        public void AddChipView(string text, string denomination)
        {
            GameObject chipViewObject = Instantiate(chipViewPrefab, transform);        
            ChipView chipView = chipViewObject.GetComponent<ChipView>();
            chipView.denomination = denomination;
            chipView.UpdateText(text);
        }

        // ! === Base Listeners === // 

        void Start()
        {
             hoverController.LoadItems();
               RefreshPendingBetDisplay();
        }

        void Awake()
        {

            // ? --- Interactable ---
            interactable = GetComponent<Interactable>();
            hoverController = GetComponent<HoverController>();
            if (hoverController != null) hoverController.LoadItems();

            // ? --- Chips ---
            chips = FindFirstObjectByType<Chips>();
            chip = GetComponent<Chip>();

            // ? --- Player ---
            player = FindFirstObjectByType<Player>();
        }

        void OnEnable()
        {
            if (interactable == null) return;
            interactable.OnDown += MoveDown;
            interactable.OnRight += MoveRight;
            interactable.OnLeft += MoveLeft;

            interactable.OnSelect += Select;

            interactable.OnLight += Light;
            interactable.OnHeavy += Heavy;
            if (hoverController != null)
            {
                hoverController.OnHoverChanged += HandleHoverChanged;
            }
        }

        void OnDisable()
        {
            if (interactable == null) return;
            interactable.OnDown -= MoveDown;
            interactable.OnRight -= MoveRight;
            interactable.OnLeft -= MoveLeft;

            interactable.OnSelect -= Select;

            interactable.OnLight -= Light;
            interactable.OnHeavy -= Heavy;
            if (hoverController != null)
            {
                hoverController.OnHoverChanged -= HandleHoverChanged;
            }
        }


        // ! === Input Listeners === //

        private void MoveRight()
        {
            if (!interactable.interacting) return;
            if (hoverController == null) return;
            if (hoverController.MoveRight())
            {
                index = hoverController.index;
                hovered = hoverController.hovered;
            }
        }

        private void MoveLeft()
        {
            if (!interactable.interacting) return;
            if (hoverController == null) return;
            if (hoverController.MoveLeft())
            {
                index = hoverController.index;
                hovered = hoverController.hovered;
            }
        }

        private void MoveDown()
        {
            if (!interactable.interacting) return;
            if (hoverController == null) return;
            hoverController.MoveDown();

            if (pendingBet > 0)
            {
                blackjackRoundManager.SetBetAmount(pendingBet);
                blackjackRoundManager.ConfirmBetAndStartRound();
                pendingBet = 0;
                RefreshPendingBetDisplay();
            }
        }

        private void Select()
        {
            if (!interactable.interacting) return;
            if (hoverController != null) hoverController.InvokeSelect();

            Chip selectedChip = hoveredObject.GetComponent<Chip>();
            Debug.Log($"Selected Chip: {selectedChip.denomination} {selectedChip.Id}");

            int potentialBet = pendingBet + selectedChip.denomination;
            if (potentialBet > player.balance) {
                Debug.Log($"Insufficient balance to add chip {selectedChip.denomination}. Pending: {pendingBet} Balance: {player.balance}");
                return;
            }

            pendingBet = potentialBet;
            Debug.Log($"Potential Bet Updated: {pendingBet}");
            RefreshPendingBetDisplay();
        }

        private void Light()
        {
            if (!interactable.interacting) return;
            if (hoverController != null) hoverController.InvokeLight();
        }

        private void Heavy()
        {
            if (!interactable.interacting) return;
            if (hoverController != null) hoverController.InvokeHeavy();
        }

        // ! === Hover Logic === //
        private void HandleHoverChanged(int newIndex, Hoverable newHover)
        {
            index = newIndex;
            hovered = newHover;
            hoveredObject = hoverController != null ? hoverController.GetHoveredObject(newIndex) : null;
        }

        private void RefreshPendingBetDisplay()
        {
            if (player == null) return;

            player.pendingBet = pendingBet;
            player.OnPendingBetChange(pendingBet);
        }

    }
}
