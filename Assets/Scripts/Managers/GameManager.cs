using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubyGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager Instance;

        private void InitSingleton()
        {
            if (!Instance) Instance = this;
            else Destroy(this);
        }

        #endregion

        [Header("Level Settings")]
        public List<GameObject> levels;
        public GameObject currentLevelObject;

        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            currentLevelObject = Instantiate(levels[LevelManager.Instance.currentLevelSettings.levelIndex]);
            currentLevelObject.SetActive(true);
        }

        public void LevelEnd(bool isWon)
        {
            if (isWon)
            {
                MoneyManager.Instance.IncreaseMoney(LevelManager.Instance.currentLevelSettings.prizeMoney);

                EventsManager.OnLevelCompleted?.Invoke();

            }
            else
            {
                MoneyManager.Instance.IncreaseMoney(LevelManager.Instance.currentLevelSettings.defeatMoney);

                EventsManager.OnLevelFailed?.Invoke();
            }
        }
    }
}
