using System;

namespace Simulation.Objects.Fighters
{
    /// <summary>
    /// Basic parameters for fighter's motion
    /// </summary>
    [Serializable]
    public struct MotionParametersFighter
    {
        public float runningSpeed;
        public float turningSpeed;

        public MotionParametersFighter(float runningSpeed, float turningSpeed)
        {
            this.runningSpeed = runningSpeed;
            this.turningSpeed = turningSpeed;
        }
    }
}
