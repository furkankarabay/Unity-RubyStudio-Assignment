using RubyGame.Managers;
using UnityEngine;

namespace RubyGame.GamePlay
{
    public class Enemy : MonoBehaviour
    {

        #region Variables

        [SerializeField] private Animator animator;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float minDistanceToAttack;
        [SerializeField] private float timeToAttack;
        [SerializeField] private int health;

        private Transform closestSoldier;
        private float elapsedTimeAfterAttack;
        private float distanceBetweenSoldier;
        private bool isMoving = false;
        private bool isAttacking = false;
        private bool isFirstTimeAttack = true;

        #endregion

        #region Unity Events
        private void OnEnable()
        {
            EventsManager.OnPlacedSoldierInArena += SetTarget;
        }


        private void OnDisable()
        {
            EventsManager.OnPlacedSoldierInArena -= SetTarget;
        }
        private void Update()
        {
            if (closestSoldier == null)
            {
                if (BattleManager.Instance.IsThereAnySoldierInArena())
                    SetTarget();

                isAttacking = false;
                isMoving = false;

                AnimateCharacter();

                return;
            }

            distanceBetweenSoldier = Vector3.Distance(closestSoldier.position, transform.position);

            RotateToTarget();

            if (CheckIfEnoughDistanceToAttack())
            {
                isMoving = false;

                if (CheckCanAttack())
                {
                    isAttacking = true;
                }
            }
            else
            {
                isAttacking = false;

                MoveToTarget();
            }

            AnimateCharacter();
        }

        #endregion

        #region Methods

        public void TakeDamage(int value)
        {
            health -= value;

            CheckHealth();
        }

        private void CheckHealth()
        {
            if (health <= 0)
            {
                BattleManager.Instance.RemoveEnemyFromList(this);
                Destroy(gameObject);
            }
        }

        private void SetTarget()
        {
            for (int i = 0; i < BattleManager.Instance.battleSoldiers.Count; i++)
            {
                Transform currentSoldier = BattleManager.Instance.battleSoldiers[i].transform;

                float minDistance = Vector3.Distance(transform.position, currentSoldier.position);

                if (!closestSoldier)
                    closestSoldier = currentSoldier;
                else
                {
                    float distance = Vector3.Distance(transform.position, currentSoldier.position);

                    if (distance < minDistance)
                    {
                        closestSoldier = currentSoldier;
                    }
                }
            }
        }


        private void RotateToTarget()
        {
            Vector3 targetDirection = closestSoldier.position - transform.position;
            targetDirection.y = 0;

            Quaternion newRotation = Quaternion.LookRotation(targetDirection);

            newRotation.eulerAngles = new Vector3(0, newRotation.eulerAngles.y, 0);

            transform.rotation = newRotation;
        }

        private void MoveToTarget()
        {
            Vector3 targetPosition = new Vector3(closestSoldier.position.x, transform.position.y, closestSoldier.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            isMoving = true;
        }

        private bool CheckIfEnoughDistanceToAttack()
        {
            if (distanceBetweenSoldier < minDistanceToAttack)
                return true;

            return false;
        }

        private bool CheckCanAttack()
        {
            if (isFirstTimeAttack)
            {
                isFirstTimeAttack = false;
                return true;
            }

            if (timeToAttack < elapsedTimeAfterAttack)
            {
                elapsedTimeAfterAttack = 0;
                return true;
            }
            else
            {
                isAttacking = false;

                elapsedTimeAfterAttack += Time.deltaTime;
                return false;
            }

        }

        private void AnimateCharacter()
        {
            animator.SetBool("Run", isMoving);
            animator.SetBool("Kick", isAttacking);

        }

        private void AttackToTarget() // Calling from animation events.
        {
            if (!closestSoldier)
                return;

            if (closestSoldier.TryGetComponent(out SoldierCombat soldier))
                soldier.TakeDamage(10);
        }

        #endregion

    }
}
