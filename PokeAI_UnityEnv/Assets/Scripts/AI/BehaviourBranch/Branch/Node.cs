using System;
using UnityEngine;
using AI.BehaviourBranch.Logistics;

namespace AI.BehaviourBranch
{
    public abstract class Node
    {
        public string type;
        public string[] args;
        public Node nodeNext;
        public Node nodePrevious;

        public abstract string GetName();

        public void ConnectNext(Node node)
        {
            node.nodePrevious = this;
            nodeNext = node;
        }

        public Node GetBottom()
        {
            Node node = this;
            while (node.nodeNext != null)
            {
                node = node.nodeNext;
            }
            return node;
        }

        public static Node ReadBranchData(BranchData data)
        {
            //to Node instance
            Node[] nodes = new Node[data.nodes.Length];
            for (int cnt = 0; cnt < data.nodes.Length; cnt++)
            {
                nodes[cnt] = ReadNodeData(data.nodes[cnt]);
            }

            //connecting next
            for (int cnt = 0; cnt < data.nodes.Length; cnt++)
            {
                if (data.nodes[cnt].node_next != -1)
                {
                    nodes[cnt].ConnectNext(nodes[data.nodes[cnt].node_next]);
                }
            }

            //connecting true
            for (int cnt = 0; cnt < data.nodes.Length; cnt++)
            {
                if (data.nodes[cnt].node_true != -1)
                {
                    Debug.Assert(data.nodes[cnt].type == "condition");

                    ((NodeCondition)nodes[cnt]).ConnectTrue(nodes[data.nodes[cnt].node_true]);
                }
            }

            return nodes[0];
        }

        private static Node ReadNodeData(NodeData data)
        {
            Node node = null;
            if (data.type == "action")
            {
                node = new NodeAction();
                ((NodeAction)node).action = data.action;
            }
            else if (data.type == "condition")
            {
                node = new NodeCondition();
                ((NodeCondition)node).conditionName = data.condition;
            }
            else if (data.type == "control")
            {
                node = new NodeControl(data.control);
            }
            else
            {
                throw new Exception("Unknown node type: " + data.type);
            }

            node.type = data.type;
            node.args = data.args;
            return node;
        }
    }
}
