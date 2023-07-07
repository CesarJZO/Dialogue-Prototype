using UnityEngine;
using UnityEngine.Events;

namespace CesarJZO.DialogueSystem
{
    [RequireComponent(typeof(DialogueNpc))]
    public class DialogueOnItemMatch : MonoBehaviour
    {
        [SerializeField] private bool dequeueIfItemMatches;

        public UnityEvent<bool> onItemMatch;

        private DialogueNpc _dialogueNpc;

        private void Awake()
        {
            _dialogueNpc = GetComponent<DialogueNpc>();
        }

        public void OnItemMatch(bool hadItem)
        {
            if (dequeueIfItemMatches && hadItem)
                _dialogueNpc.DequeueDialogue();

            onItemMatch?.Invoke(hadItem);
        }
    }
}
