using UnityEngine;

namespace Dialogue
{
    public class ChoiceGroup : MonoBehaviour
    {
        // --- Choice Properties --- //
        public Choice[] choices;

        private void Awake()
        {
            // Load all choices in order
            choices = this.GetComponentsInChildren<Choice>();
        }

        // Display all choices available in group
        public void Display()
        {
            
        }
    }
}