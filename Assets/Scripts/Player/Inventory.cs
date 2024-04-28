using System;
using System.Collections.Generic;
using Classes;
using Managers;
using UnityEngine;

namespace Player
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;
        public List<Item> items = new List<Item>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddItem(Item itemToAdd)
        {
            bool itemExists = false;

            foreach (var item in items)
            {
                if (item.itemName == itemToAdd.itemName)
                {
                    item.count += itemToAdd.count;
                    itemExists = true;
                    HUDManager.Instance.UpdateInventoryUI(itemToAdd);
                    break;
                }
            }
            
            if (!itemExists)
            {
                items.Add(itemToAdd);
                HUDManager.Instance.UpdateInventoryUI(itemToAdd);
            }
            Debug.Log("Added " + itemToAdd.itemName + " to inventory" + " with count " + itemToAdd.count);
        }
        
        public void RemoveItem(Item itemToRemove)
        {
            foreach (var item in items)
            {
                if (item.itemName == itemToRemove.itemName)
                {
                    item.count -= itemToRemove.count;
                    if (item.count <= 0)
                    {
                        items.Remove(item);
                        HUDManager.Instance.UpdateInventoryUI(itemToRemove);
                    }
                    break;
                }
            }
            Debug.Log("Removed " + itemToRemove.itemName + " from inventory" + " with count " + itemToRemove.count);
        }
    }
}
