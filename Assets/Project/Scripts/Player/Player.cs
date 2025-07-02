#define TESTING

using System;
using UnityEngine;

using static CurseOfNaga.Global.UniversalConstant;

namespace CurseOfNaga.Gameplay
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private GameInput gameInput;
        [SerializeField] private float movSpeed = 7f;

        private PlayerStatus _playerStatus;

        private void OnDisable()
        {
            MainGameplayManager.Instance.OnObjectiveVisible -= UpdatePlayerStatus;
        }

        private void Start()
        {
#if TESTING
            movSpeed = 25f;
#endif

            MainGameplayManager.Instance.OnObjectiveVisible += UpdatePlayerStatus;
        }

        private void UpdatePlayerStatus(PlayerStatus playerStatus)
        {
            switch (playerStatus)
            {
                case PlayerStatus.IDLE:
                    _playerStatus = PlayerStatus.IDLE;
                    break;

                case PlayerStatus.MOVING:
                    break;

                case PlayerStatus.INVOKED_CUTSCENE:
                    _playerStatus |= PlayerStatus.PERFORMING_ACTION;
                    break;

                case PlayerStatus.IN_CUTSCENE:
                    break;
            }

            _playerStatus |= playerStatus;
        }

        void Update()
        {

            if ((_playerStatus & PlayerStatus.PERFORMING_ACTION) == 0)
                HandleMovement();

        }

        private void HandleMovement()
        {
            Vector2 inputVector = gameInput.GetMovementVector();

            if (Mathf.Max(Mathf.Abs(inputVector.x), Mathf.Abs(inputVector.y)) > 0f)
            {
                _playerStatus &= ~PlayerStatus.IDLE;
                _playerStatus |= PlayerStatus.MOVING;
            }
            else
            {
                _playerStatus &= ~PlayerStatus.MOVING;
                _playerStatus |= PlayerStatus.IDLE;
            }

            Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);
            transform.position += movDir * movSpeed * Time.deltaTime;
        }
    }
}