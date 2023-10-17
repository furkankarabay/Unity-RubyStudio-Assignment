using RubyGame.SaveSystem;
using RubyGame.ScriptableObjects.Level;
using RubyGame.Singleton;
using TMPro;
using UnityEngine;

namespace RubyGame.Managers
{
    public class LevelManager : NonPersistentSingleton<LevelManager>
    {
        [Header("Test Variables")]

        public bool testMode;
        public LevelSettings levelSettingsTest;


        [Space(4)]
        [Header("Levels")]

        public LevelSettings[] allLevels;
        public LevelSettings[] repeatLevels;
        public LevelSettings currentLevelSettings;

        [Header("UI")]

        [SerializeField] private TextMeshProUGUI levelContent;


        private int currentLevel;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            SetupLevelIndex();

            EventsManager.OnLevelCreated?.Invoke();

            levelContent.text = "LEVEL " + (currentLevel + 1);
        }

        public void SetupLevelIndex()
        {
            currentLevel = LevelSaveSystem.Instance.currentLevel;

            if (testMode)
            {
                currentLevelSettings = levelSettingsTest;
            }
            else
            {
                currentLevelSettings = currentLevel < allLevels.Length ? allLevels[currentLevel] : repeatLevels[(currentLevel - allLevels.Length) % repeatLevels.Length];
            }
        }
    }
}
