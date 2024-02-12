using System;
using Logistics.PythonConnection;

namespace AI.BehaviourBranch.Logistics
{
    [Serializable]
    public class BranchData : DataClass
    {
        public NodeData[] nodes;
    }
}
