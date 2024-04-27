using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Classes
{
    public enum QuestType
    {
        TalkTo,
        Acquire,
        Destroy,
        Location
    }
    [Serializable, XmlRoot("Quest")]
    public class Quest
    {
        [XmlAttribute("questID")]
        public string questID;
        
        [XmlAttribute("isCompleted")]
        public bool isCompleted;
        
        [XmlElement("stage")]
        public List<Stage> stages;
    }

    [Serializable]
    public class Stage
    {
        [XmlAttribute("stageID")]
        public string stageID;
        
        [XmlAttribute("stageName")]
        public string stageName;
        
        [XmlAttribute("stageDescription")]
        public string stageDescription;
        
        [XmlElement("results")]
        public List<Result> results;
    }
    
    [Serializable]
    public class Result
    {
        [XmlAttribute("action")] 
        public QuestType questType;
        
        [XmlAttribute("target")]
        public string target;
        
        [XmlAttribute("xp")]
        public int xp;
        
        [XmlAttribute("isCompleted")]
        public bool isCompleted;
    }
}
