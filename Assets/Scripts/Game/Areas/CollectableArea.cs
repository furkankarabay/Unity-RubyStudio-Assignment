using RubyGame.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace RubyGame.GamePlay
{
    public class CollectableArea : MonoBehaviour
    {
        #region Variables

        [Header("Settings")]

        [SerializeField] private TypeOfCharacters typeOfCollectable;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private int cost;
        [SerializeField] private int productionTime;
        [SerializeField] private int waitingAreaCapacity;
        [SerializeField] private bool isAvailableToUse;


        [Header("UI")]

        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Image lockImage;
        [SerializeField] private Image circle;


        private bool onTriggerArea = false;
        private bool isWaitingAreaFull;
        private int waitingAreaCount;

        [HideInInspector] public List<StackableCharacter> stackableCharacters;

        #endregion


        #region Unity Events

        private void Awake()
        {
            for (int i = 0; i < waitingAreaCapacity; i++)
            {
                stackableCharacters.Add(null);
            }

            lockImage.gameObject.SetActive(!isAvailableToUse);
            costText.gameObject.SetActive(isAvailableToUse);

            costText.text = cost.ToString() + "$";
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                if (other.CompareTag("Player"))
                {
                    if (!isAvailableToUse || MoneyManager.Instance.GetCurrentMoney() < cost)
                    {
                        circle.DOColor(Color.red, 0.6f).OnComplete(() => circle.DOColor(Color.white, 0.6f));
                        return;

                    }

                    onTriggerArea = true;
                    StartProduction();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != null)
            {
                if (other.CompareTag("Player"))
                {
                    onTriggerArea = false;
                    CancelProduction();
                }
            }
        }

        #endregion


        #region Methods

        private int GetEmptySpawnPointIndex()
        {
            for (int i = 0; i < stackableCharacters.Count; i++)
            {
                if (stackableCharacters[i] == null)
                    return i;
            }

            return -1;
        }
        public void StackableCharacterCollected(int index)
        {
            stackableCharacters[index] = null;

            waitingAreaCount--;

            if (waitingAreaCount < waitingAreaCapacity)
                isWaitingAreaFull = false;
        }

        private void OnProductionCompleted()
        {
            MoneyManager.Instance.DecreaseMoney(cost);
            int spawnPointIndex = GetEmptySpawnPointIndex();
            CharacterSpawnManager.instance.SpawnStackableCharacter(TypeOfCharacters.villager, spawnPoints[spawnPointIndex].position, this, spawnPointIndex);
            waitingAreaCount++;

            if (waitingAreaCapacity == waitingAreaCount)
                isWaitingAreaFull = true;

            fillImage.DOFillAmount(0, 0.4f).OnComplete(() => StartProduction());

        }

        private void CancelProduction()
        {
            DOTween.Kill(fillImage);
            fillImage.DOFillAmount(0, 0.5f);
        }

        private void StartProduction()
        {
            if (onTriggerArea)
            {
                if (isWaitingAreaFull)
                    return;

                if (MoneyManager.Instance.GetCurrentMoney() < cost)
                    return;

                fillImage.DOFillAmount(1, productionTime).OnComplete(() => OnProductionCompleted());
            }
        }

        public int GetStackableCharacterIndex()
        {
            return waitingAreaCount;
        }
        #endregion
    }
}


