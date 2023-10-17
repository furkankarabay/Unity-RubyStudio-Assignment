using RubyGame.GamePlay;
using RubyGame.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace RubyGame.Player
{
    public class StackArmyController : MonoBehaviour
    {
        #region Variables

        public List<StackableCharacter> stack;
        public bool doesHaveStack = false;

        [SerializeField] private Transform parentStack;
        [SerializeField] private Transform firstStackPosition;
        [SerializeField] private int stackCapacity;

        private int stackCount;
        #endregion

        #region Unity Events

        private void OnEnable()
        {
            EventsManager.OnDeploySoldierToBarrack += RemoveCharacterFromStack;
        }

        private void OnDisable()
        {
            EventsManager.OnDeploySoldierToBarrack -= RemoveCharacterFromStack;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                if (other.TryGetComponent(out StackableCharacter stackableCharacter))
                {
                    if (!stackableCharacter.isStackable || stackableCharacter.isStacked)
                        return;

                    if (stackCapacity > stackCount)
                    {
                        stackableCharacter.isStacked = true;

                        stack.Add(stackableCharacter);
                        stackCount++;
                        doesHaveStack = true;
                        Vector3 stackPosition = new Vector3(firstStackPosition.localPosition.x, firstStackPosition.localPosition.y,
                            firstStackPosition.localPosition.z - (stackCount * 0.75f));

                        stackableCharacter.QueueInStack(parentStack, stackPosition);
                    }
                }
            }
        }

        #endregion

        #region Methods

        private void RemoveCharacterFromStack(Transform parent, GameObject willDeployCharacter)
        {
            if (willDeployCharacter.TryGetComponent(out StackableCharacter stackableCharacter))
            {
                stack.Remove(stackableCharacter);

                stackableCharacter.LeaveFromStack();
                stackCount--;

                if (stackCount == 0)
                    doesHaveStack = false;
            }
        }

        public GameObject GetLastCharacterFromStack()
        {
            return stack[stack.Count - 1].gameObject;
        }


        public void AnimateStackedArmy(bool isMoving)
        {
            for (int i = 0; i < stack.Count; i++)
            {
                if (stack[i].TryGetComponent(out SoldierMovement soldierMovement))
                    soldierMovement.Animate(isMoving);
            }
        }

        #endregion
    }
}
