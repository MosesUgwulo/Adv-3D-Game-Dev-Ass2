using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Classes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class QuestManager : MonoBehaviour
    {
        public List<Quest> quests;
        public static QuestManager Instance;
        private Dictionary<string, Quest> _quests = new Dictionary<string, Quest>();
        private string _filePath = Path.Combine(Application.streamingAssetsPath, "Quests.xml");
        
        private Quest _currentQuest;
        private Stage _currentStage;
        private Result _currentResult;
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
            InitQuests();
        }

        private void InitQuests()
        {
            if (File.Exists(_filePath))
            {
                foreach (var quest in Load(_filePath)._quests)
                {
                    _quests.Add(quest.questID, quest);
                }
            }
            // Debug.Log("Loaded " + _quests.Count + " quests.");
        }
        
        
        void Start()
        {
            UpdateQuestUI();
        }

        public void UpdateQuestUI()
        {
            _currentQuest = GetQuest();
            if (_currentQuest == null)
            {
                HUDManager.Instance.stageDescription.text = "All quests completed!";
                HUDManager.Instance.questDetails.text = "";
                return;
            }
            
            _currentStage = GetStage(_currentQuest.stages[0].stageID);
            if (_currentStage == null)
            {
                HUDManager.Instance.stageDescription.text = "Stage completed!";
                HUDManager.Instance.questDetails.text = "";
                return;
            }
            
            _currentResult = GetResult(_currentStage.stageID);
            if (_currentResult == null)
            {
                HUDManager.Instance.stageDescription.text = _currentStage.stageDescription;
                HUDManager.Instance.questDetails.text = "All actions completed!, Proceed to next level.";
                _currentQuest.isCompleted = true;
                UpdateQuestUI();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }

            string questPrefix = "";
            switch (_currentResult.questType)
            {
                case QuestType.TalkTo:
                    questPrefix = "Talk to";
                    break;
                case QuestType.Acquire:
                    questPrefix = "Acquire a";
                    break;
                case QuestType.Destroy:
                    questPrefix = "Destroy the";
                    break;
                case QuestType.Location:
                    questPrefix = "Find the";
                    break;
            }
            
            HUDManager.Instance.stageDescription.text = _currentStage.stageDescription;
            HUDManager.Instance.questDetails.text = questPrefix + " " + _currentResult.target;
        }

        public Quest GetQuest()
        {
            return _quests.Values.FirstOrDefault(q => !q.isCompleted);
        }

        public Stage GetStage(string stageID)
        {
            return GetQuest().stages.FirstOrDefault(s => s.stageID == stageID);
        }
        
        public Result GetResult(string stageID)
        {
            return GetStage(stageID).results.FirstOrDefault(r => !r.isCompleted);
        }
        
        public (Quest, Stage, Result) GetCurrentQuestStageResult()
        {
            return (_currentQuest, _currentStage, _currentResult);
        }
        
        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     Save(_filePath, new QuestSaveData(quests));
            // }
        }
        
        public QuestSaveData Load(string fileName)
        {
            var serializer = new XmlSerializer(typeof(QuestSaveData));
            using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return serializer.Deserialize(stream) as QuestSaveData;
            }
        }
        
        public void Save(string fileName, QuestSaveData questData)
        {
            var serializer = new XmlSerializer(typeof(QuestSaveData));
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(stream, questData);
            }
        }
    }
    
    [System.Serializable]
    public class QuestSaveData
    {
        public List<Quest> _quests;
        
        public QuestSaveData(List<Quest> quests)
        {
            _quests = quests;
        }
        
        public QuestSaveData()
        {
            _quests = new List<Quest>();
        }
    }
}
