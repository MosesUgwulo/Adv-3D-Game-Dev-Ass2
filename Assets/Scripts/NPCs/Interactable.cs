using Classes;
using Managers;
using UnityEngine;


namespace NPCs
{
    public class Interactable : MonoBehaviour
    {
        public string npcName;
        public string questID;
        public Character character;
        void Start()
        {
            InitDialogue();
        }
        
        public void Interact()
        {
            DialogueManager.Instance.StartDialogue(npcName, character.dialogues);
        }

        public void InitDialogue()
        {
            if (!string.IsNullOrEmpty(questID))
            {
                character = DialogueManager.Instance.GetDialogue(questID);
            }
        }
        
        void Update()
        {
        
        }
    }
}
