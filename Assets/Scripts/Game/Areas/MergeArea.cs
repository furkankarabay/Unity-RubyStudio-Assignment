using RubyGame.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace RubyGame.GamePlay
{

    public class MergeArea : MonoBehaviour
    {
        #region Variables

        [Header("Settings")]

        [SerializeField] private int startMergeTime;

        [Header("UI")]

        [SerializeField] private Image fillImage;
        [SerializeField] private Image circle;

        private bool onTriggerArea = false;

        #endregion

        #region Unity Events
        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                if (other.CompareTag("Player"))
                {
                    if (!BarrackManager.instance.DoesHaveAnySoldier())
                    {
                        circle.DOColor(Color.red, 0.6f).OnComplete(() => circle.DOColor(Color.white, 0.6f));
                        return;
                    }

                    onTriggerArea = true;
                    StartMerge();
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

        private void OnMergeTimeCompleted()
        {
            EventsManager.OnInteractedMergeArea?.Invoke(true);
        }

        private void CancelProduction()
        {
            DOTween.Kill(fillImage);
            fillImage.DOFillAmount(0, 0.5f);
            EventsManager.OnInteractedMergeArea?.Invoke(false);
        }

        private void StartMerge()
        {
            if (onTriggerArea)
            {
                fillImage.DOFillAmount(1, startMergeTime).OnComplete(() => OnMergeTimeCompleted());
            }
        }

        #endregion
    }

}
