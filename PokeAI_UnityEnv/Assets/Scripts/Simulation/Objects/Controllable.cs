namespace Simulation.Objects
{
    /// <summary>
    /// Attach this to the object that can be controlled by the client
    /// </summary>
    public interface Controllable
    {
        /// <summary>
        /// Client starts to control this object
        /// </summary>
        public void RideOn();
    }
}
