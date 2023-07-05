﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Response Node", menuName = "Dialogue/Response Node", order = 2)]
    public class ResponseNode : DialogueNode
    {
        [SerializeField] private List<Response> responses;
        [SerializeField, Min(0f)] private float timeLimit;
        public float TimeLimit => timeLimit;

        private Response _currentResponse;

        public Response CurrentResponse
        {
            set => _currentResponse = value;
            get => _currentResponse;
        }

        public override DialogueNode Child => _currentResponse?.child;

        public override bool IsChild(DialogueNode node)
        {
            return responses.Any(response => response.child == node);
        }

        public override void RemoveChild(DialogueNode node)
        {
            responses.RemoveAll(response => response.child == node);
        }

        public IEnumerable<Response> Responses => responses;

        public int AddResponse()
        {
            responses.Add(new Response());
            return responses.Count - 1;
        }

        public int RemoveResponse()
        {
            responses.RemoveAt(responses.Count - 1);
            return responses.Count;
        }

        public void SetText(string text, int index)
        {
            responses[index].text = text;
        }

        public void SetChild(DialogueNode node, int index)
        {
            responses[index].child = node;
        }

        public DialogueNode GetChild(int index)
        {
            return responses[index].child;
        }
    }

    [Serializable]
    public class Response
    {
        public string text;
        public DialogueNode child;
    }
}
