using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem
{
    public class Dialogue : MonoBehaviour
    {
        public abstract class Lines
        {
            public string[] lines;
            public override string ToString()
            {
                var s = string.Empty;
                foreach (var line in lines)
                {
                    s += line;
                }

                return s;
            }
        }
        public TMP_Text textComponent;
        public Lines lines;
        public float textSpeed;

        private int _index;

        private void OnEnable()
        {
            textComponent.text = string.Empty;
            StartDialogue();
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (textComponent.text == lines.lines[_index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines.lines[_index];
                }
            }
        }

        private void StartDialogue()
        {
            _index = 0;
            StartCoroutine(TypeLine());
        }

        private IEnumerator TypeLine()
        {
            foreach (var c in lines.lines[_index].ToCharArray())
            {
                textComponent.text += c;
                if (c == ' ')
                    yield return null;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        private void NextLine()
        {
            if (_index < lines.lines.Length - 1)
            {
                _index++;
                textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
