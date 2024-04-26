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
        public TMP_Text option1, option2;
        public Animator anim;
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
        }

        void Start()
        {
        
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I)) // TODO: Change this so that it works when the player is near an NPC
            {
                anim.SetBool(IsOpen, !anim.GetBool(IsOpen));
            }
        }
    }
}
