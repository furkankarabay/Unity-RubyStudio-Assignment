using RubyGame.GameData;
using RubyGame.GamePlay;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RubyGame.Managers
{
    public class BattleManager : MonoBehaviour
    {
        #region Singleton

        public static BattleManager Instance;

        private void InitSingleton()
        {
            if (!Instance) Instance = this;
            else Destroy(this);
        }

        #endregion

        #region Variables

        [Header("Settings")]

        [SerializeField] private LayerMask arenaGroundLayerMask;

        [Header("UI")]

        [SerializeField] private GameObject cardsPanel;
        [SerializeField] private TextMeshProUGUI firstSoldierCardContent;
        [SerializeField] private TextMeshProUGUI secondSoldierCardContent;

        [SerializeField] private Transform firstSoldierCard;
        [SerializeField] private Transform secondSoldierCard;

        private int firstLevelCardCount;
        private int secondLevelCardCount;

        private bool isBattleStarted = false;
        private bool isCardSelected = false;
        private TypeOfCharacters selectedCardType;

        [HideInInspector] public List<GameObject> battleSoldiers = new List<GameObject>();
        [HideInInspector] public List<Enemy> battleEnemies;

        #endregion

        #region UnityEvents

        private void Awake()
        {
            InitSingleton();
        }

        private void OnEnable()
        {

            EventsManager.OnInteractedBattleArea += BattleStarted;
            EventsManager.OnLevelCreated += OnLevelCreated;
            EventsManager.OnLevelCompleted += OnLevelCompleted;
        }

        private void OnDisable()
        {
            EventsManager.OnInteractedBattleArea -= BattleStarted;
            EventsManager.OnLevelCreated -= OnLevelCreated;
            EventsManager.OnLevelCompleted -= OnLevelCompleted;
        }

        private void Update()
        {
            if (!isBattleStarted)
                return;

            if (!isCardSelected)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000, arenaGroundLayerMask))
                {
                    if (CheckCanSpawnSoldier(selectedCardType))
                    {
                        Vector3 spawnPosition = hit.point + new Vector3(0, 0.5f, 0);

                        GameObject soldierGO = CharacterSpawnManager.instance.SpawnBattleCharacter(selectedCardType, spawnPosition);
                        battleSoldiers.Add(soldierGO);

                        UpdateCardsCount(selectedCardType);
                        UpdateUICards();

                        EventsManager.OnPlacedSoldierInArena?.Invoke();
                    }
                }
            }
        }

        #endregion

        #region Methods

        private void OnLevelCreated()
        {
            UpdateUICards();

        }

        private void OnLevelCompleted()
        {
            isBattleStarted = false;
        }

        private void UpdateUICards()
        {
            firstSoldierCardContent.text = firstLevelCardCount.ToString();
            secondSoldierCardContent.text = secondLevelCardCount.ToString();

        }

        private void BattleStarted(bool isStarted)
        {
            isBattleStarted = isStarted;

            cardsPanel.SetActive(isStarted);

            firstLevelCardCount = SaveData.FirstLevelSoldierNumberValue;
            secondLevelCardCount = SaveData.SecondLevelSoldierNumberValue;

            UpdateUICards();
        }

        private bool CheckCanSpawnSoldier(TypeOfCharacters type)
        {
            if (type == TypeOfCharacters.villager)
                return firstLevelCardCount > 0;
            else if (type == TypeOfCharacters.upgradedVillager)
                return secondLevelCardCount > 0;

            return false;
        }

        private void UpdateCardsCount(TypeOfCharacters type)
        {
            switch (type)
            {
                case TypeOfCharacters.villager:
                    firstLevelCardCount -= 1;
                    break;

                case TypeOfCharacters.upgradedVillager:
                    secondLevelCardCount -= 1;
                    break;
            }
        }

        public void SelectCard(int type)
        {
            if (type == 0) // First Level Soldier
            {
                if (firstLevelCardCount > 0)
                {
                    if (isCardSelected)
                    {
                        secondSoldierCard.DOLocalMoveY(0, 0.2f);
                    }

                    firstSoldierCard.DOLocalMoveY(100, 0.2f);
                    isCardSelected = true;
                    selectedCardType = TypeOfCharacters.villager;
                }
            }
            else if (type == 1)
            {
                if (secondLevelCardCount > 0)
                {
                    if (isCardSelected)
                    {
                        firstSoldierCard.DOLocalMoveY(0, 0.2f);
                    }

                    secondSoldierCard.DOLocalMoveY(100, 0.2f);
                    isCardSelected = true;
                    selectedCardType = TypeOfCharacters.upgradedVillager;
                }
            }
        }

        public bool IsThereAnySoldierInArena()
        {
            if (battleSoldiers.Count > 0)
                return true;

            return false;
        }

        public bool IsThereAnyEnemyInArena()
        {
            if (battleEnemies.Count > 0)
                return true;

            return false;
        }

        public void RemoveSoldierFromList(GameObject soldierToRemove, TypeOfCharacters type)
        {
            battleSoldiers.Remove(soldierToRemove);
            BarrackManager.instance.RemoveSoldierFromList(type);

            CheckLevelFailed();
        }

        public void RemoveEnemyFromList(Enemy enemyToRemove)
        {
            battleEnemies.Remove(enemyToRemove);
            CheckLevelCompleted();
        }

        private void CheckLevelFailed()
        {
            if (!IsThereAnySoldierInArena() && !BarrackManager.instance.DoesHaveAnySoldier())
            {
                GameManager.Instance.LevelEnd(false);
            }
        }

        private void CheckLevelCompleted()
        {
            if (!IsThereAnyEnemyInArena())
            {
                GameManager.Instance.LevelEnd(true);
            }
        }

        #endregion
    }
}
