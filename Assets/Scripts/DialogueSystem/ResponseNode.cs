using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class ResponseNode : DialogueNode
    {
        [SerializeField] [Min(0f)] private float timeLimit;

        private List<Response> _responses;

        public float TimeLimit => timeLimit;

        public Response CurrentResponse { set; get; }

        public override DialogueNode Child => CurrentResponse?.child;

        public IEnumerable<Response> Responses => _responses;

        public int ChildrenCount => _responses.Count;

        private void Awake()
        {
            _responses ??= new List<Response>();
        }

        public override bool TryRemoveChild(DialogueNode node)
        {
            Response response = _responses.FirstOrDefault(r => r.child == node);

            if (response is null) return false;

            _responses.Remove(response);
            return true;
        }

        public void UnlinkChild(int index)
        {
            _responses[index].child = null;
        }

        public int AddResponse()
        {
            _responses.Add(new Response());
            return _responses.Count - 1;
        }

        public int RemoveResponse()
        {
            _responses.RemoveAt(_responses.Count - 1);
            return _responses.Count;
        }

        public void SetText(string text, int index)
        {
            _responses[index].text = text;
        }

        public string GetText(int index)
        {
            return _responses[index].text;
        }

        public void SetChild(DialogueNode node, int index)
        {
            _responses[index].child = node;
        }

        public DialogueNode GetChild(int index)
        {
            return _responses[index].child;
        }
    }

    [Serializable]
    public class Response
    {
        public string text;
        public DialogueNode child;
    }
}
