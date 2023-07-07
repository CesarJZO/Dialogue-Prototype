using UnityEngine;

namespace Core
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private string triggerCode;

        public void TryOpenDoor(string trigger)
        {
            if (trigger != triggerCode) return;

            Debug.Log("Door opened");
        }
    }
}
