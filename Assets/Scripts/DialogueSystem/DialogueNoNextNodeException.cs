using System;

namespace CesarJZO.DialogueSystem
{
    public class DialogueNoNextNodeException : Exception
    {
        public DialogueNoNextNodeException(string message) : base(message) { }
    }
}
