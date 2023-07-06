﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
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
        public override bool TryRemoveChild(DialogueNode node)
        {
            Response response = responses.FirstOrDefault(r => r.child == node);

            if (response is null) return false;

            responses.Remove(response);
            return true;
        }

        public void UnlinkChild(int index)
        {
            responses[index].child = null;
        }

        public IEnumerable<Response> Responses => responses;

        public int ChildrenCount => responses.Count;

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

        public string GetText(int index)
        {
            return responses[index].text;
        }

        public void SetChild(DialogueNode node, int index)
        {
            responses[index].child = node;
        }

        public DialogueNode GetChild(int index)
        {
            return responses[index].child;
        }

        private void Awake()
        {
            responses ??= new List<Response>();
        }
    }

    [Serializable]
    public class Response
    {
        public string text;
        public DialogueNode child;
    }
}
