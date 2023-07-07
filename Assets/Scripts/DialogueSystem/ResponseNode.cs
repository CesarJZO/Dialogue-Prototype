using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    public class ResponseNode : DialogueNode
    {
        [SerializeField] [Min(0f)] private float timeLimit;

        [SerializeField] private List<Response> responses;

        public float TimeLimit
        {
            get => timeLimit;
#if UNITY_EDITOR
            set
            {
                timeLimit = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public Response CurrentResponse { set; get; }

        public override DialogueNode Child => CurrentResponse?.child;

        public IEnumerable<Response> Responses => responses;

        public int ChildrenCount => responses.Count;

        public string GetText(int index)
        {
            return responses[index].text;
        }

        public DialogueNode GetChild(int index)
        {
            return responses[index].child;
        }

#if UNITY_EDITOR
        private void Awake()
        {
            responses ??= new List<Response>();
        }

        public void UnlinkChild(int index)
        {
            responses[index].child = null;
            EditorUtility.SetDirty(this);
        }

        public int AddResponse()
        {
            responses.Add(new Response());
            EditorUtility.SetDirty(this);
            return responses.Count - 1;
        }

        public int RemoveResponse()
        {
            responses.RemoveAt(responses.Count - 1);
            EditorUtility.SetDirty(this);
            return responses.Count;
        }

        public void SetText(string text, int index)
        {
            responses[index].text = text;
            EditorUtility.SetDirty(this);
        }

        public void SetChild(DialogueNode node, int index)
        {
            responses[index].child = node;
            EditorUtility.SetDirty(this);
        }

#endif
        public override bool TryRemoveChild(DialogueNode node)
        {
            Response response = responses.FirstOrDefault(r => r.child == node);

            if (response is null) return false;
#if UNITY_EDITOR
            response.child = null;
            EditorUtility.SetDirty(this);
#endif
            return true;
        }
    }

    [Serializable]
    public class Response
    {
        public string text;
        [HideInInspector, SerializeField] public DialogueNode child;
    }
}
