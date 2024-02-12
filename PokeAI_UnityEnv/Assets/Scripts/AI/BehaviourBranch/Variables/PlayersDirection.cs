using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch.Variables
{
    public class PlayersDirection : Variable
    {
        public PlayersDirection(FighterWatcher fighterWatcher)
            : base(fighterWatcher) { }

        public override float Get()
        {
            return 180f;
        }
    }
}
