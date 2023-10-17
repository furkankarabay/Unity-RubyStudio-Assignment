using RubyGame.Managers;
using UnityEngine;

namespace RubyGame.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Variables

        public VariableJoystick joystick;
        public CharacterController controller;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;

        [SerializeField] Animator playerAnimator;
        [SerializeField] Canvas inputCanvas;
        [SerializeField] StackArmyController armyController;
        public bool isJoystick;

        private Vector3 movementDirection;
        private Vector3 targetDirection;

        private bool canControl = true;
        private bool isMoving = false;

        #endregion

        #region Unity Events

        private void Start()
        {
            EnableJoystickInput();
        }

        private void OnEnable()
        {
            EventsManager.OnTouchedMergeableSoldier += ToggleJoystickControl;
            EventsManager.OnInteractedBattleArea += ToggleJoystickControl;

        }

        private void OnDisable()
        {
            EventsManager.OnTouchedMergeableSoldier -= ToggleJoystickControl;
            EventsManager.OnInteractedBattleArea -= ToggleJoystickControl;
        }


        private void Update()
        {
            if (isJoystick)
            {
                if (!canControl)
                    return;

                Movement();

                Animate();
            }
        }

        #endregion

        #region Methods


        private void ToggleJoystickControl(bool isControlActive)
        {
            canControl = !isControlActive;
        }

        public void EnableJoystickInput()
        {
            isJoystick = true;
            inputCanvas.gameObject.SetActive(true);
        }



        private void Movement()
        {
            movementDirection = new Vector3(joystick.Direction.x, 0.0f, joystick.Direction.y);
            controller.SimpleMove(movementDirection * movementSpeed);

            targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection,
                    rotationSpeed * Time.deltaTime, 0.0f);

            controller.transform.rotation = Quaternion.LookRotation(targetDirection);
        }

        private void Animate()
        {
            armyController.AnimateStackedArmy(isMoving);

            if (movementDirection.sqrMagnitude <= 0)
            {

                isMoving = false;
                playerAnimator.SetBool("Run", false);
                return;
            }

            isMoving = true;
            playerAnimator.SetBool("Run", true);

        }


        #endregion

    }
}
