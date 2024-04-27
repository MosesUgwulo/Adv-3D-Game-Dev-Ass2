using System;
using TMPro;
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
        
        }

        void Update()
        {
            
        }
    }
}
