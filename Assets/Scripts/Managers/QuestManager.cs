using UnityEngine;

namespace Managers
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;
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
