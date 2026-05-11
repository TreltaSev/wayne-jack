using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        // ! === Game Manager === //
        GameManager gameManager;

        // === TextLink Components === //
        [SerializeField] private TextLink nameplate_object;
        [SerializeField] private TextLink content_object;

        // === Dialog UI === //
        public GameObject dialogueUI;

        // === Dialog Items === //
        public DialogueItem[] dialogueItems;

        // --- Dialogue Properties --- //
        public int currentDialogueItemIndex = 0;
        private DialogueItem currentDialogueItem;

        private readonly UnityEvent<int> attemptDialogueUpdate = new();
        private readonly UnityEvent successfulDialogueUpdate = new();

        public bool typing = false; 
        [SerializeField] private float typingCharacterDelay = 0.03f;
        private Coroutine typingCoroutine;


        void Awake()
        {
            // Get Manager
            gameManager = FindFirstObjectByType<GameManager>();
            
            
            // Load all dialogue items in order
            dialogueItems = this.GetComponentsInChildren<DialogueItem>();

            this.attemptDialogueUpdate.AddListener(AttemptDialogueUpdate);
            this.successfulDialogueUpdate.AddListener(UpdateDialogue);            
        }

        private void Start()
        {
            attemptDialogueUpdate.Invoke(currentDialogueItemIndex);
            
            // * Handle Dialog Interactions ( pressing btn:south/click:left/enter )
            gameManager.onSelectDialogue.AddListener(HandleDialogInteract);
        }

        private void OnDestroy()
        {
            gameManager.onSelectDialogue.RemoveListener(HandleDialogInteract);

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
        }

        public void SetDialogue(DialogueItem dialogueItem)
        {
            currentDialogueItem = dialogueItem;
            currentDialogueItemIndex = dialogueItem.relativeIndex;
            UpdateDialogue();
        }

        void HandleDialogInteract()
        {
            if (typing)
            {
                CompleteTyping();
                return;
            }

            this.Next();
        }

        /// <summary>
        /// Attempts to update the current dialogue item.
        /// If everything checks out, this calls UpdateDialog
        /// </summary>
        /// <param name="new_index"></param>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        private void AttemptDialogueUpdate(int new_index)
        {
            if (currentDialogueItem && !currentDialogueItem.default_next)
            {
                currentDialogueItem.onNext.Invoke();
                return;
            }
            ;
            if (new_index < 0 || new_index > (dialogueItems.Length + 1)) throw new System.IndexOutOfRangeException();
            this.currentDialogueItem = dialogueItems[new_index];
            this.currentDialogueItemIndex = new_index;
            this.successfulDialogueUpdate.Invoke();
        }

        /// <summary>
        /// Updates the dialogue by updating the values in the nameplate and content
        /// </summary>
        private void UpdateDialogue()
        {
            if (currentDialogueItem)
            {
                if (currentDialogueItem.relativeIndex != currentDialogueItemIndex)
                {
                    currentDialogueItem.onNext.Invoke();                    
                }
            }
            ;
            DialogueItem dialogueItem = dialogueItems[currentDialogueItemIndex];
            currentDialogueItem = dialogueItem;
            dialogueItem.onShow.Invoke();

            // Make the nameplate refresh its sizing
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)nameplate_object.transform.parent);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)nameplate_object.transform);

            

            nameplate_object.TrySetTransmitterContent(dialogueItem.talker);

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            typingCoroutine = StartCoroutine(TypeDialogueContent(dialogueItem.content));
        }

        private IEnumerator TypeDialogueContent(string fullContent)
        {
            typing = true;

            string safeContent = fullContent ?? string.Empty;
            content_object.TrySetTransmitterContent(string.Empty);

            for (int i = 1; i <= safeContent.Length; i++)
            {
                content_object.TrySetTransmitterContent(safeContent.Substring(0, i));
                yield return new WaitForSeconds(typingCharacterDelay);
            }

            typing = false;
            typingCoroutine = null;
        }

        private void CompleteTyping()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            string fullContent = currentDialogueItem ? currentDialogueItem.content : string.Empty;
            content_object.TrySetTransmitterContent(fullContent ?? string.Empty);
            typing = false;
        }

        /// <summary>
        /// Editor Only
        /// </summary>
        void OnValidate()
        {
            attemptDialogueUpdate.Invoke(currentDialogueItemIndex);
        }

        public void Next()
        {
            attemptDialogueUpdate.Invoke(currentDialogueItemIndex + 1);
        }

        /// <summary>
        /// Disables the dialogue and dialog ui
        /// </summary>
        public void Disable()
        {
            Debug.Log("Disabling Dialogue & Dialogue UI");
            dialogueUI.SetActive(false);
        }

        public void Enable()
        {
            Debug.Log("ENabling Dialogue & Dialogue UI");
            dialogueUI.SetActive(true);
        }
    }

}