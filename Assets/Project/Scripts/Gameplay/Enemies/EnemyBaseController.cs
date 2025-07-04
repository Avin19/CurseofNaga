using UnityEngine;

using static CurseOfNaga.Global.UniversalConstant;

namespace CurseOfNaga.Gameplay.Enemies
{
    public class EnemyBaseController : MonoBehaviour
    {
        protected float _OgHealth = 100f;
        protected float _Health;

        private EnemyStatus _enemyStatus;
        public float _speedMult = 2f;                 //Set to protected
        public int _VISIBILITYTHRESHOLD = 10;              //Set to const?
        public float _PROXIMITYTHRESHOLD = 10;              //Set to const?

        private void OnDestroy()
        {
            MainGameplayManager.Instance.OnEnemyStatusUpdate -= HandleStatusChange;
        }

        void Start()
        {
            _enemyStatus = EnemyStatus.IDLE;
            _Health = _OgHealth;

            MainGameplayManager.Instance.OnEnemyStatusUpdate += HandleStatusChange;
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

        private void HandleStatusChange(EnemyStatus status, int transformID, float damage)
        {
            switch (status)
            {
                case EnemyStatus.BEING_ATTACKED:
                    if (transformID == transform.GetInstanceID())
                        GetDamage(damage);
                    break;
            }
        }

        public void GetDamage(float damage)
        {
            _Health -= damage;

            if (_Health <= 0)
            {
                MainGameplayManager.Instance.OnEnemyStatusUpdate?.Invoke(EnemyStatus.DEAD, transform.GetInstanceID(), -1);
                gameObject.SetActive(false);
            }
        }
    }
}