using UnityEngine;

namespace CesarJZO.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/Speaker", order = 4)]
    public class Speaker : ScriptableObject
    {
        [field: SerializeField] public Sprite NeutralSprite { get; private set; }
        [field: SerializeField] public Sprite HappySprite { get; private set; }
        [field: SerializeField] public Sprite SadSprite { get; private set; }
    }
}
