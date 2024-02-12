namespace Simulation.Controllers.Fighters
{
    public class FighterManualControllerFromCamera : FighterManualController
    {
        /// <summary>
        /// Moves the fighter based on to-target angle
        /// </summary>
        protected override void Move(float ratio, float angle)
        {
            fighterTarget.TurnAndRun(ratio, angle - 90f, fromTarget: true);
        }
    }
}
