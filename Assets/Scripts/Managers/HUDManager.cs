using System;
using UnityEngine;

namespace Managers
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance;

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
