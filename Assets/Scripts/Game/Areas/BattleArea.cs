using RubyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BrokenVector.LowPolyFencePack;

namespace RubyGame.GamePlay
{
    public class BattleArea : MonoBehaviour
    {
        #region Variables
        [Header("Settings")]
        [SerializeField] private int startBattleTime;
        [SerializeField] private DoorController doorController;

        [Header("UI")]
        [SerializeField] private Image circle;
        [SerializeField] private Image fillImage;
        private bool onTriggerArea = false;

        #endregion

        #region Unity Events
        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                if (other.CompareTag("Player"))
                {
                    if (BarrackManager.instance.DoesHaveAnySoldier())
                    {
                        onTriggerArea = true;
                        StartBattleInteraction();
                    }
                    else
                    {
                        circle.DOColor(Color.red, 0.6f).OnComplete(() => circle.DOColor(Color.white, 0.6f));
                    }
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
                    CancelBattleAreaInteraction();
                }
            }
        }

        #endregion

        #region Methods

        private void OnBattleAreaInteractionTimeCompleted()
        {
            EventsManager.OnInteractedBattleArea?.Invoke(true);
            doorController.OpenDoor();
        }

        private void CancelBattleAreaInteraction()
        {
            DOTween.Kill(fillImage);
            fillImage.DOFillAmount(0, 0.5f);
            EventsManager.OnInteractedBattleArea?.Invoke(false);
        }

        private void StartBattleInteraction()
        {
            if (onTriggerArea)
            {
                fillImage.DOFillAmount(1, startBattleTime).OnComplete(() => OnBattleAreaInteractionTimeCompleted());
            }
        }
        #endregion
    }
}

