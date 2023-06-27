using CesarJZO.DialogueSystem;
using CesarJZO.Input;
using CesarJZO.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CesarJZO.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button quitButton;

        [Header("Speaker")]
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private Image leftSpeakerImage;
        [SerializeField] private Image rightSpeakerImage;

        [Header("Response Panel")]
        [Tooltip("The root that contains the response prefabs")]
        [SerializeField] private Transform responseRoot;
        [Tooltip("The prefab for the response buttons.")]
        [SerializeField] private Button responsePrefab;

        [Header("Inventory Panel")]
        [Tooltip("The panel that contains the inventory UI.")]
        [SerializeField] private GameObject inventoryPanel;

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

            nextButton.onClick.AddListener(TryPerformNext);
            quitButton.onClick.AddListener(TryPerformQuit);

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

            UpdatePortraitImage(
                side: _dialogueManager.CurrentSide,
                speaker: _dialogueManager.CurrentSpeaker,
                emotion: _dialogueManager.CurrentEmotion
            );

            text.text = _dialogueManager.CurrentText;

            if (_dialogueManager.CurrentNode is ResponseNode responseNode)
                BuildResponses(responseNode);
            responseRoot.gameObject.SetActive(_dialogueManager.Choosing);

            if (_dialogueManager.CurrentNode is ItemConditionalNode)
            {
                inventoryPanel.SetActive(true);
            }

            nextButton.gameObject.SetActive(_dialogueManager.NextNode && !_dialogueManager.Choosing);
            quitButton.gameObject.SetActive(!_dialogueManager.NextNode && !_dialogueManager.Choosing);
        }

        public void CheckItem(Item item)
        {
            if (_dialogueManager.CurrentNode is not ItemConditionalNode itemConditionalNode)
                return;

            itemConditionalNode.SetItem(item);
            inventoryPanel.SetActive(false);

            _dialogueManager.Next();
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
            if (_dialogueManager.NextNode)
                TryPerformNext();
            else
                TryPerformQuit();
        }

        private void TryPerformNext()
        {
            if (_dialogueManager.Choosing)
                return;
            _dialogueManager.Next();
        }

        private void TryPerformQuit()
        {
            if (_dialogueManager.Choosing)
                return;
            _dialogueManager.Quit();
        }

        /// <summary>
        ///     Updates the portrait image for the given side and speaker.
        /// </summary>
        /// <param name="side">The side of the dialogue UI where the speaker should be displayed.</param>
        /// <param name="speaker">The speaker to display.</param>
        /// <param name="emotion">The emotion to select the sprite for.</param>
        private void UpdatePortraitImage(Speaker speaker, DialogueNode.Side side, Emotion emotion = Emotion.Neutral)
        {
            if (!speaker) return;

            Image portraitImage = side is DialogueNode.Side.Left ? leftSpeakerImage : rightSpeakerImage;

            if (!portraitImage) return;

            portraitImage.sprite = emotion switch
            {
                _ => speaker.NeutralSprite
            };
        }
    }
}
