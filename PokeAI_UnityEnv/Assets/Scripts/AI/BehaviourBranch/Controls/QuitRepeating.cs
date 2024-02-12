using AI.BehaviourBranch.Variables;

namespace AI.BehaviourBranch.Controls
{
    public class QuitRepeating : Control
    {
        public const string name = "QuitRepeating";

        public QuitRepeating(NodeControl nodeControl)
            : base(nodeControl) { }

        public static NodeControl CreateInstance()
        {
            return new NodeControl(name);
        }

        public override void ExecuteFirst(BehaviourBranchAI behaviourBranchAI)
        {
            //stop repeating
            behaviourBranchAI.StopRepeating();
        }

        public override void ExecuteUpdate(BehaviourBranchAI behaviourBranchAI) { }
    }
}
