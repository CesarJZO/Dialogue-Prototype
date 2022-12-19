using UnityEngine;

namespace Interactable
{
    public class NpcInteractable : MonoBehaviour, IInteractable
    {
        public Transform parent;
        public void Interact()
        {
            Debug.Log($"Hi, I'm {parent.name}!");
        }
    }
}