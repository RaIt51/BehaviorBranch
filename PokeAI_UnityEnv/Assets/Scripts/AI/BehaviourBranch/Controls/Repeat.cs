using AI.BehaviourBranch.Variables;

namespace AI.BehaviourBranch.Controls
{
    public class Repeat : Control
    {
        public Repeat(NodeControl nodeControl)
            : base(nodeControl)
        {
            nodeControl.flagAfterSatisfaction = true;
        }

        public static bool JudgeEndRepeatition(Node newBranchRoot)
        {
            Node nodeLooking = newBranchRoot;
            while (nodeLooking.nodeNext != null)
            {
                if (nodeLooking is NodeAction)
                {
                    return true;
                }

                nodeLooking = nodeLooking.nodeNext;
            }

            return false;
        }

        public override void ExecuteFirst(BehaviourBranchAI behaviourBranchAI)
        {
            int repitition = (int)
                Variable.ConvertVariable(nodeThis.args[0], behaviourBranchAI.fighterWatcher);

            behaviourBranchAI.StartRepeating(nodeThis.nodeNext, repitition);
        }

        public override void ExecuteUpdate(BehaviourBranchAI behaviourBranchAI) { }
    }
}
