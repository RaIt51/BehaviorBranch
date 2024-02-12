using System;
using Logistics.PythonConnection;

namespace AI.BehaviourBranch.Logistics
{
    [Serializable]
    public class NodeData : DataClass
    {
        public int id;
        public string type;
        public string action;
        public string condition;
        public string control;
        public string[] args;
        public int node_next = -1;
        public int node_true = -1;
    }
}
