using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{   

    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(HoverController))]
    public class HandView : MonoBehaviour
    {

        // ! === Static Properties === //
        private Interactable interactable;
        private HoverController hoverController;

        [SerializeField] private Game.CardGenerator cardGenerator;

        // ! === Dynamic Properties === //

        // ? --- Selection Logic --- //
        private Hoverable hovered;
        private int index = 0;

        // ! === Base Listeners === // 
        void Awake()
        {

            // ? --- Interactable ---
            interactable = GetComponent<Interactable>();
            hoverController = GetComponent<HoverController>();
            if (hoverController != null) hoverController.LoadItems();
        }

        void OnEnable()
        {
            if (interactable == null) return;
            interactable.OnUp += MoveUp;
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
            interactable.OnUp -= MoveUp;
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

        private void MoveUp()
        {
            if (!interactable.interacting) return;
            if (hoverController == null) return;
            hoverController.MoveUp();
        }

        private void Select()
        {
            if (!interactable.interacting) return;
            if (hoverController != null) hoverController.InvokeSelect();

            Player player = FindFirstObjectByType<Player>();
            player.ResetAll();
        }

        private void Light()
        {
            if (!interactable.interacting) return;
            if (hoverController != null) hoverController.InvokeLight();

            Player player = FindFirstObjectByType<Player>();
            player.DealCard();
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
        }

    }
}
