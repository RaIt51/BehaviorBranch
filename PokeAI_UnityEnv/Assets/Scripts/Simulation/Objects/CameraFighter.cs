using UnityEngine;

namespace Simulation.Objects
{
    /// <summary>
    /// Camera following the fighter
    /// </summary>
    public class CameraFighter : MonoBehaviour
    {
        /// <summary>
        /// Target looking at
        /// </summary>
        private BattleObject target;

        [Tooltip("Point keep focusing on")]
        [SerializeField]
        private Transform focusPoint;

        [Tooltip("Distance from the focus point (world coordinate)")]
        [SerializeField]
        private float distanceFromFocusPoint = 3f;

        private void Update()
        {
            Move();
        }

        /// <summary>
        /// Calculate the disired position/rotation and move this position
        /// </summary>
        private void Move()
        {
            Vector3 target2Focus = focusPoint.position - target.transform.position;

            //change the length
            Vector3 focus2position = target2Focus.normalized * distanceFromFocusPoint;

            //move to the position
            transform.position = focusPoint.position + focus2position;

            //look at the focus point
            transform.LookAt(focusPoint, Vector3.up);
        }

        /// <summary>
        /// Call this when the target is changed.
        /// </summary>
        public void ChangeTarget(BattleObject target)
        {
            this.target = target;
        }
    }
}
