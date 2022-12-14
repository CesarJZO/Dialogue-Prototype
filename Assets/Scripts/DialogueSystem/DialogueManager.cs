using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private float textSpeed;
        
        public GameObject container;
        public TMP_Text nameText;
        public TMP_Text dialogueText;
        
        private Queue<string> _sentences;

        private void Start()
        {
            _sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            container.SetActive(true);
            nameText.text = dialogue.name;
            
            _sentences.Clear();
            foreach (var sentence in dialogue.sentences)
            {
                _sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            var sentence = _sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }

        private IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = string.Empty;
            foreach (var c in sentence.ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }
        
        private void EndDialogue()
        {
            container.SetActive(false);
        }
    }
}