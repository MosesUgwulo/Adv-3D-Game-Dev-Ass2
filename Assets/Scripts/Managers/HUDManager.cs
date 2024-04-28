using System;
using System.Collections;
using Classes;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance;
        
        [Header("Dialogue")]
        public TMP_Text nameText;
        public TMP_Text dialogueText;
        public TMP_Text option1, option2, option3;
        public Animator anim;
        public static readonly int IsOpen = Animator.StringToHash("IsOpen");
        
        [Header("Quests")]
        public GameObject questTab;
        public TMP_Text stageDescription;
        public TMP_Text questDetails;
        public TMP_Text xpText;
        
        [Header("Inventory")]
        public GameObject inventory;
        public GameObject inventorySlotPrefab;
        public Transform inventoryContent;
        public TMP_Text itemDetails;
        public Image itemImage;

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

        void Start()
        {
            inventory.SetActive(false);
            
        }
        
        public void UpdateInventoryUI(Item item)
        {
            foreach (Transform child in inventoryContent)
            {
                Destroy(child.gameObject);
                itemDetails.text = "";
                itemImage.sprite = null;
            }
            
            foreach (var itemInInventory in Inventory.Instance.items)
            {
                GameObject slot = Instantiate(inventorySlotPrefab, inventoryContent);
                
                var itemName = slot.transform.Find("ItemName");
                itemName.GetComponent<Button>().onClick.AddListener(() => ShowItemDetails(itemInInventory));
                itemName.GetComponentInChildren<TMP_Text>().text = itemInInventory.itemName;
                
                var removeItem = slot.transform.Find("RemoveItem");
                removeItem.GetComponent<Button>().onClick.AddListener(() => Inventory.Instance.RemoveItem(itemInInventory));
            }
        }

        private void ShowItemDetails(Item item)
        {
            itemDetails.text = $"Item name: {item.itemName}\n" +
                               $"Item description: {item.itemDescription}\n" +
                               $"Item count: {item.count}\n" +
                               $"Item damage: {item.damage}";
            itemImage.sprite = item.itemImage;
        }
        
        public void ShowXpText()
        {
            var (_, _, xp) = QuestManager.Instance.GetCurrentQuestStageResult();
            xpText.text = $"You have earned {xp.xp} XP!";
            StartCoroutine(FadeTextOut(2f));
        }

        IEnumerator FadeTextOut(float fadeTime)
        {
            float startAlpha = xpText.color.a;
            
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
            {
                Color newColor = new Color(xpText.color.r, xpText.color.g, xpText.color.b, Mathf.Lerp(startAlpha, 0, t));
                xpText.color = newColor;
                yield return null;
            }

            xpText.text = "";
            xpText.color = new Color(xpText.color.r, xpText.color.g, xpText.color.b, startAlpha);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                questTab.SetActive(!questTab.activeSelf);
            }
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
                inventory.SetActive(!inventory.activeSelf);
            }
        }
    }
}
