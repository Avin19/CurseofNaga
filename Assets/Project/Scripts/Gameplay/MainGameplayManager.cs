#define DEBUG_VISIBLE_AREA

using System;
using System.Collections.Generic;

using UnityEngine;

using CurseOfNaga.Global;

namespace CurseOfNaga.Gameplay
{
    public class MainGameplayManager : MonoBehaviour
    {
        #region Singleton
        private static MainGameplayManager _instance;
        public static MainGameplayManager Instance { get => _instance; }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this.gameObject);
        }
        #endregion Singleton

        public List<Transform> _objectiveTransforms;
        public Transform PlayerTransform;

        public Action<UniversalConstant.Objective> OnPlayerReachedPosition;

        void Start()
        {

        }

        void Update()
        {
        }


        private void CheckIfObjectiveVisible()
        {
            Vector2 _visibleArea = new Vector2(102.20f, 57.7f);
            Vector3 offsetPos;
            Vector3 OffsetPosDebug = new Vector3(0f, 0f, 40.7f);
            float innerDistance;
            for (int i = 0; i < _objectiveTransforms.Count; i++)
            {
                offsetPos = _objectiveTransforms[i].position - (PlayerTransform.position + OffsetPosDebug);
                innerDistance = CheckIfPointVisible(offsetPos, _visibleArea);
                if (innerDistance <= 0f)
                {
                    Debug.Log($"Point Visible: {_objectiveTransforms[i].position} | innerDistance: {innerDistance}");
                }
            }
        }

        // https://iquilezles.org/articles/distfunctions2d
        // float sdBox(in vec2 p, in vec2 b)
        // {
        //     vec2 d = abs(p) - b;
        //     return length(max(d, 0.0)) + min(max(d.x, d.y), 0.0);
        // }

        private float CheckIfPointVisible(Vector2 pointFromBox, in Vector2 boxDimensions)
        {
            pointFromBox.x = Mathf.Abs(pointFromBox.x);
            pointFromBox.y = Mathf.Abs(pointFromBox.x);
            Vector2 absDistFromBox = pointFromBox - boxDimensions;
            return (Mathf.Min(Mathf.Max(absDistFromBox.x, absDistFromBox.y), 0f));
            // bool visible = (Mathf.Min(Mathf.Max(absDistFromBox.x, absDistFromBox.y), 0f) <= 0f);
            // return visible;
        }

#if DEBUG_VISIBLE_AREA
        private void OnDrawGizmos()
        {
            Vector2 _visibleArea = new Vector2(102.20f, 57.7f);
            Vector3 OffsetPosDebug = new Vector3(0f, 0f, 40.7f);
            Vector3 offsetPos = PlayerTransform.position + OffsetPosDebug;
            Gizmos.DrawWireCube(offsetPos, new Vector3(_visibleArea.x * 2, 5f, _visibleArea.y * 2));
        }
#endif
    }
}