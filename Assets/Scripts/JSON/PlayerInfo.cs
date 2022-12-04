using UnityEngine;

namespace JSON
{
    [System.Serializable]
    public class PlayerInfo
    {
        public string name;
        public int lives;
        public float health;

        public static PlayerInfo CreateFromJson(string jsonString)
        {
            return JsonUtility.FromJson<PlayerInfo>(jsonString);
        }

        public override string ToString() => $"Name: {name}, Lives: {lives}, Health: {health}";
    }
}