using System;
using System.IO;
using Player;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public PlayerController player;
        public Dialogue dialogue;

        private void Start()
        {
            player = PlayerController.Instance;
            // player.idleState.OnInteract += IdleStateOnInteract; 
        }

        private void IdleStateOnInteract(object sender, IdleState.InteractArgs eventArgs)
        {
            Debug.Log("Interacting!");
            var json = File.ReadAllText(Application.dataPath + $"/Dialogues/{eventArgs.npcName}/.json");
            dialogue.lines = JsonUtility.FromJson<Dialogue.Lines>(json);
            Debug.Log(dialogue.lines.lines.ToString());
        }
    }
}
