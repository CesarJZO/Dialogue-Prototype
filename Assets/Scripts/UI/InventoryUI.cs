using System;
using UnityEngine;

namespace CesarJZO.UI
{
    public class InventoryUI : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Quit()
        {
            gameObject.SetActive(false);
        }
    }
}
