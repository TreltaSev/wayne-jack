using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{  

    /// <summary>
    /// Nameplate object, contains method to get and update the current "speaker"
    /// </summary>
    public class Nameplate : MonoBehaviour
    {  
        // === TALKER === //
        [SerializeField]
        [Tooltip("Name of the person currently talking.")]
        private string talker = "Talker";
        
        // --- Events ---
        public UnityEvent<string> e_talker_change;        

        public string GetTalker()
        {
            return talker;
        }

        public void SetTalker(string new_talker)
        {
            // Set Talker
            this.talker = new_talker;

            // Announce Change
            e_talker_change.Invoke(new_talker);
        }
    }

}