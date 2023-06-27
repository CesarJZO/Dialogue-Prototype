using System.Collections.Generic;
using System.Linq;
using CesarJZO.DialogueSystem;
using CesarJZO.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
                PlayerInput.Instance.NextPerformed += TryPerformNextOrExit;

            nextButton.onClick.AddListener(TryPerformNextOrExit);
            quitButton.onClick.AddListener(TryPerformNextOrExit);

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

            if (_dialogueManager.CurrentNode is ResponseNode responseNode)
                BuildResponses(responseNode);
            responseRoot.gameObject.SetActive(_dialogueManager.Choosing);

            nextButton.gameObject.SetActive(_dialogueManager.NextNode && !_dialogueManager.Choosing);
            quitButton.gameObject.SetActive(!_dialogueManager.NextNode && !_dialogueManager.Choosing);
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

        /// <summary>
        ///     If the player is choosing, then the next button will be disabled. Otherwise,
        ///     it will perform Next() if there is a next node, or Quit() if there is not.
        /// </summary>
        private void TryPerformNextOrExit()
        {
            if (_dialogueManager.Choosing)
                return;

            if (_dialogueManager.NextNode)
                _dialogueManager.Next();
            else
                _dialogueManager.Quit();
        }
    }
}
