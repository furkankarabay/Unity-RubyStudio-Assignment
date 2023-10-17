using DG.Tweening;
using RubyGame.GamePlay;
using UnityEngine;

namespace RubyGame.GamePlay
{
    public class SoldierMovement : MonoBehaviour
    {

        #region Variables

        [SerializeField] private MergeableCharacter mergeableCharacter;
        [SerializeField] private Animator animator;

        #endregion

        #region Unity Events


        #endregion

        #region Methods

        public void PlaceInDeployPosition(Vector3 position)
        {
            transform.DOMove(position, 1).OnComplete(() => PlaceInBarrackStarterPosition());
        }

        private void PlaceInBarrackStarterPosition()
        {
            mergeableCharacter.SetSoliderMergeable();
            //splineFollower.enabled = true;
            //splineFollower.spline = GetRandomStarterPositionInBarrack();
        }

        public void Animate(bool isRunning)
        {
            animator.SetBool("Run", isRunning);
        }

        #endregion
    }
}

