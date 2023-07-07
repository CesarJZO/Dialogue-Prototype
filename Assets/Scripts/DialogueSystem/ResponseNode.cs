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

        public override DialogueNode Child => CurrentResponse?.Child;

        public IEnumerable<Response> Responses => responses;

        public int ChildrenCount => responses.Count;

        public string GetText(int index)
        {
            return responses[index].Text;
        }

        public DialogueNode GetChild(int index)
        {
            return responses[index].Child;
        }

#if UNITY_EDITOR
        private void Awake()
        {
            responses ??= new List<Response>();
        }

        public void UnlinkChild(int index)
        {
            responses[index].Child = null;
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
            responses[index].Text = text;
            EditorUtility.SetDirty(this);
        }

        public void SetChild(DialogueNode node, int index)
        {
            responses[index].Child = node;
            EditorUtility.SetDirty(this);
        }

        public override bool TryRemoveChild(DialogueNode node)
        {
            Response response = responses.FirstOrDefault(r => r.Child == node);

            if (response is null) return false;
            response.Child = null;
            EditorUtility.SetDirty(this);
            return true;
        }
#endif
    }

    [Serializable]
    public class Response
    {
        [SerializeField] private string text;
        [SerializeField] private string trigger;
        [SerializeField, HideInInspector] private DialogueNode child;

        public string Text
        {
            get => text;
#if UNITY_EDITOR
            set
            {
                text = value;
            }
#endif
        }

        public string Trigger
        {
            get => trigger;
#if UNITY_EDITOR
            set
            {
                trigger = value;
            }
#endif
        }

        public DialogueNode Child
        {
            get => child;
#if UNITY_EDITOR
            set => child = value;
#endif
        }
    }
}
