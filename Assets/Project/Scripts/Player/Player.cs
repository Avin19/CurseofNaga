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
        [SerializeField] private Transform _weaponPlacement;
        [SerializeField] private Transform _playerMain;

        private readonly Vector3 _LEFTFACING = new Vector3(-40f, 180f, 0f);
        private readonly Vector3 _RIGHTFACING = new Vector3(40f, 0f, 0f);
        private const float _LEFTFACINGWEAPONPLACEMENT = 1.45f;
        private const float _RIGHTFACINGWEAPONPLACEMENT = -1.2f;

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

        private void Update()
        {
            if ((_playerStatus & PlayerStatus.PERFORMING_ACTION) == 0)
            {

                HandleMovement();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Detected Collider: {other.name}");
        }

        Vector2 inputVector; //For debugging
        private void HandleMovement()
        {
            // Vector2 inputVector = gameInput.GetMovementVector();
            inputVector = gameInput.GetMovementVector();
            Vector3 moveDir;

            if (Mathf.Max(Mathf.Abs(inputVector.x), Mathf.Abs(inputVector.y)) > 0f)
            {
                // if ((_playerStatus & PlayerStatus.MOVING) == 0)
                {
                    _playerStatus &= ~PlayerStatus.IDLE;
                    _playerStatus |= PlayerStatus.MOVING;

                    if (inputVector.x < 0f)
                    // && ((_playerStatus & PlayerStatus.FACING_RIGHT) != 0)
                    {
                        if ((_playerStatus & PlayerStatus.FACING_LEFT) == 0)
                        {
                            _playerStatus &= ~PlayerStatus.FACING_RIGHT;
                            _playerStatus |= PlayerStatus.FACING_LEFT;

                            moveDir = _weaponPlacement.localPosition;
                            moveDir.x = _LEFTFACINGWEAPONPLACEMENT;
                            _weaponPlacement.localPosition = moveDir;

                            _playerMain.localEulerAngles = _LEFTFACING;
                            // Debug.Log($"Facing Left | inputVector: {inputVector}");
                        }
                    }
                    else if (inputVector.x > 0f)
                    // && ((_playerStatus & PlayerStatus.FACING_LEFT) != 0)
                    {
                        if ((_playerStatus & PlayerStatus.FACING_RIGHT) == 0)
                        {
                            _playerStatus &= ~PlayerStatus.FACING_LEFT;
                            _playerStatus |= PlayerStatus.FACING_RIGHT;

                            moveDir = _weaponPlacement.localPosition;
                            moveDir.x = _RIGHTFACINGWEAPONPLACEMENT;
                            _weaponPlacement.localPosition = moveDir;

                            _playerMain.localEulerAngles = _RIGHTFACING;
                            // Debug.Log($"Facing Right | inputVector: {inputVector}");
                        }
                    }
                }
            }
            else
            {
                _playerStatus &= ~PlayerStatus.MOVING;
                _playerStatus |= PlayerStatus.IDLE;
            }

            moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
            transform.position += moveDir * movSpeed * Time.deltaTime;
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
    }
}