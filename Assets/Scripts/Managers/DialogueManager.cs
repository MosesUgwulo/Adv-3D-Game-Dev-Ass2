using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using HUD;
using NPCs;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        public List<Character> characters;
        // public TMP_Text nameText;
        // public TMP_Text dialogueText;
        // public TMP_Text option1, option2;
        // public Animator anim;
        private Queue<string> _sentences;
        private Dictionary<string, Character> _charactersById;
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
                    _charactersById.Add(character.questID, character);
                }
            }
            
            Debug.Log("Loaded " + _charactersById.Count + " characters.");
        }


        public void StartDialogue(string npcName, List<Dialogue> sentences)
        {
            HUDManager.Instance.anim.SetBool(IsOpen, true);
            HUDManager.Instance.nameText.text = npcName;
            
            HUDManager.Instance.option1.text = sentences[0].decisions[0].content;
            HUDManager.Instance.option2.text = sentences[0].decisions[1].content;
            
            Cursor.lockState = CursorLockMode.None;
            
            
            _sentences.Clear();
            
            foreach (var sentence in sentences)
            {
                _sentences.Enqueue(sentence.content);
            }
            DisplayNextSentence();
        }

        public Character GetDialogue(string questID)
        {
            return _charactersById.GetValueOrDefault(questID);
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
