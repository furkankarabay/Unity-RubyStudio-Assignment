using RubyGame.Interfaces;
using RubyGame.Managers;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace RubyGame.GamePlay
{
    public class MergeableCharacter : MonoBehaviour, IMergeable
    {
        #region Variables

        [SerializeField] TypeOfCharacters type;
        [SerializeField] private BoxCollider soldierCollider;
        [SerializeField] private Rigidbody rigidbodyCharacter;

        public bool isMergeable = false;
        [HideInInspector] public bool isHoldingByPlayer = false;
        [HideInInspector] public bool canBeHold = false;

        private Vector3 positionBeforeHeld;
        private bool droppedInsideBarrackArea = false;
        #endregion

        #region Unity Events

        private void OnTriggerEnter(Collider other)
        {
            if (!isHoldingByPlayer)
                return;

            if (other.CompareTag("MergeDropArea"))
                droppedInsideBarrackArea = true;

            if (!droppedInsideBarrackArea)
                return;

            if (other.CompareTag("Ground"))
            {
                ResetVariablesAfterPlacedGround();
            }

            if (other.TryGetComponent(out MergeableCharacter mergeableCharacter))
            {
                if (mergeableCharacter.isMergeable)
                {
                    if (mergeableCharacter.type == type)
                    {
                        EventsManager.OnMergeTriggered(mergeableCharacter.gameObject, gameObject, type);
                    }
                    else
                    {
                        ResetPosition();
                    }
                }
            }
        }

        #endregion

        #region Methods
        IEnumerator CheckIfDroppedBarrackArea()
        {
            yield return new WaitForSecondsRealtime(0.6f);

            if (!droppedInsideBarrackArea)
                ResetPosition();

        }
        public void ReleasedCharacter()
        {
            StartCoroutine(CheckIfDroppedBarrackArea());
            canBeHold = false;
            rigidbodyCharacter.isKinematic = false;
            rigidbodyCharacter.useGravity = true;
        }

        public void HoldingForMerge()
        {
            positionBeforeHeld = transform.position;
            rigidbodyCharacter.useGravity = false;
            isHoldingByPlayer = true;
        }

        public void SetSoliderMergeable()
        {
            soldierCollider.isTrigger = false;
        }

        public void ResetPosition()
        {
            transform.DOMove(positionBeforeHeld, 0.25f).
                OnComplete(() => ResetVariablesAfterPlacedGround());
        }

        private void ResetVariablesAfterPlacedGround()
        {
            canBeHold = true;
            isHoldingByPlayer = false;
            droppedInsideBarrackArea = false;
        }

        public TypeOfCharacters GetCharacterType()
        {
            return type;
        }
        #endregion
    }
}
