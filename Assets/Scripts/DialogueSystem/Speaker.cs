using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/Speaker", order = 4)]
    public class Speaker : ScriptableObject
    {
        [SerializeField] private string displayName;
        public string DisplayName => displayName;

        [SerializeField] private Sprite neutralSprite;
        public Sprite NeutralSprite => neutralSprite;
    }
}
