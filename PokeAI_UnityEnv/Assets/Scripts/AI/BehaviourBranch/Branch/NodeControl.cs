using AI.BehaviourBranch.Controls;

namespace AI.BehaviourBranch
{
    public class NodeControl : Node
    {
        public string controlName;
        public Control control;

        /// <summary>
        /// If true, this won't be activated until BehaviorRunner.satisfied == true
        /// </summary>
        public bool flagAfterSatisfaction = false;

        public NodeControl(string controlName)
        {
            this.controlName = controlName;
            control = Control.CreateInstance(this);
        }

        public override string GetName()
        {
            return controlName;
        }
    }
}
