namespace CesarJZO.DialogueSystem.Editor
{
    public class NodeContext
    {
        public readonly DialogueNode parentNode;
        public bool valueIfConditional;
        public int indexIfResponse;

        public NodeContext(DialogueNode parentNode)
        {
            this.parentNode = parentNode;
        }
    }
}
