using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Dialogue
{
    public class ChoiceGroup : MonoBehaviour
    {

        // ! === GAME MANAGER === //
        GameManager gameManager;

        // --- Choice Properties --- //
        public Choice[] choices;
        public int selectedIndex = 0;

        // --- Events --- //
        public UnityEvent onShow = new();
        public UnityEvent onHide = new();


        private void Awake()
        {
            // Game Manager
            gameManager = FindFirstObjectByType<GameManager>();

            // Load all choices in order
            choices = this.GetComponentsInChildren<Choice>();
        }

        private void OnEnable()
        {

            // Initial Events
            onShow.Invoke();

            // Inputs
            gameManager.onSelectMenu.AddListener(OnSelect);
            gameManager.onMoveUpMenu.AddListener(OnMoveUp);
            gameManager.onMoveDownMenu.AddListener(OnMoveDown);

            // Selecting First Item
            Button choiceButton = choices[selectedIndex].gameObject.GetComponent<Button>();
            choiceButton.Select();
        }

        private void OnDisable()
        {

            // Call Events
            onHide.Invoke();

            // Inputs
            gameManager.onSelectMenu.RemoveListener(OnSelect);
            gameManager.onMoveUpMenu.RemoveListener(OnMoveUp);
            gameManager.onMoveDownMenu.RemoveListener(OnMoveDown);

            // Reset Selected
            selectedIndex = 0;
        }


        private void OnSelect()
        {
            Button choiceButton = choices[selectedIndex].gameObject.GetComponent<Button>();
            choiceButton.onClick.Invoke();
        }

        private void OnMoveUp()
        {
            if ((selectedIndex - 1) < 0) return;
            selectedIndex--;
            Button choiceButton = choices[selectedIndex].gameObject.GetComponent<Button>();
            choiceButton.Select();
        }

        private void OnMoveDown()
        {
            if ((selectedIndex + 1) > choices.Length) return;
            selectedIndex++;
            Button choiceButton = choices[selectedIndex].gameObject.GetComponent<Button>();
            choiceButton.Select();
        }




        // Display all choices available in group
        public void Display()
        {

        }
    }
}