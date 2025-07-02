using UnityEngine;

namespace CurseOfNaga.Global
{
    public class UniversalConstant
    {
        public enum ObjectiveType { ACTIVE, INACTIVE, CURRENT, INVOKE_CUTSCENE }

        public enum PlayerStatus
        {
            IDLE = 0,
            MOVING = 1 << 0,
            INVOKED_CUTSCENE = 1 << 1,
            IN_CUTSCENE = 1 << 2,
            PERFORMING_ACTION = 1 << 3,                 // Player cant move while performing this
            PERFORMING_ADDITIVE_ACTION = 1 << 4         // Player can move while performing this
        }
    }
}