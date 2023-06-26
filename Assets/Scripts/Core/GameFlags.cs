using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CesarJZO.Core
{
    [CreateAssetMenu]
    public class GameFlags : ScriptableObject
    {
        [SerializeField] private List<GameFlag> flags;

        public List<GameFlag> Flags => flags;

        public bool CheckFlag(GameFlag flag) => flags.Any(gameFlag => flag == gameFlag);
    }

    [Serializable]
    public class GameFlag
    {
        public string name;
        public bool value;
    }
}
