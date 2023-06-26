using CesarJZO.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CesarJZO.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button nextButton;

        [Header("Simple Panel")]
        [SerializeField] private GameObject simplePanel;

        [Header("Response Panel")]
        [Tooltip("The panel that contains the responses.")]
        [SerializeField] private GameObject responsePanel;
        [Tooltip("The root that contains the response prefabs")]
        [SerializeField] private Transform responseRoot;
        [Tooltip("The prefab for the response buttons.")]
        [SerializeField] private GameObject responsePrefab;

        [Header("Item Conditional Panel")]
        [Tooltip("The panel that contains the inventory UI.")]
        [SerializeField] private GameObject itemConditionalPanel;

        private DialogueManager _dialogueManager;

        private void Start()
        {
            _dialogueManager = DialogueManager.Instance;

            if (!_dialogueManager)
                Debug.LogWarning("No <b>DialogueManager</b> found in scene.", this);

            _dialogueManager.ConversationUpdated += UpdateUI;
        }

        private void UpdateUI()
        {

        }
    }
}
