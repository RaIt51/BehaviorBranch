using AI.BehaviourBranch.Conditions;
using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch
{
    public class NodeCondition : Node
    {
        public string conditionName;
        public Node nodeTrue;
        public Condition condition;

        public override string GetName()
        {
            return conditionName;
        }

        public void ConnectTrue(Node node)
        {
            nodeTrue = node;
            node.nodePrevious = this;
        }

        public Node GetTrueBottom()
        {
            if (nodeTrue == null)
            {
                return null;
            }

            Node node = nodeTrue;
            while (node.nodeNext != null)
            {
                node = node.nodeNext;
            }
            return node;
        }

        public bool Check(Fighter fighter)
        {
            //if this is the first time...
            if (condition == null)
            {
                //...create new condition instance
                condition = new Condition(fighter, this);
            }

            //check condition
            return condition.Check();
        }
    }
}
