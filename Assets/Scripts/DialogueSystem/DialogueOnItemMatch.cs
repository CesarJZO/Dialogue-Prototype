using UnityEngine;
using UnityEngine.Events;

namespace CesarJZO.DialogueSystem
{
    [RequireComponent(typeof(DialogueNpc))]
    public class DialogueOnItemMatch : MonoBehaviour
    {
        [SerializeField] private bool dequeueIfItemMatches;

        [SerializeField] private UnityEvent onItemMatch;
        [SerializeField] private UnityEvent onItemMismatch;

        private DialogueNpc _dialogueNpc;

        private void Awake()
        {
            _dialogueNpc = GetComponent<DialogueNpc>();
        }

        private void OnItemMatch()
        {
            if (dequeueIfItemMatches)
                _dialogueNpc.DequeueDialogue();

            onItemMatch?.Invoke();
        }

        private void OnItemMismatch()
        {
            onItemMismatch?.Invoke();
        }

        public void OnConditionalNodeEvaluated(bool hadItem)
        {
            if (hadItem)
                OnItemMatch();
            else
                OnItemMismatch();
        }
    }
}
