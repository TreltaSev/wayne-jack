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

        public UnityEvent onNext = new();

        public UnityEvent onShow = new();
    }

}