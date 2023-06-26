using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> nodes;
        public DialogueNode RootNode => nodes[0];

        public IEnumerable<DialogueNode> Nodes => nodes.AsReadOnly();


    }
}
