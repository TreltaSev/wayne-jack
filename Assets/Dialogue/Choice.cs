using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    public class Choice : MonoBehaviour
    {
        public string title = "Choice";
        public string subtext = "Subtext";
        public UnityEvent onSelect = new();
    }
}