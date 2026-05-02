using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Dialogue
{
    public class DialogueInput : MonoBehaviour
    {

        // === GAME STATE === //
        private GameManager gameManager;

        // === INPUTS === //

        // --- Menu Select --- //
        private InputAction m_menuSelectAction;        
        [Tooltip("Events that will be called whenever the user selects something and the current game state is set to Menu")]
        public UnityEvent e_menuSelectAction;

        

        private void Awake()
        {
            // Game State
            gameManager = FindFirstObjectByType<GameManager>();
            
            // Inputs
            m_menuSelectAction = InputSystem.actions.FindAction("MenuSelect");
            
        }

        private void Update()
        {
            // Handle All Inputs
            HandleMenuSelect();
        }


        private void HandleMenuSelect()
        {
            if (gameManager.state != State.Menu) return;
            if (!m_menuSelectAction.WasPressedThisFrame()) return;
            e_menuSelectAction.Invoke();
        }


    }

}