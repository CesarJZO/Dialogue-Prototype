using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/Speaker", order = 1)]
    public class Speaker : ScriptableObject
    {
        [SerializeField] private string displayName;
        public string DisplayName => displayName;

        [SerializeField] private Sprite neutralSprite;
        public Sprite NeutralSprite => neutralSprite;

        [SerializeField] private Sprite happySprite;
        public Sprite HappySprite => happySprite;

        [SerializeField] private Sprite sadSprite;
        public Sprite SadSprite => sadSprite;

        [SerializeField] private Sprite angrySprite;
        public Sprite AngrySprite => angrySprite;
    }
}
