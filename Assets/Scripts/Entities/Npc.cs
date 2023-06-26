using CesarJZO;
using CesarJZO.DialogueSystem;
using UnityEngine;

namespace Entities
{
    public class Npc : MonoBehaviour, IInteractable
    {
        [SerializeField] private Dialogue dialogue;

        public void Interact()
        {
            if (!dialogue) return;
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}
