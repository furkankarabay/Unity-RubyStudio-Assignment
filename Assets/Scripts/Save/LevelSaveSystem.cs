using RubyGame.Constants;
using RubyGame.Managers;
using RubyGame.Singleton;
using UnityEngine;

namespace RubyGame.SaveSystem
{
    public class LevelSaveSystem : NonPersistentSingleton<LevelSaveSystem>
    {
        [Header("Test Variables")]
        public bool resetSaveFile;

        [HideInInspector]
        public int currentLevel;

        [HideInInspector]
        public bool levelCountIncreased = false;


        protected override void Awake()
        {
            base.Awake();

            currentLevel = PlayerPrefs.GetInt(PlayerPrefConstants.CURRENT_LEVEL_KEY, 0);
        }

        private void LevelCompleted()
        {
            currentLevel++;
            levelCountIncreased = true;
            PlayerPrefs.SetInt(PlayerPrefConstants.CURRENT_LEVEL_KEY, currentLevel);
            PlayerPrefs.Save();
        }

        private void OnEnable()
        {
            EventsManager.OnLevelCompleted += LevelCompleted;
        }

        private void OnDisable()
        {
            if (EventsManager.OnLevelCompleted != null)
                EventsManager.OnLevelCompleted -= LevelCompleted;
        }

        private void OnApplicationQuit()
        {
            if (resetSaveFile)
            {
                PlayerPrefs.DeleteKey(PlayerPrefConstants.CURRENT_LEVEL_KEY);
                Debug.Log("Reset save file successful!");
            }
        }
    }
}
