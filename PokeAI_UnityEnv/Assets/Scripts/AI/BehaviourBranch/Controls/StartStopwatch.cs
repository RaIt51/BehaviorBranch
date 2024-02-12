namespace AI.BehaviourBranch.Controls
{
    public class StartStopwatch : Control
    {
        public StartStopwatch(NodeControl nodeControl)
            : base(nodeControl) { }

        public override void ExecuteFirst(BehaviourBranchAI behaviourBranchAI)
        {
            //stop repeating
            behaviourBranchAI.StartTime();
        }

        public override void ExecuteUpdate(BehaviourBranchAI behaviourBranchAI) { }
    }
}
