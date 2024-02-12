using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AI.BehaviourBranch.Logistics;
using AI.BehaviourBranch.Behaviours;
using AI.BehaviourBranch.Controls;
using Logistics.PythonConnection;

namespace AI.BehaviourBranch
{
    /// <summary>
    /// Controlling the fighter using Behaviour Branch
    /// </summary>
    [RequireComponent(typeof(BranchDecoder))]
    [RequireComponent(typeof(BranchConnector))]
    public class BehaviourBranchAI : FighterControllerAI
    {
        //structure
        protected Node nodeCurrent;
        protected NodeAction nodeActionActive;

        //action
        protected BehaviourRunner behaviourRunnerActive;
        public bool actionSatisfied
        {
            get
            {
                //null handling
                if (behaviourRunnerActive == null)
                {
                    return true;
                }
                else
                {
                    return behaviourRunnerActive.satisfied;
                }
            }
        }

        //repitition
        protected bool flagRepeating = false;
        protected Node repeatingStart;
        protected int repititionLeft = 0;

        //node stopwatch
        public float stopwatch { get; protected set; } = 0f;

        private void Start()
        {
            PrepareConnection();
        }

        private void Update()
        {
            ControlBehaviour();
            RunBehaviour();
            UpdateStopwatch();
        }

        public void StartRepeating(Node starterAction, int repitition)
        {
            flagRepeating = true;
            repititionLeft = repitition;
            repeatingStart = starterAction;

            //for the first time
            CountRepeating();

            Debug.Log("Repetition Started");
        }

        public void StopRepeating()
        {
            flagRepeating = false;

            Debug.Log("Repetition Finished");
        }

        /// <summary>
        /// Get the active conditions
        /// </summary>
        /// <param name="nodeCurrent">node checking</param>
        /// <returns>active conditions</returns>
        public static NodeCondition[] GetActiveConditions(Node nodeCurrent)
        {
            return GetActiveNodes(nodeCurrent)
                .Where(node => node is NodeCondition)
                .Select(node => (NodeCondition)node)
                .ToArray();
        }

        public static NodeControl[] GetActiveControls(Node nodeCurrent)
        {
            return GetActiveNodes(nodeCurrent)
                .Where(node => node is NodeControl)
                .Select(node => (NodeControl)node)
                .ToArray();
        }

        //nodes control
        public static Node[] GetActiveNodes(Node nodeCurrent)
        {
            List<Node> nodesActive = new List<Node>();

            //look backwards for the first condition
            Node nodeLooking = nodeCurrent;
            while (nodeLooking != null)
            {
                nodesActive.Add(nodeLooking);

                // if the node is was an action node...
                if (nodeLooking is NodeAction)
                {
                    //...stop searching
                    break;
                }

                // to next
                nodeLooking = nodeLooking.nodePrevious;
            }

            return nodesActive.ToArray();
        }

        public void StartTime()
        {
            stopwatch = 0f;
        }

        private void ControlBehaviour()
        {
            if (nodeCurrent == null)
            {
                return;
            }

            TransitCurrentNode();

            //check conditions
            //only when tranferable
            if (actionSatisfied)
            {
                NodeCondition nodeConditionMet = CheckConditions(nodeCurrent);

                //if there is a met condition...
                if (nodeConditionMet != null)
                {
                    //...to its true node
                    ChangeCurrentNode(nodeConditionMet.nodeTrue);

                    //update current node
                    TransitCurrentNode();
                }
            }

            //update controls
            UpdateControls(nodeCurrent);

            //if repeating AND transmittable...
            if (flagRepeating && actionSatisfied)
            {
                /*
                This won't interrupt the linear flow because the linear flow processed before this
                */

                //...repeat
                ChangeCurrentNode(repeatingStart);

                CountRepeating();

                //control the current node
                TransitCurrentNode();
            }
        }

