using RubyGame.GameData;
using RubyGame.GamePlay;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RubyGame.Managers
{
    public class BarrackManager : MonoBehaviour
    {
        #region Singleton

        public static BarrackManager instance;

        private void InitSingleton()
        {
            if (!instance) instance = this;
            else Destroy(this);
        }

        #endregion

        #region Variables

        [Header("Settings")]

        public List<Transform> deployPositions;
        public List<MergeableCharacter> mergeableCharacters = new List<MergeableCharacter>();

        [Header("UI")]

        [SerializeField] private TextMeshProUGUI firstLevelSoldierText;
        [SerializeField] private TextMeshProUGUI secondLevelSoldierText;

        private int currentFirstLevelSoldier;
        private int currentSecondLevelSoldier;

        #endregion

        #region Unity Events
        private void Awake()
        {
            InitSingleton();
        }

        void Start()
        {
            UpdateCurrentSoldiersUI();
            //player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable()
        {
            EventsManager.OnInteractedMergeArea += SetSoldiersMergeable;
            EventsManager.OnDeploySoldierToBarrack += DeploySoldierToBarrack;
            EventsManager.OnLevelCreated += UpdateBarrack;

        }

        private void OnDisable()
        {
            EventsManager.OnInteractedMergeArea -= SetSoldiersMergeable;
            EventsManager.OnDeploySoldierToBarrack -= DeploySoldierToBarrack;
            EventsManager.OnLevelCreated -= UpdateBarrack;

        }


        #endregion

        #region Methods

        public bool DoesHaveAnySoldier()
        {
            if (mergeableCharacters.Count > 0)
                return true;

            return false;
        }

        private void UpdateBarrack()
        {
            int firstLevelSoldierCount = GetCurrentSoldier(TypeOfCharacters.villager);
            int secondLevelSoldierCount = GetCurrentSoldier(TypeOfCharacters.upgradedVillager);

            for (int i = 0; i < firstLevelSoldierCount; i++)
            {
                GameObject soldierGO = CharacterSpawnManager.instance.SpawnMergeableCharacter(TypeOfCharacters.villager, GetRandomPositionInArea());
                DeploySoldierToBarrack(transform, soldierGO);
            }

            for (int i = 0; i < secondLevelSoldierCount; i++)
            {
                GameObject soldierGO = CharacterSpawnManager.instance.SpawnMergeableCharacter(TypeOfCharacters.upgradedVillager, GetRandomPositionInArea());
                DeploySoldierToBarrack(transform, soldierGO);
            }

        }


        private int GetCurrentSoldier(TypeOfCharacters type)
        {
            int number = 0;

            switch (type)
            {
                case TypeOfCharacters.villager:

                    number = SaveData.FirstLevelSoldierNumberValue;
                    break;

                case TypeOfCharacters.upgradedVillager:

                    number = SaveData.SecondLevelSoldierNumberValue;
                    break;
            }

            return number;
        }

        public void SetCurrentSoldier(int value, TypeOfCharacters type)
        {
            switch (type)
            {
                case TypeOfCharacters.villager:

                    SaveData.FirstLevelSoldierNumberValue = value;
                    break;

                case TypeOfCharacters.upgradedVillager:

                    SaveData.SecondLevelSoldierNumberValue = value;

                    break;
            }
        }

        public void UpdateCurrentSoldiersUI()
        {

            var currentFirstLevelSoldier = GetCurrentSoldier(TypeOfCharacters.villager);
            firstLevelSoldierText.text = currentFirstLevelSoldier.ToString();

            var currentSecondLevelSoldier = GetCurrentSoldier(TypeOfCharacters.upgradedVillager);
            secondLevelSoldierText.text = currentSecondLevelSoldier.ToString();
            //IncrementalManager.instance.CheckAffordability();

        }

        public void IncreaseSoldier(int value, TypeOfCharacters type)
        {

            switch (type)
            {
                case TypeOfCharacters.villager:

                    currentFirstLevelSoldier += value;
                    SetCurrentSoldier(currentFirstLevelSoldier, type);

                    break;

                case TypeOfCharacters.upgradedVillager:

                    currentSecondLevelSoldier += value;
                    SetCurrentSoldier(currentSecondLevelSoldier, type);
                    break;
            }

            UpdateCurrentSoldiersUI();
        }

        public void DecreaseSoldier(int value, TypeOfCharacters type)
        {

            switch (type)
            {
                case TypeOfCharacters.villager:

                    currentFirstLevelSoldier -= value;
                    SetCurrentSoldier(currentFirstLevelSoldier, type);

                    break;

                case TypeOfCharacters.upgradedVillager:

                    currentSecondLevelSoldier -= value;
                    SetCurrentSoldier(currentSecondLevelSoldier, type);
                    break;
            }

            UpdateCurrentSoldiersUI();
        }

        private void SetSoldiersMergeable(bool isMergeable)
        {
            for (int i = 0; i < mergeableCharacters.Count; i++)
            {
                mergeableCharacters[i].isMergeable = isMergeable;
                mergeableCharacters[i].canBeHold = isMergeable;
            }
        }

        private Vector3 GetRandomPositionInArea()
        {
            int random = Random.Range(0, deployPositions.Count);
            Vector3 position = deployPositions[random].position;
            deployPositions.RemoveAt(random);
            return position;
        }

        private void DeploySoldierToBarrack(Transform parent, GameObject willDeploySoldier)
        {
            willDeploySoldier.transform.SetParent(parent);

            if (willDeploySoldier.TryGetComponent(out SoldierMovement soldierMovement))
            {
                soldierMovement.PlaceInDeployPosition(GetRandomPositionInArea());
            }

            if (willDeploySoldier.TryGetComponent(out MergeableCharacter mergeableCharacter))
            {
                IncreaseSoldier(1, mergeableCharacter.GetCharacterType());
                mergeableCharacters.Add(mergeableCharacter);

            }
        }

        public void RemoveSoldierFromList(TypeOfCharacters type)
        {
            mergeableCharacters.RemoveAt(mergeableCharacters.Count - 1);
            DecreaseSoldier(1, type);
        }

        #endregion
    }
}
