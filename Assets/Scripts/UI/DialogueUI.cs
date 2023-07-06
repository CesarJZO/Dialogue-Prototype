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
        [SerializeField] private InventoryUI inventoryPanel;

        private DialogueManager _dialogueManager;

        private bool _textWarningShown;
        private bool _responseWarningShown;

        private void Start()
        {
            HideComponents();

            _dialogueManager = DialogueManager.Instance;

            if (!_dialogueManager)
            {
                Debug.LogWarning("No <b>DialogueManager</b> instance found.", this);
                return;
            }

            _dialogueManager.ConversationStarted += ResetUI;
            _dialogueManager.ConversationUpdated += UpdateUI;

            InventoryUI.ItemSelected += OnItemSelected;

            if (PlayerInput.Instance)
                PlayerInput.Instance.NextPerformed += TryPerformNextOrQuit;

            if (nextButton)
                nextButton.onClick.AddListener(TryPerformNextOrQuit);
            else
                Debug.Log("<b>Next Button</b> is not set. DialogueUI will try to use <b>PlayerInput</b> instead.", this);

            UpdateUI();
        }

        private void ResetUI()
        {
            HideComponents();
            leftSpeakerImage.sprite = null;
            rightSpeakerImage.sprite = null;
        }

        private void UpdateUI()
        {
            gameObject.SetActive(_dialogueManager.HasDialogue);

            if (!_dialogueManager.HasDialogue)
                return;

            UpdateSpeakerText();
            UpdateText();
            UpdatePortraitImage();
            UpdateResponseUI();
            UpdateItemConditionalUI();
            UpdateNextButton();
        }

        /// <summary>
        ///     Sets the item to the current Node if the current node is an <see cref="ItemConditionalNode"/>.
        ///     And then proceeds to the next node closing the inventory panel.
        /// </summary>
        /// <param name="item">The item to be set for validation.</param>
        private void OnItemSelected(Item item)
        {
            if (!_dialogueManager)
                return;
            if (!_dialogueManager.HasDialogue)
                return;
            if (_dialogueManager.CurrentNode is not ItemConditionalNode itemConditionalNode)
                return;

            itemConditionalNode.SetItem(item);

            _dialogueManager.Next();
        }

        /// <summary>
        ///     Updates the speaker text on UI.
        /// </summary>
        private void UpdateSpeakerText()
        {
            if (speakerName)
                speakerName.text = _dialogueManager.CurrentSpeaker
                    ? _dialogueManager.CurrentSpeaker.DisplayName
                    : "Speaker not set";
        }

        /// <summary>
        ///     Updates the dialogue text on UI. Throws a warning if no text is selected.
        /// </summary>
        private void UpdateText()
        {
            if (text)
            {
                text.text = _dialogueManager.CurrentText;
            }
            else if (!_textWarningShown)
            {
                Debug.LogWarning("No text ui selected for the dialogue UI.", this);
                _textWarningShown = true;
            }
        }

        /// <summary>
        ///     If current node is a <see cref="ResponseNode"/>, then build the response
        ///     buttons inside the response root.
        /// </summary>
        private void UpdateResponseUI()
        {
            if (!_dialogueManager) return;

            DialogueNode currentNode = _dialogueManager.CurrentNode;

            if (currentNode is ResponseNode responseNode)
                BuildResponses(responseNode);

            if (responseRoot)
            {
                responseRoot.gameObject.SetActive(currentNode is ResponseNode);
            }
            else if (currentNode is ResponseNode && !_responseWarningShown)
            {
                Debug.LogWarning("No response root selected for the dialogue UI.", this);
                _responseWarningShown = true;
            }
        }

        /// <summary>
        ///     Put response buttons in the response root.
        /// </summary>
        /// <param name="responseNode"></param>
        private void BuildResponses(ResponseNode responseNode)
        {
            if (!responseRoot) return;
            foreach (Transform item in responseRoot)
            {
                Destroy(item.gameObject);
            }

            if (!responsePrefab)
            {
                Debug.Log("No response prefab selected for the dialogue UI.", this);
                return;
            }

            foreach (Response response in responseNode.Responses)
            {
                Button responseButton = Instantiate(responsePrefab, responseRoot);
                var buttonText = responseButton.GetComponentInChildren<TextMeshProUGUI>();
                if (!buttonText)
                {
                    Debug.LogWarning("No text component found on the response button.", this);
                    return;
                }
                buttonText.text = response.text;
                responseButton.onClick.AddListener(() =>
                {
                    responseNode.CurrentResponse = response;
                    _dialogueManager.Next();
                });
            }
        }

        /// <summary>
        ///     Updates the portrait image for the given side and speaker.
        /// </summary>
        private void UpdatePortraitImage()
        {
            Speaker speaker = _dialogueManager.CurrentSpeaker;
            if (!speaker) return;

            Image portraitImage = _dialogueManager.CurrentSide is PortraitSide.Left
                ? leftSpeakerImage
                : rightSpeakerImage;
            if (!portraitImage) return;

            portraitImage.gameObject.SetActive(true);

            portraitImage.sprite = _dialogueManager.CurrentEmotion switch
            {
                _ => speaker.NeutralSprite
            };
        }

        /// <summary>
        ///     Opens the inventory panel if the current node is an <see cref="ItemConditionalNode"/>.
        /// </summary>
        private void UpdateItemConditionalUI()
        {
            if (_dialogueManager.CurrentNode is not ItemConditionalNode) return;

            if (inventoryPanel)
                inventoryPanel.Open();
            else
                Debug.LogWarning("No inventory panel selected for the dialogue UI.", this);
        }

        /// <summary>
        ///     Updates the next button to display "Next" if there is another node, or "Quit" if there is not.
        ///     Also, hides the next button if the player is choosing a response.
        /// </summary>
        private void UpdateNextButton()
        {
            if (!nextButton) return;
            var buttonText = nextButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText)
                buttonText.text = _dialogueManager.HasNextNode ? "Next" : "Quit";
            nextButton.gameObject.SetActive(!_dialogueManager.Prompting);
        }

        /// <summary>
        ///     If the player is choosing, then the next button will be disabled. Otherwise,
        ///     it will perform Next() if there is a next node, or Quit() if there is not.
        /// </summary>
        private void TryPerformNextOrQuit()
        {
            if (_dialogueManager.Prompting)
                return;
            if (_dialogueManager.HasNextNode)
                _dialogueManager.Next();
            else
                _dialogueManager.Quit();
        }

        /// <summary>
        ///     Hides basic components of the UI, such as the speaker images and the dialogue UI itself.
        /// </summary>
        private void HideComponents()
        {
            if (leftSpeakerImage)
                leftSpeakerImage.gameObject.SetActive(false);
            if (rightSpeakerImage)
                rightSpeakerImage.gameObject.SetActive(false);

            gameObject.SetActive(false);
        }
    }
}
