using System;

namespace AI.BehaviourBranch.Controls
{
    public abstract class Control
    {
        protected NodeControl nodeThis;

        public Control(NodeControl nodeControl)
        {
            nodeThis = nodeControl;
        }

        public static Control CreateInstance(NodeControl nodeControl)
        {
            switch (nodeControl.controlName)
            {
                case Then.name:
                    return new Then(nodeControl);
                case "Repeat":
                    return new Repeat(nodeControl);
                case QuitRepeating.name:
                    return new QuitRepeating(nodeControl);
                case "StartStopwatch":
                    return new StartStopwatch(nodeControl);
                default:
                    throw new Exception("Control not found: " + nodeControl.controlName);
            }
        }

        public abstract void ExecuteFirst(BehaviourBranchAI behaviourBranchAI);
        public abstract void ExecuteUpdate(BehaviourBranchAI behaviourBranchAI);
    }
}
