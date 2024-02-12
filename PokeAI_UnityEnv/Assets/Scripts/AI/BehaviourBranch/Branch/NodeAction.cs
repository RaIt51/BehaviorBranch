namespace AI.BehaviourBranch
{
    public class NodeAction : Node
    {
        public string action;

        public override string GetName()
        {
            return action;
        }
    }
}
