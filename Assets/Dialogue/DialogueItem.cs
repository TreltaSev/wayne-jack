using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    public class DialogueItem : MonoBehaviour
    {
        public string talker = "Talker";
        public string description = "Dialogue Description";
        public string content = "Long Dialogue Text";
        public bool default_next = true;

        public int relativeIndex;

        void Awake()
        {
            DialogueItem[] dialogueItems = transform.parent.GetComponentsInChildren<DialogueItem>();
            relativeIndex = System.Array.IndexOf(dialogueItems, this);
        }

        public UnityEvent onNext = new();

        public UnityEvent onShow = new();
    }

}