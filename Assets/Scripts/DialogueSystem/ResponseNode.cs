using System;
using System.Collections.Generic;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class ResponseNode : DialogueNode
    {
        [SerializeField] private List<Response> responses;
        [SerializeField] private int selectedResponseIndex;

        public int ResponseIndex
        {
            get => selectedResponseIndex;
            set => selectedResponseIndex = value;
        }

        public override DialogueNode Child => responses[selectedResponseIndex]?.child;

        public IEnumerable<string> Responses => responses.ConvertAll(response => response.text);

        private void Awake()
        {
            Initialize();
        }

        public int AddResponse()
        {
            responses.Add(new Response());
            return responses.Count - 1;
        }

        public void SetText(string text, int index)
        {
            responses[index].text = text;
        }

        public void SetChild(DialogueNode node, int index)
        {
            responses[index].child = node;
        }
    }

    [Serializable]
    public class Response
    {
        public string text;
        public DialogueNode child;
    }
}
