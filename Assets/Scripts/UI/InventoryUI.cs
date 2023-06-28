using System;
using CesarJZO.InventorySystem;
using UnityEngine;

namespace CesarJZO.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public static event Action<Item> ItemSelected;

        private void Start()
        {
            Quit();
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Quit()
        {
            gameObject.SetActive(false);
        }

        public void SelectItem(Item item)
        {
            ItemSelected?.Invoke(item);
        }
    }
}
