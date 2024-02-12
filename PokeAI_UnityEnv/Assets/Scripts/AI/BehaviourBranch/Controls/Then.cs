namespace AI.BehaviourBranch.Controls
{
    public class Then : Control
    {
        public const string name = "Then";

        public Then(NodeControl nodeControl)
            : base(nodeControl) { }

        /// <summary>
        /// Connect "then" node to the current node
        /// </summary>
        public static void ConnectThen(Node nodeCurrent, NodeControl nodeThen)
        {
            Node nodeLast = nodeCurrent.GetBottom();
            //if the last node is condition...
            if (nodeLast is NodeCondition)
            {
                //...connect to the condition's "true" route's last
                NodeCondition nodeCondition = nodeLast as NodeCondition;
                if (nodeCondition.nodeTrue != null)
                {
                    //null guard
                    nodeCondition.ConnectTrue(nodeThen);
                }
                else
                {
                    nodeCondition.GetTrueBottom().ConnectNext(nodeThen);
                }
            }
            //if action or control...
            else
            {
                //...simply connect to the last node
                nodeLast.ConnectNext(nodeThen);
            }
        }

        public override void ExecuteFirst(BehaviourBranchAI behaviourBranchAI)
        {
            //do nothing
        }

        public override void ExecuteUpdate(BehaviourBranchAI behaviourBranchAI)
        {
            //do nothing
        }
    }
}
