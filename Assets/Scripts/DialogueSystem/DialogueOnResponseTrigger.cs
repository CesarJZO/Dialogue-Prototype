using UnityEngine;
using UnityEngine.Events;

namespace CesarJZO.DialogueSystem
{
    public class DialogueOnResponseTrigger : MonoBehaviour
    {
        [SerializeField] private bool dequeueIfTrigger;
        [SerializeField] private string dequeueTrigger;

        [SerializeField] private UnityEvent<string> onResponseSelected;

        private DialogueNpc _dialogueNpc;

        private void Awake()
        {
            _dialogueNpc = GetComponent<DialogueNpc>();
        }

        public void OnResponseSelected(string response)
        {
            if (dequeueIfTrigger && response == dequeueTrigger)
                _dialogueNpc.DequeueDialogue();

            onResponseSelected?.Invoke(response);
        }
    }
}
