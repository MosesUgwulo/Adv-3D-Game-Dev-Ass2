using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Classes;
using NPCs;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        // public List<Character> characters;
        private Queue<string> _sentences;
        private Dictionary<string, Character> _charactersById;
        private Dialogue _currentDialogue;
        private string _filePath = Path.Combine(Application.streamingAssetsPath, "Dialogues.xml");
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        
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
            InitCharacters();
        }
        void Start()
        {
            _sentences = new Queue<string>();
        }

        public void InitCharacters()
        {
            _charactersById = new Dictionary<string, Character>();
            if (File.Exists(_filePath))
            {
                foreach (var character in Load(_filePath)._characters)
                {
                    // characters.Add(character);
                    _charactersById.Add(character.questID, character);
                }
            }
            
            // Debug.Log("Loaded " + _charactersById.Count + " characters.");
        }


        public void StartDialogue(string npcName, List<Dialogue> sentences)
        {
            HUDManager.Instance.anim.SetBool(IsOpen, true);
            HUDManager.Instance.nameText.text = npcName;
            
            _currentDialogue = sentences[0];
            
            HUDManager.Instance.dialogueText.text = _currentDialogue.content;

            if (_currentDialogue.decisions.Count >= 3)
            {
                HUDManager.Instance.option1.text = _currentDialogue.decisions[0].content;
                HUDManager.Instance.option2.text = _currentDialogue.decisions[1].content;
                HUDManager.Instance.option3.text = _currentDialogue.decisions[2].content;
            }
            
            Cursor.lockState = CursorLockMode.None;
            _sentences.Clear();
            
            foreach (var sentence in sentences)
            {
                _sentences.Enqueue(sentence.content);
            }
        }
        
        public void Option1Selected()
        {
            DisplayNextSentence(0);
        }
        
        public void Option2Selected()
        {
            DisplayNextSentence(1);
        }

        public void Option3Selected()
        {
            DisplayNextSentence(2);
        }

        private void DisplayNextSentence(int optionSelected)
        {
            string targetID = _currentDialogue.decisions[optionSelected].target.ToString();
            // Debug.Log("Target ID: " + targetID);

            if (targetID == "-1")
            {
                EndDialogue();
                return;
            }
            
            Dialogue nextDialogue = null;
            
            foreach (var character in _charactersById.Values)
            {
                foreach (var dialogue in character.dialogues)
                {
                    if (dialogue.id == targetID)
                    {
                        // Debug.Log("Character: " + character.questID + " Dialogue: " + dialogue.content);
                        nextDialogue = dialogue;
                        break;
                    }
                }
            }
            
            if (nextDialogue == null)
            {
                Debug.LogError("Dialogue not found");
                return;
            }
            
            _currentDialogue = nextDialogue;
            _sentences.Clear();
            
            _sentences.Enqueue(_currentDialogue.content);
            
            if (_currentDialogue.decisions.Count >= 3)
            {
                HUDManager.Instance.option1.text = _currentDialogue.decisions[0].content;
                HUDManager.Instance.option2.text = _currentDialogue.decisions[1].content;
                HUDManager.Instance.option3.text = _currentDialogue.decisions[2].content;
            }
            else
            {
                HUDManager.Instance.option1.text = "";
                HUDManager.Instance.option2.text = "";
                HUDManager.Instance.option3.text = "";
            }
            
            DisplayNextSentence();
        }
        
        private void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            
            string sentence = _sentences.Dequeue();
            HUDManager.Instance.dialogueText.text = sentence;
        }
        
        public Character GetDialogue(string questID)
        {
            return _charactersById.GetValueOrDefault(questID);
        }
        

        private void EndDialogue()
        {
            HUDManager.Instance.anim.SetBool(IsOpen, false);
        }

        public CharacterSaveData Load(string fileName)
        {
            var serializer = new XmlSerializer(typeof(CharacterSaveData));
            using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return serializer.Deserialize(stream) as CharacterSaveData;
            }
        }
        
        public void Save(string fileName, CharacterSaveData character)
        {
            var serializer = new XmlSerializer(typeof(CharacterSaveData));
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(stream, character);
            }
        }

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     Save(_filePath, new CharacterSaveData(characters));
            // }
        }
    }

    [System.Serializable]
    public class CharacterSaveData
    {
        public List<Character> _characters;
        
        public CharacterSaveData(List<Character> characters)
        {
            _characters = characters;
        }
        
        public CharacterSaveData()
        {
            _characters = new List<Character>();
        }
    }
    
}
