using System.Collections.Generic;
using Simulation.Objects.Fighters;

namespace Simulation
{
    /// <summary>
    /// Control the fighters meta data.
    /// </summary>
    public class FightersManager : SingletonMonoBehaviour<FightersManager>
    {
        /// <summary>
        /// Fighters currently in the battle.
        /// </summary>
        protected List<Fighter> fighters = new List<Fighter>();

        /// <summary>
        /// Register new fighter
        /// </summary>
        public virtual void RegisterFighter(Fighter fighterNew)
        {
            fighters.Add(fighterNew);
        }

        /// <summary>
        /// delete fighter
        /// </summary>
        public virtual bool DeleteFighter(Fighter fighter)
        {
            return fighters.Remove(fighter);
        }
    }
}