        private void TransitCurrentNode()
        {
            while (nodeCurrent.nodeNext != null)
            {
                Node nodeNext = nodeCurrent.nodeNext;

                //if next is an action node...
                if (nodeNext is NodeAction)
                {
                    //...if able to transfer...
                    if (actionSatisfied)
                    {
                        //...transfer
                        ChangeCurrentNode(nodeNext);
                    }
                    //...if not able...
                    else
                    {
                        //...stop transit
                        return;
                    }
                }
                //if next is a control node...
                else if (nodeNext is NodeControl)
                {
                    NodeControl nodeControl = nodeNext as NodeControl;

                    //if this was a "then" node AND in repeating...
                    if ((nodeControl.controlName == Then.name) && flagRepeating)
                    {
                        //...stop transit
                        //wait until the repitition finishes
                        return;
                    }

                    //...if not able to transfer...
                    if (nodeControl.flagAfterSatisfaction && !actionSatisfied)
                    {
                        //...stop transit
                        return;
                    }
                    // ... if able...
                    else
                    {
                        //...transfer
                        ChangeCurrentNode(nodeNext);
                    }
                }
                //if next is a condition node...
                else if (nodeNext is NodeCondition)
                {
                    NodeCondition nodeCondition = nodeNext as NodeCondition;

                    //...transfer without any limitation
                    ChangeCurrentNode(nodeCondition);

                    //...if the condition is met AND transmittable...
                    if (nodeCondition.Check(fighterTarget) && actionSatisfied)
                    {
                        //...to the true node
                        if (nodeCondition.nodeTrue != null) //null guard
                        {
                            ChangeCurrentNode(nodeCondition.nodeTrue);
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException("Node type not implemented");
                }
            }
        }

        private void ChangeCurrentNode(Node nodeNext)
        {
            //simple variable update
            nodeCurrent = nodeNext;

            Debug.Log("Node changed to " + nodeCurrent.GetName());

            //process with the change
            if (nodeCurrent is NodeControl)
            {
                NodeControl nodeControl = nodeCurrent as NodeControl;

                //first execution
                nodeControl.control.ExecuteFirst(this);
            }
            else if (nodeCurrent is NodeAction)
            {
                NodeAction nodeAction = nodeCurrent as NodeAction;

                SetNewActionBehaviour(nodeAction);
            }
        }

        private BehaviourRunner SetNewActionBehaviour(NodeAction nodeAction)
        {
            //...create a new one
            behaviourRunnerActive = BehaviourRunner.CreateInstance(fighterTarget, nodeAction);
            nodeActionActive = nodeAction;
            return behaviourRunnerActive;
        }

        private NodeCondition CheckConditions(Node currentNode)
        {
            //for all active conditions...
            foreach (NodeCondition nodeCondition in GetActiveConditions(currentNode))
            {
                //if the condition is met...
                if (nodeCondition.Check(fighterTarget))
                {
                    //skip if no true node
                    if (nodeCondition.nodeTrue == null)
                    {
                        continue;
                    }

                    //...return the condition
                    return nodeCondition;
                }
            }

            //none condition met
            return null;
        }

        private void UpdateControls(Node currentNode)
        {
            foreach (NodeControl control in GetActiveControls(currentNode))
            {
                control.control.ExecuteUpdate(this);
            }
        }

        private void ConnectNext(Node rootNewBranch)
        {
            //if `node` is the first node...
            if (nodeCurrent == null)
            {
                //...set as the current node
                ChangeCurrentNode(rootNewBranch);

                //early return
                return;
            }

            //not the first node

            //if root is "Then"...
            if (
                (rootNewBranch is NodeControl)
                && ((rootNewBranch as NodeControl).controlName == Then.name)
            )
            {
                Then.ConnectThen(nodeCurrent, rootNewBranch as NodeControl);
            }
            //if root is not "Then"...
            else
            {
                //if repeating + condition...
                if (flagRepeating && (rootNewBranch is NodeCondition))
                {
                    NodeCondition rootCondition = rootNewBranch as NodeCondition;

                    //...this is a repeating ending condition
                    nodeCurrent.GetBottom().ConnectNext(rootCondition);

                    //make this as end condition
                    //sandwich "QuitRepeating" between the condition and the true node
                    NodeControl controlQuitRepeating = QuitRepeating.CreateInstance();
                    Node nodeTrue = rootCondition.nodeTrue;
                    rootCondition.ConnectTrue(controlQuitRepeating);
                    controlQuitRepeating.ConnectNext(nodeTrue);
                }
                else
                {
                    //stop repeating if it is
                    //this should be first
                    //  because it could rewrite Repeat node
                    if (flagRepeating)
                    {
                        StopRepeating();
                    }

                    //...act the new branch root immediately
                    nodeCurrent.GetBottom().ConnectNext(rootNewBranch);
                    ChangeCurrentNode(rootNewBranch);
                }
            }
        }

        private void CountRepeating()
        {
            repititionLeft--;
            if (repititionLeft == 0)
            {
                StopRepeating();
            }
        }

        //action control
        private void RunBehaviour()
        {
            if (behaviourRunnerActive == null)
            {
                return;
            }

            behaviourRunnerActive.Update();
        }

        //stop watch
        private void UpdateStopwatch()
        {
            stopwatch += Time.deltaTime;
        }

        // python connection
        private void PrepareConnection()
        {
            BranchDecoder decoder = GetComponent<BranchDecoder>();
            decoder.RegisterAction(typeof(BranchData), ReceiveNewBranch);
        }

        private void ReceiveNewBranch(DataClass data)
        {
            BranchData branchData = (BranchData)data;

            //null guard
            if (branchData == null)
            {
                return;
            }

            Node branchRoot = Node.ReadBranchData(branchData);

            //connect
            ConnectNext(branchRoot);
        }
    }
}
