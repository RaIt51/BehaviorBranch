using System;
using UnityEngine;
using AI.BehaviourBranch.Variables;
using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch.Conditions
{
    /// <summary>
    /// Base class for all conditions.
    ///
    /// Check if the Fighter should change behaviour
    /// </summary>
    public class Condition
    {
        protected const float EQUAL_THRESHOLD = 0.5f;

        protected Fighter fighter;
        protected FighterWatcher fighterWatcher;
        protected NodeCondition nodeCondition;

        public Condition(Fighter fighter, NodeCondition nodeCondition)
        {
            this.fighter = fighter;
            this.fighterWatcher = fighter.GetComponent<FighterWatcher>();
            this.nodeCondition = nodeCondition;
        }

        public virtual bool Check()
        {
            switch (nodeCondition.conditionName)
            {
                case "<":
                    return Variable.ConvertVariable(nodeCondition.args[0], fighterWatcher)
                        < Variable.ConvertVariable(nodeCondition.args[1], fighterWatcher);
                case ">":
                    return Variable.ConvertVariable(nodeCondition.args[0], fighterWatcher)
                        > Variable.ConvertVariable(nodeCondition.args[1], fighterWatcher);
                case "==":
                    //approximate equality
                    float difference =
                        Variable.ConvertVariable(nodeCondition.args[0], fighterWatcher)
                        - Variable.ConvertVariable(nodeCondition.args[1], fighterWatcher);
                    return Math.Abs(difference) < EQUAL_THRESHOLD;
                default:
                    Debug.Log("Unknown condition: " + nodeCondition.conditionName);
                    return true;
            }
        }
    }
}
