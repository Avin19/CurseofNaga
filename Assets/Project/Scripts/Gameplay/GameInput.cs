
using UnityEngine;

using static CurseOfNaga.Global.UniversalConstant;

using InputAction = UnityEngine.InputSystem.InputAction;

namespace CurseOfNaga.Gameplay
{
    public class GameInput : MonoBehaviour
    {
        private PlayerController playerInput;

        public InputStatus CurrentInputStatus;

        void Awake()
        {
            playerInput = new PlayerController();
            playerInput.Player.Enable();

            playerInput.Player.Jump.performed += (ctx) => SetAction(ctx, InputStatus.JUMP);
            playerInput.Player.Jump.started += (ctx) => SetAction(ctx, InputStatus.JUMP);
            playerInput.Player.Jump.canceled += (ctx) => SetAction(ctx, InputStatus.JUMP);

            playerInput.Player.Roll.performed += (ctx) => SetAction(ctx, InputStatus.ROLL);
            playerInput.Player.Roll.canceled += (ctx) => SetAction(ctx, InputStatus.ROLL);

            playerInput.Player.Interact.performed += (ctx) => SetAction(ctx, InputStatus.INTERACT);
            playerInput.Player.Interact.canceled += (ctx) => SetAction(ctx, InputStatus.INTERACT);

            playerInput.Player.Attack.performed += (ctx) => SetAction(ctx, InputStatus.ATTACK);
            playerInput.Player.Attack.canceled += (ctx) => SetAction(ctx, InputStatus.ATTACK);
        }

        private void SetAction(InputAction.CallbackContext context, InputStatus status)
        {
            switch (status)
            {
                case InputStatus.JUMP:
                    {
                        float contextValue = context.ReadValue<float>();
                        if (contextValue > 0)
                            CurrentInputStatus |= InputStatus.JUMP;
                        else
                            CurrentInputStatus &= ~InputStatus.JUMP;
                        // Debug.Log($"Jump: {context.ReadValue<float>()}");
                    }

                    break;

                case InputStatus.ROLL:
                    {
                        float contextValue = context.ReadValue<float>();
                        if (contextValue > 0)
                            CurrentInputStatus |= InputStatus.ROLL;
                        else
                            CurrentInputStatus &= ~InputStatus.ROLL;
                        // Debug.Log($"Roll: {context.ReadValue<float>()}");
                    }

                    break;

                case InputStatus.INTERACT:
                    {
                        float contextValue = context.ReadValue<float>();
                        if (contextValue > 0)
                            CurrentInputStatus |= InputStatus.INTERACT;
                        else
                            CurrentInputStatus &= ~InputStatus.INTERACT;
                        // Debug.Log($"Interact: {context.ReadValue<float>()}");
                    }

                    break;

                case InputStatus.ATTACK:
                    {
                        float contextValue = context.ReadValue<float>();
                        if (contextValue > 0)
                            CurrentInputStatus |= InputStatus.ATTACK;
                        else
                            CurrentInputStatus &= ~InputStatus.ATTACK;
                        // Debug.Log($"Attack: {context.ReadValue<float>()}");
                    }

                    break;
            }
        }

        public Vector2 GetMovementVector()
        {
            Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();
            // Normalized Vector
            inputVector = inputVector.normalized;

            return inputVector;
        }
    }
}