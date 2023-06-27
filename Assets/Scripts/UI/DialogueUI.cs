using CesarJZO.DialogueSystem;
using CesarJZO.Input;
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
        [SerializeField] private Button quitButton;


        [Header("Response Panel")]
        [Tooltip("The panel that contains the responses.")]
        [SerializeField] private GameObject responsePanel;
        [Tooltip("The root that contains the response prefabs")]
        [SerializeField] private Transform responseRoot;
        [Tooltip("The prefab for the response buttons.")]
        [SerializeField] private Button responsePrefab;

        [Header("Item Conditional Panel")]
        [Tooltip("The panel that contains the inventory UI.")]
        [SerializeField] private GameObject itemConditionalPanel;

        private DialogueManager _dialogueManager;

        private void Start()
        {
            _dialogueManager = DialogueManager.Instance;

            if (!_dialogueManager)
            {
                Debug.LogWarning("No <b>DialogueManager</b> found in scene.", this);
                return;
            }

            _dialogueManager.ConversationUpdated += UpdateUI;

            if (PlayerInput.Instance)
                PlayerInput.Instance.NextPerformed += PerformNextOrExit;

            nextButton.onClick.AddListener(_dialogueManager.Next);
            quitButton.onClick.AddListener(_dialogueManager.Quit);

            UpdateUI();
        }

        private void UpdateUI()
        {
            gameObject.SetActive(_dialogueManager.HasDialogue);

            if (!_dialogueManager.HasDialogue)
                return;

            speakerName.text = _dialogueManager.CurrentSpeaker
                ? _dialogueManager.CurrentSpeaker.DisplayName
                : "Speaker not set";
            text.text = _dialogueManager.CurrentText;

            DialogueNode currentNode = _dialogueManager.CurrentNode;

            nextButton.gameObject.SetActive(_dialogueManager.NextNode);
            quitButton.gameObject.SetActive(!_dialogueManager.NextNode);

            if (currentNode is ResponseNode responseNode)
                BuildResponses(responseNode);
        }

        private void BuildResponses(ResponseNode responseNode)
        {
            foreach (Transform item in responseRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (Response response in responseNode.Responses)
            {
                Button responseButton = Instantiate(responsePrefab, responseRoot);
                responseButton.GetComponentInChildren<TextMeshProUGUI>().text = response.text;
                responseButton.onClick.AddListener(() =>
                {
                    responseNode.CurrentResponse = response;
                    _dialogueManager.Next();
                });
            }
        }

        private void PerformNextOrExit()
        {
            if (_dialogueManager.NextNode)
                _dialogueManager.Next();
            else
                _dialogueManager.Quit();
        }
    }
}
