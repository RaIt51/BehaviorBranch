using System;
using System.Collections.Generic;
using Logistics.PythonConnection;

namespace AI.BehaviourBranch.Logistics
{
    /// <summary>
    /// Connect with Python and receive new BehaviourBranch
    /// </summary>
    public class BranchDecoder : DataDecoder
    {
        protected override Dictionary<string, Type> DataToType()
        {
            return new Dictionary<string, Type>() { { "branch", typeof(BranchData) }, };
        }
    }
}
