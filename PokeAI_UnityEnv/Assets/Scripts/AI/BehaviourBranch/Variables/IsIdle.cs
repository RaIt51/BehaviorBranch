using UnityEngine;
using Simulation;
using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch.Variables
{
    public class IsIdle : Variable
    {
        public IsIdle(FighterWatcher fighterWatcher)
            : base(fighterWatcher) { }

        public override float Get()
        {
            return EvaluateBool(fighterWatcher.state == FighterStateController.stateNameIdle);
        }
    }
}
