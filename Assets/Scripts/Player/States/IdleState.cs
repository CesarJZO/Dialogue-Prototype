using System;
using UnityEngine;

namespace Player
{
    public class IdleState : PlayerState
    {
        public event EventHandler OnInteract;
        public class InteractArgs : EventArgs
        {
            public string npcName;
            public InteractArgs(string npcName)
            {
                this.npcName = npcName;
            }
        }
        public IdleState(PlayerController player) : base(player) { }
        
        public override void Start()
        {
            player.interacting = false;
            player.rigidbody.velocity = Vector2.zero;
        }

        public override void HandleInput()
        {
            var npc = player.CanInteract;
            if (npc && player.interactAction.WasPressedThisFrame())
            {
                Debug.Log("Interacting!");
                OnInteract?.Invoke(player, new InteractArgs(npc.name));
            }
            
            if (player.moveAction.ReadValue<float>() != 0f)
                player.ChangeState(player.walkState);
        }

        public override string ToString() => nameof(IdleState);
    }
}
