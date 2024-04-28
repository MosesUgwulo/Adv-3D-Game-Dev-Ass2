using UnityEngine;
using UnityEngine.UI;

namespace Classes
{
    public enum ItemType
    {
        Sword,
        Map,
    }
    [System.Serializable]
    public class Item
    {
        public ItemType itemType;
        public Sprite itemImage;
        public string itemName, itemDescription;
        public int count, damage;
        
        public Item(ItemType itemType, Sprite itemImage, string itemName, string itemDescription, int count, int damage)
        {
            this.itemType = itemType;
            this.itemImage = itemImage;
            this.itemName = itemName;
            this.itemDescription = itemDescription;
            this.count = count;
            this.damage = damage;
        }
    }
}
