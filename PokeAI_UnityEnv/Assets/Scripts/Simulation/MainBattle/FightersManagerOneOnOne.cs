using Simulation.Objects.Fighters;

namespace Simulation.OneOnOne
{
    /// <summary>
    /// Manages the fighters in OneOnOne scene.
    /// </summary>
    public class FightersManagerOneOnOne : FightersManager
    {
        /// <summary>
        /// If 2 fighters are registered, give them targets.
        /// </summary>
        private void GiveTargets()
        {
            //each fighter has the other as target
            fighters[0].SetTarget(fighters[1]);
            fighters[1].SetTarget(fighters[0]);
        }

        /// <summary>
        /// Call this when a fighter is appeared
        /// </summary>
        public override void RegisterFighter(Fighter fighterNew)
        {
            base.RegisterFighter(fighterNew);

            if (fighters.Count == 2)
            {
                //fighters all full

                GiveTargets();
            }
        }
    }
}
