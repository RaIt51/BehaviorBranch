using Simulation.Objects.Fighters;
using Simulation.Objects.Fighters.Actions;

namespace AI.BehaviourBranch.Behaviours
{
    public class LaunchingThunderbolt : BehaviourRunner
    {
        private bool launched = false;
        private bool launchedFormerFrame = false;

        public LaunchingThunderbolt(Fighter fighter, NodeAction action)
            : base(fighter, action)
        {
            satisfied = false;
        }

        protected override void Start() { }

        public override void Update()
        {
            //if Thunderbolt is not launched...
            if (!launched)
            {
                //...try to launch it
                launched = fighter.StartAction(typeof(Thunderbolt));
                launchedFormerFrame = launched;
            }

            //satisfied one frame after the launch
            if (launchedFormerFrame)
            {
                satisfied = true;
                launchedFormerFrame = false;
            }
        }
    }
}
