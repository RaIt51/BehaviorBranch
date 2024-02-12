namespace Simulation.Controllers.Fighters
{
    /// <summary>
    /// Moves correspoinding to wolrd coordinate.
    /// </summary>
    public class FighterManualControllerFromWorld : FighterManualController
    {
        /// <summary>
        /// Moves the fighter based on world coordinate
        /// </summary>
        protected override void Move(float ratio, float angle)
        {
            fighterTarget.TurnAndRun(ratio, angle);
        }
    }
}
