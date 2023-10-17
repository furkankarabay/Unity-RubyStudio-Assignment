using RubyGame.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace RubyGame.GamePlay
{

    public class StackableCharacter : MonoBehaviour, IStackable
    {
        public bool isStackable = true;

        [HideInInspector] public CollectableArea locatedArea;
        [HideInInspector] public bool isStacked = false;
        [HideInInspector] public int index;

        public void LeaveFromStack()
        {
            isStackable = false;
            isStacked = false;
        }

        public void QueueInStack(Transform parent, Vector3 position)
        {
            locatedArea.StackableCharacterCollected(index);

            transform.SetParent(parent);
            transform.DOLocalRotate(Vector3.zero, 0.2f);
            transform.DOLocalMove(position, 0.2f);
        }
    }
}
