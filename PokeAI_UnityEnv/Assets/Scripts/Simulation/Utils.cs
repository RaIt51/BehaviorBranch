using UnityEngine;

namespace Simulation
{
    /// <summary>
    /// Helper functions for simulation namespace
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Limit angle to (-180, 180]
        /// </summary>
        public static float LimitAngle(float angle)
        {
            float result = angle;
            while (result > 180)
            {
                result -= 360;
            }
            while (result <= -180)
            {
                result += 360;
            }

            return result;
        }

        /// <summary>
        /// Get horizontal angle (on zx plane, x+axis:0, z+axis:90) of a vector
        /// </summary>
        public static float VectorToHorizontalAngle(Vector3 vector)
        {
            return LimitAngle(Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg);
        }
    }
}
