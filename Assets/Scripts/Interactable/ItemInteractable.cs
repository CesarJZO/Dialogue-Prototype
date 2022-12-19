using UnityEngine;
using Player;

namespace Interactable
{
    public class ItemInteractable : MonoBehaviour, IInteractable
    {
        public PlayerInventory playerInventory;

        public void Interact()
        {
            playerInventory.AddItem(gameObject);
            Destroy(gameObject);
        }
    }
}
