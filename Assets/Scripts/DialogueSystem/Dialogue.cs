using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DialogueNode> nodes;
        public DialogueNode RootNode => nodes[0];

        public IEnumerable<DialogueNode> Nodes => nodes.AsReadOnly();

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            nodes ??= new List<DialogueNode>();
            // if (nodes.Count == 0)
            // {
            //     var newNode = CreateInstance<DialogueNode>();
            //     nodes.Add(newNode);
            // }

            // if (AssetDatabase.GetAssetPath(this) == string.Empty) return;
            // foreach (DialogueNode node in nodes.Where(node => AssetDatabase.GetAssetPath(node) == string.Empty))
            //     AssetDatabase.AddObjectToAsset(node, this);
#endif
        }

        public void OnAfterDeserialize() { }
    }
}
