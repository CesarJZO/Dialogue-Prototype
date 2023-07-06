namespace CesarJZO.DialogueSystem
{
    public class SimpleNode : DialogueNode
    {
        private DialogueNode _child;

        public override DialogueNode Child => _child;

        public override bool TryRemoveChild(DialogueNode node)
        {
            if (_child != node) return false;

            _child = null;
            return true;
        }

        public DialogueNode GetChild()
        {
            return _child;
        }

        public void SetChild(DialogueNode node)
        {
            _child = node;
        }

        public void UnlinkChild()
        {
            _child = null;
        }
    }
}
