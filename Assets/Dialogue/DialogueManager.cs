using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {


        // === TextLink Components === //
        [SerializeField] private TextLink nameplate_object;
        [SerializeField] private TextLink content_object;

        // === Dialog Items === //
        public DialogueItem[] dialogueItems;

        // --- Dialogue Properties --- //
        public int currentDialogueItemIndex = 0;
        private DialogueItem currentDialogueItem;

        private readonly UnityEvent<int> attemptDialogueUpdate = new();
        private readonly UnityEvent successfulDialogueUpdate = new();


        void Awake()
        {

            // Load all dialogue items in order
            dialogueItems = this.GetComponentsInChildren<DialogueItem>();

            this.attemptDialogueUpdate.AddListener(AttemptDialogueUpdate);
            this.successfulDialogueUpdate.AddListener(UpdateDialogue);
        }

        void Start()
        {
            attemptDialogueUpdate.Invoke(currentDialogueItemIndex);
        }

        /// <summary>
        /// Attempts to update the current dialogue item.
        /// If everything checks out, this calls UpdateDialog
        /// </summary>
        /// <param name="new_index"></param>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        private void AttemptDialogueUpdate(int new_index)
        {
            if (new_index < 0 || new_index > (dialogueItems.Length + 1)) throw new System.IndexOutOfRangeException();
            this.currentDialogueItemIndex = new_index;
            this.currentDialogueItem = dialogueItems[new_index];
            this.successfulDialogueUpdate.Invoke();
        }

        /// <summary>
        /// Updates the dialogue by updating the values in the nameplate and content
        /// </summary>
        private void UpdateDialogue()
        {
            DialogueItem dialogueItem = dialogueItems[currentDialogueItemIndex];
            nameplate_object.TrySetTransmitterContent(dialogueItem.talker);
            content_object.TrySetTransmitterContent(dialogueItem.content);
        }

        /// <summary>
        /// Editor Only
        /// </summary>
        void OnValidate()
        {
            attemptDialogueUpdate.Invoke(currentDialogueItemIndex);
        }

        void Next()
        {
            attemptDialogueUpdate.Invoke(currentDialogueItemIndex + 1);
        }
    }

}