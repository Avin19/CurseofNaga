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
            PERFORMING_ADDITIVE_ACTION = 1 << 6         // Player can move while performing this
        }
    }
}