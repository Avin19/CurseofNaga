using UnityEngine;

namespace CurseOfNaga.Global
{
    public class UniversalConstant
    {
        public enum GameStatus { DEFAULT, LOADED }

        public enum ObjectiveType { ACTIVE, INACTIVE, CURRENT, INVOKE_CUTSCENE }

        public enum PlayerStatus
        {
            IDLE = 0,
            MOVING = 1 << 0,
            FACING_LEFT = 1 << 1,
            FACING_RIGHT = 1 << 2,
            INVOKED_CUTSCENE = 1 << 3,
            IN_CUTSCENE = 1 << 4,
            PERFORMING_ACTION = 1 << 5,                 // Player cant move while performing this
            PERFORMING_ADDITIVE_ACTION = 1 << 6,         // Player can move while performing this
            JUMPING = 1 << 7,
            ROLLING = 1 << 8,
            ATTACKING = 1 << 9,
            INTERACTING = 1 << 10,
        }

        public enum InputStatus
        {
            DEFAULT = 0,
            MOVE = 1 << 1,
            JUMP = 1 << 2,
            ROLL = 1 << 3,
            INTERACT = 1 << 4,
            ATTACK = 1 << 5
        }
    }
}