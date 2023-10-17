using RubyGame.Managers;
using RubyGame.Player;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RubyGame.GamePlay
{
    public class DeployArea : MonoBehaviour
    {
        #region Variables

        [Header("Settings")]
        [SerializeField] private Transform barrackParent;
        [SerializeField] private int deployTime;

        [Header("UI")]
        [SerializeField] private Image fillImage;
        [SerializeField] private Image circle;


        private GameObject willDeployCharacter;
        private StackArmyController stackArmyController;
        private bool onTriggerArea = false;
        private bool isBarrackFull;
        private int barrackCapacity;
        private int numberOfPeopleIn;

        #endregion

        #region Unity Events
        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                if (other.TryGetComponent(out StackArmyController stackArmyController))
                {
                    if (stackArmyController.doesHaveStack)
                    {
                        this.stackArmyController = stackArmyController;

                        onTriggerArea = true;
                        StartDeploy();
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
                if (other.TryGetComponent(out StackArmyController stackArmyController))
                {
                    onTriggerArea = false;
                    CancelProduction();
                }
            }
        }

        #endregion

        #region Methods

        private void OnDeployCompleted()
        {
            numberOfPeopleIn++;

            willDeployCharacter = stackArmyController.GetLastCharacterFromStack();

            EventsManager.OnDeploySoldierToBarrack?.Invoke(barrackParent, willDeployCharacter);

            if (barrackCapacity == numberOfPeopleIn)
                isBarrackFull = true;

            fillImage.DOFillAmount(0, 0.4f).OnComplete(() => StartDeploy());

        }

        private void CancelProduction()
        {
            DOTween.Kill(fillImage);
            fillImage.DOFillAmount(0, 0.5f);
        }

        private void StartDeploy()
        {
            if (onTriggerArea)
            {
                if (isBarrackFull)
                    return;

                if (!stackArmyController.doesHaveStack)
                    return;

                fillImage.DOFillAmount(1, deployTime).OnComplete(() => OnDeployCompleted());
            }
        }

        #endregion

    }

}
