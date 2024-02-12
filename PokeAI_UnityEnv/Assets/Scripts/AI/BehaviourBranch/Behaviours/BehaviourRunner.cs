using System;
using UnityEngine;
using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch.Behaviours
{
    /// <summary>
    /// Base class for all behaviours.
    ///
    /// Show what the Fighter should do in this behaviour state
    /// The Fighter will be controlled by this as long as the behaviour is active
    /// </summary>
    public abstract class BehaviourRunner
    {
        public bool satisfied { get; protected set; } = true;

        protected Fighter fighter;
        protected NodeAction nodeAction;

        public BehaviourRunner(Fighter fighter, NodeAction nodeAction)
        {
            this.fighter = fighter;
            this.nodeAction = nodeAction;

            Start();
        }

        protected abstract void Start();
        public abstract void Update();

        public static BehaviourRunner CreateInstance(Fighter fighter, NodeAction nodeAction)
        {
            switch (nodeAction.action)
            {
                case "RunTowardsTarget":
                    return new RunningTowardsTarget(fighter, nodeAction);
                case "Stop":
                    return new Stopping(fighter, nodeAction);
                case "Irontail":
                    return new LaunchingIronTail(fighter, nodeAction);
                case "Tackle":
                    return new LaunchingTackle(fighter, nodeAction);
                case "Thunderbolt":
                    return new LaunchingThunderbolt(fighter, nodeAction);
                default:
                    Debug.Log("Unknown action: " + nodeAction.action);
                    return new Stopping(fighter, nodeAction);
            }
        }
    }
}
