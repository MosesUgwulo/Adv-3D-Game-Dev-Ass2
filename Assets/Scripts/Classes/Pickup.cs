using System;
using Managers;
using Player;
using UnityEngine;

namespace Classes
{
    public class Pickup : MonoBehaviour
    {
        public Item item;

        private void OnTriggerEnter(Collider other)
        {
            var (_, _, result) = QuestManager.Instance.GetCurrentQuestStageResult();
            
            if (other.CompareTag("Player"))
            {
                if (result.target != item.itemName)
                {
                    Debug.Log("Wrong item");
                    return;
                }
                Inventory.Instance.AddItem(item);
                result.isCompleted = true;
                HUDManager.Instance.ShowXpText();
                QuestManager.Instance.UpdateQuestUI();
                Destroy(gameObject);
            }
        }
    }
}
