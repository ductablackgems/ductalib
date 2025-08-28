using UnityEngine;
using UnityEngine.Events;

namespace _0.DucTALib.Gameplay
{
    public class ColliderHelper : MonoBehaviour
    {
        public string detectTag;
        public UnityEvent onTriggerEnter;
        public UnityEvent onTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == detectTag && onTriggerEnter != null)
            {
                onTriggerEnter?.Invoke();
            }
        }
    }
}