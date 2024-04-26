using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace HUD
{
    [System.Serializable]
    public class Decision
    {
        [XmlAttribute("content")]
        [TextArea(3, 10)]
        public string content;
        
        [XmlAttribute("target")]
        public int target;
    }

    [System.Serializable]
    public class Dialogue
    {
        [XmlAttribute("id")]
        public string id;
        
        [XmlAttribute("content")]
        [TextArea(3, 10)]
        public string content;
        
        public List<Decision> decisions;
    }

    [System.Serializable]
    public class Character
    {
        [XmlAttribute("name")]
        public string name;
        
        [XmlAttribute("questID")]
        public string questID;
        
        public List<Dialogue> dialogues;
    }
}
