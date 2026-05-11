using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Hoverable : MonoBehaviour
    {   

        // ! === Events === //
        public UnityEvent OnHoverEnter = new();
        public UnityEvent OnHoverLeave = new();

        public void Enter()
        {
            OnHoverEnter.Invoke();
        }

        public void Leave()
        {
            OnHoverLeave.Invoke();
        }
    }
}
