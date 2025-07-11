using _0.DucLib.Scripts.Machine;
using _0.DucTALib.Character.Scripts.State;
using UnityEngine;
using KinematicCharacterController.Examples;
using ThirdPersonCamera;

namespace _0.DucTALib.Character.Scripts
{
    public class D_CharacterMovement : StateMachine
    {
        [SerializeField] private CameraController CameraController;
        [SerializeField] private float maxMoveSpeed;
        public bool isStop;
        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int OnGround = Animator.StringToHash("OnGround");
        private ExampleCharacterController Character;
        private Animator Animator;

        private Vector2 direction;
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        public void OnJoyMove(Vector2 assist)
        {
            direction = assist;
        }

        public void OnJoyEnd()
        {
            direction = Vector2.zero;
        }

        private void Awake()
        {
            Character = GetComponent<ExampleCharacterController>();
            Animator = GetComponent<Animator>();

            if (CameraController == null)
            {
                CameraController = FindObjectOfType<CameraController>();

                if (CameraController == null)
                {
                    enabled = false;
                }
            }
        }

        private void FixedUpdate()
        {
            HandleCharacterInput();
        }

        public void StopMovement()
        {
           
        }

        public void ResumeMovement()
        {
           
        }
        private void HandleCharacterInput()
        {
            Vector2 combinedInput = new Vector2(
                direction.x + Input.GetAxisRaw(HorizontalInput),
                direction.y + Input.GetAxisRaw(VerticalInput)
            ).normalized;
            if(isStop) combinedInput = Vector2.zero;
            if (combinedInput != Vector2.zero)
            {
                if (Character.MaxStableMoveSpeed < maxMoveSpeed)
                {
                    Character.MaxStableMoveSpeed += (Time.deltaTime * 1.5f);
                }

                if (Character.MaxStableMoveSpeed > 4)
                {
                    ChangeState<D_CharacterRun>();
                }
                else
                {
                    ChangeState<D_CharacterWalk>();
                }
               
            }
            else
            {
                Character.MaxStableMoveSpeed = Character.MaxStableMoveSpeedBase;
                ChangeState<D_CharacterIdle>();
            }
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs
            {
                // Cấu trúc CharacterInputs
                MoveAxisForward = combinedInput.y,
                MoveAxisRight = combinedInput.x,
                JumpDown = Input.GetKeyDown(KeyCode.Space),
                CrouchDown = Input.GetKeyDown(KeyCode.C),
                CrouchUp = Input.GetKeyUp(KeyCode.C),
                CameraRotation = CameraController.transform.rotation
            };

            Character.SetInputs(ref characterInputs);
            
        }
    }
}