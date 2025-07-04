using System.Threading.Tasks;
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

        #region Animation
        private Animator _enemyAC;
        private const string _ENEMY_STATUS = "EnemyStatus";
        private const string _IDLE = "Enemy_Idle", _MOVE = "Enemy_Move", _DODGE = "Enemy_Dodge",
            _ATTACK = "Enemy_Attack";
        #endregion Animation

        private void OnDisable()
        {
            MainGameplayManager.Instance.OnEnemyStatusUpdate -= HandleStatusChange;
        }

        private void OnEnable()
        {
            MainGameplayManager.Instance.OnEnemyStatusUpdate += HandleStatusChange;
        }

        void Start()
        {
            _enemyAC = transform.GetComponent<Animator>();
            _enemyStatus = EnemyStatus.IDLE;
            _Health = _OgHealth;
        }

        private void Update()
        {
            if ((MainGameplayManager.Instance.GameStatus & GameStatus.LOADED) == 0)
                return;

            Vector3 tempVec = transform.position - MainGameplayManager.Instance.PlayerTransform.position;

            if (tempVec.sqrMagnitude <= _VISIBILITYTHRESHOLD * _VISIBILITYTHRESHOLD)
            // && (_enemyStatus & EnemyStatus.PLAYER_VISIBLE) == 0)
            {
                _enemyStatus &= ~EnemyStatus.IDLE;
                _enemyStatus |= EnemyStatus.PLAYER_VISIBLE;
                _enemyStatus |= EnemyStatus.CHASING_PLAYER;
                ChasePlayer();
            }
            else if ((_enemyStatus & EnemyStatus.PLAYER_VISIBLE) != 0)
            {
                _enemyStatus &= ~EnemyStatus.PLAYER_VISIBLE;
                _enemyStatus &= ~EnemyStatus.CHASING_PLAYER;
            }
            // else if ((_enemyStatus & EnemyStatus.REACHED_PLAYER) != 0)
            // {

            // }

            // if ((_enemyStatus & EnemyStatus.PLAYER_VISIBLE) != 0)
            // {
            //     _enemyStatus |= EnemyStatus.CHASING_PLAYER;
            //     MoveTowardsPlayer();
            // }
        }

        private void ChasePlayer()
        {
            // if ((_enemyStatus & EnemyStatus.PLAYER_VISIBLE) == 0) return;

            Vector3 playerDir = transform.position - MainGameplayManager.Instance.PlayerTransform.position;

            // Enemy is close enough to Player
            if (playerDir.sqrMagnitude <= _PROXIMITYTHRESHOLD * _PROXIMITYTHRESHOLD)
            {
                _enemyStatus |= EnemyStatus.REACHED_PLAYER;
                // MakeDecision();
                return;
            }

            transform.position -= playerDir.normalized * _speedMult * Time.deltaTime;
        }

        // Decision on what to do
        private async void MakeDecision()
        {
            //Consider possibilities of what enemy can do and then execute accordingly


            int randomDecisionTime = Random.Range(500, 5000);       //0.5s - 5s

            await Task.Delay(randomDecisionTime);
            // MakeDecision();
        }

        private void RoamAroundTheArea() { }

        //Async as attacks would need to be chained
        private async void AttackPlayer()
        {

        }

        private void HandleStatusChange(EnemyStatus status, int transformID, float value)
        {
            switch (status)
            {
                case EnemyStatus.ENEMY_WITHIN_PLAYER_RANGE:
                    if (value == 1)
                        _enemyStatus |= EnemyStatus.ENEMY_WITHIN_PLAYER_RANGE;
                    else
                        _enemyStatus &= ~EnemyStatus.ENEMY_WITHIN_PLAYER_RANGE;

                    break;

                case EnemyStatus.PLAYER_ATTACKING:
                    if ((_enemyStatus & EnemyStatus.ENEMY_WITHIN_PLAYER_RANGE) != 0)
                        GetDamage(value);
                    break;
            }
        }

        private void PlayAnimation(EnemyStatus enemyStatus)
        {
            switch (enemyStatus)
            {
                case EnemyStatus.IDLE:
                    _enemyAC.SetInteger(_ENEMY_STATUS, 0);
                    _enemyAC.Play(_IDLE);

                    break;

                case EnemyStatus.MOVING:
                    _enemyAC.SetInteger(_ENEMY_STATUS, (int)enemyStatus);
                    _enemyAC.Play(_MOVE);

                    break;

                case EnemyStatus.DODGING:
                    _enemyAC.SetInteger(_ENEMY_STATUS, (int)enemyStatus);
                    _enemyAC.Play(_DODGE);

                    break;

                case EnemyStatus.ATTACKING:
                    _enemyAC.SetInteger(_ENEMY_STATUS, (int)enemyStatus);
                    _enemyAC.Play(_ATTACK);

                    break;
            }
        }

        private async void UnsetAction_Async(EnemyStatus status)
        {
            switch (status)
            {
                case EnemyStatus.IDLE:
                    PlayAnimation(EnemyStatus.IDLE);

                    break;

                case EnemyStatus.ATTACKING:
                    await Task.Delay(500);
                    _enemyStatus &= ~EnemyStatus.ATTACKING;

                    goto case EnemyStatus.IDLE;
            }
        }

        public void GetDamage(float damage)
        {
            // Debug.Log($"Received Damage: {damage}");

            if (_Health <= 0)
            {
                _enemyStatus &= ~EnemyStatus.ENEMY_WITHIN_PLAYER_RANGE;
                MainGameplayManager.Instance.OnEnemyStatusUpdate?.Invoke(EnemyStatus.DEAD, transform.GetInstanceID(), -1);
                gameObject.SetActive(false);
            }

            _Health -= damage;
        }
    }
}