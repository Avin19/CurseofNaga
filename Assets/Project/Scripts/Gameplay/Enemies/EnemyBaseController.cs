using UnityEngine;

using static CurseOfNaga.Global.UniversalConstant;

namespace CurseOfNaga.Gameplay.Enemies
{
    public class EnemyBaseController : MonoBehaviour
    {
        internal enum EnemyStatus
        {
            IDLE = 0,
            MOVING = 1 << 1,
            PLAYER_VISIBLE = 1 << 2,
            CHASING_PLAYER = 1 << 3,
            REACHED_PLAYER = 1 << 4,
            ATTACKING_PLAYER = 1 << 5,
            DEAD = 1 << 6
        }

        private EnemyStatus _enemyStatus;
        public float _speedMult = 2f;                 //Set to protected
        public int _VISIBILITYTHRESHOLD = 10;              //Set to const?
        public float _PROXIMITYTHRESHOLD = 10;              //Set to const?

        void Start()
        {
            _enemyStatus = EnemyStatus.IDLE;
        }

        private void Update()
        {
            if ((MainGameplayManager.Instance.GameStatus & GameStatus.LOADED) == 0)
                return;

            Vector3 tempVec = transform.position - MainGameplayManager.Instance.PlayerTransform.position;

            if ((_enemyStatus & EnemyStatus.PLAYER_VISIBLE) == 0
                && (tempVec.sqrMagnitude <= _VISIBILITYTHRESHOLD * _VISIBILITYTHRESHOLD))
            {
                _enemyStatus &= ~EnemyStatus.IDLE;
                _enemyStatus |= EnemyStatus.PLAYER_VISIBLE;
            }
            else
            {
                _enemyStatus |= EnemyStatus.CHASING_PLAYER;
                MoveTowardsPlayer();
            }
        }

        private void MoveTowardsPlayer()
        {
            // if ((_enemyStatus & EnemyStatus.PLAYER_VISIBLE) == 0) return;

            Vector3 playerDir = transform.position - MainGameplayManager.Instance.PlayerTransform.position;

            // Enemy is close enough to Player
            if (playerDir.sqrMagnitude <= _PROXIMITYTHRESHOLD * _PROXIMITYTHRESHOLD)
            {
                _enemyStatus |= EnemyStatus.REACHED_PLAYER;
                return;
            }

            transform.position -= playerDir.normalized * _speedMult * Time.deltaTime;
        }
    }
}