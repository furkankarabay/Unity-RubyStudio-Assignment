using RubyGame.Managers;
using UnityEngine;

namespace RubyGame.GamePlay
{

    public class SoldierCombat : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Animator animator;
        [SerializeField] private TypeOfCharacters typeOfCharacter;
        [SerializeField] private int health;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float minDistanceToAttack;
        [SerializeField] private float timeToAttack;

        private Transform closestEnemy;
        private float elapsedTimeAfterAttack;
        private float distanceBetweenSoldier;
        private bool isMoving = false;
        private bool isAttacking = false;
        private bool isFirstTimeAttack = true;

        #endregion

        #region Unity Events
        private void Start()
        {
            SetTarget();
        }

        private void Update()
        {
            if (closestEnemy == null)
            {
                if (BattleManager.Instance.IsThereAnyEnemyInArena())
                    SetTarget();

                isAttacking = false;
                isMoving = false;

                AnimateCharacter();

                return;
            }

            distanceBetweenSoldier = Vector3.Distance(closestEnemy.position, transform.position);

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

        private void SetTarget()
        {
            for (int i = 0; i < BattleManager.Instance.battleEnemies.Count; i++)
            {
                Transform currentEnemy = BattleManager.Instance.battleEnemies[i].transform;

                float minDistance = Vector3.Distance(transform.position, currentEnemy.position);

                if (!closestEnemy)
                    closestEnemy = currentEnemy;
                else
                {
                    float distance = Vector3.Distance(transform.position, currentEnemy.position);

                    if (distance < minDistance)
                    {
                        closestEnemy = currentEnemy;
                    }
                }
            }
        }

        private void AnimateCharacter()
        {
            animator.SetBool("Run", isMoving);
            animator.SetBool("Kick", isAttacking);

        }

        public void TakeDamage(int value)
        {
            health -= value;

            CheckHealth();
        }

        private void CheckHealth()
        {
            if (health <= 0)
            {
                BattleManager.Instance.RemoveSoldierFromList(gameObject, typeOfCharacter);
                Destroy(gameObject);
            }
        }

        private void RotateToTarget()
        {
            Vector3 targetDirection = closestEnemy.position - transform.position;
            targetDirection.y = 0;

            Quaternion newRotation = Quaternion.LookRotation(targetDirection);

            newRotation.eulerAngles = new Vector3(0, newRotation.eulerAngles.y, 0);

            transform.rotation = newRotation;
        }

        private void MoveToTarget()
        {
            Vector3 targetPosition = new Vector3(closestEnemy.position.x, transform.position.y, closestEnemy.position.z);

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

        private void AttackToTarget() // Calling from animation events.
        {
            if (!closestEnemy)
                return;

            if (closestEnemy.TryGetComponent(out Enemy enemy))
                enemy.TakeDamage(10);
        }

        #endregion
    }
}
