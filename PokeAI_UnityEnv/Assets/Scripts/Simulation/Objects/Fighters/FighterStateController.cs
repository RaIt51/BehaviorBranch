using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Simulation.Objects.Fighters
{
    /// <summary>
    /// Fighter's state controller. Also contains motion parameters that should be used.
    /// </summary>
    public class FighterStateController
    {
        //state names
        public const string stateNameIdle = "idle";
        public const string stateNameStagger = "stagger";

        /// <summary>
        /// Current state of this fighter.
        /// </summary>
        public State stateCurrent { get; protected set; }

        /// <summary>
        /// Previous state of this fighter. Stacked more as interrupted
        /// </summary>
        public Stack<State> statesPrevious { get; protected set; } = new Stack<State>();

        /// <summary>
        /// States that this fighter can potentially transit to.
        /// </summary>
        private List<State> statesPotential = new List<State>();

        //priority levels
        /// <summary>
        /// Priority level for idle state.
        /// </summary>
        public int priorityIdle { get; protected set; } = 1;

        /// <summary>
        /// Priority level for stagger state.
        /// </summary>
        public int priorityStagger { get; protected set; } = 70;

        /// <summary>
        /// Highest priority level.
        /// </summary>
        public int priorityMax { get; protected set; } = 100;

        /// <summary>
        /// Priority level for invoking action.
        /// </summary>
        public int priorityAction { get; protected set; } = 20;

        /// <summary>
        /// Default motion parameters to use when state is changed to the state,
        /// which has useDefaultMotionParameters == true.
        /// </summary>
        private MotionParametersFighter motionParametersDefault;

        /// <summary>
        /// Basic state. This state indicates not doing any other states.
        /// </summary>
        private State stateIdle = new State(
            stateNameIdle,
            new State.MovementAllowance(true, true, true, true),
            1,
            false
        );

        private State stateStagger = new State(
            stateNameStagger,
            new State.MovementAllowance(false, false, false, false),
            70,
            true
        );

        /// <summary>
        /// Current motion parameters.
        /// </summary>
        public MotionParametersFighter motionParametersCurrent
        {
            get
            {
                if (stateCurrent.useDefaultMotionParameters)
                {
                    //default
                    return motionParametersDefault;
                }
                else
                {
                    //specified by state
                    return stateCurrent.motionParameters;
                }
            }
        }

        /// <summary>
        /// Initializer
        /// </summary>
        public FighterStateController(MotionParametersFighter defaultMotionParameters)
        {
            //add basical states
            RegisterState(stateIdle);
            RegisterState(stateStagger);

            //initialize state
            stateCurrent = stateIdle;

            //initialize motion parameters
            RegisterDefaultMotionParameters(defaultMotionParameters);
        }

        /// <summary>
        /// Set default motion parameters.
        ///
        /// This parameters will be used when state is changed to the state,
        /// which has useDefaultMotionParameters == true.
        /// </summary>
        public void RegisterDefaultMotionParameters(MotionParametersFighter motionParameters)
        {
            motionParametersDefault = motionParameters;
        }

        /// <summary>
        /// Request to this fighter what state wanted to start
        /// </summary>
        /// <param name="stateName">Name of the state want to start.
        /// It should be registered by RegisterState() in advance.</param>
        /// <returns>If state changes succeeded.</returns>
        public bool RequestState(string stateName)
        {
            //find state
            State stateRequested = statesPotential.Find(state => state.name == stateName);

            //if not found
            if (stateRequested == null)
            {
                //can't change
                Debug.LogError("No state found: " + stateName);
                return false;
            }

            //found

            //change state
            return ChangeState(stateRequested, true);
        }

        /// <summary>
        /// Request the end of the state
        /// </summary>
        /// <param name="stateName">State you want to end</param>
        /// <returns>True if succeeded. False indicates the given name is not the current state</returns>
        public bool EndState(string stateName)
        {
            //check if current state is the state want to end
            if (stateCurrent.name != stateName)
            {
                //erace only the last stack corresponding to the state
                List<State> list = statesPrevious.ToList();
                int lastIndex = list.FindLastIndex(state => state.name == stateName);
                if (lastIndex == -1)
                {
                    //no state found
                    return false;
                }
                else
                {
                    //found
                    //-> erace
                    list.RemoveAt(lastIndex);

                    //return the stack
                    statesPrevious = new Stack<State>(list);

                    return true;
                }
            }
            else
            {
                //current is the state

                if (stateCurrent.previousContinuity)
                {
                    //if previous state exists
                    if (statesPrevious.Count > 0)
                    {
                        //change to previous state
                        stateCurrent = statesPrevious.Pop();
                    }
                    else
                    {
                        //no previous state
                        //-> change to idle
                        stateCurrent = stateIdle;
                    }
                }
                else
                {
                    //should not continue
                    stateCurrent = stateIdle;

                    Debug.Log("state: " + stateCurrent.name + " started");

                    //forget all stacks
                    statesPrevious.Clear();
                }

                //succeeded
                return true;
            }
        }

        /// <summary>
        /// Register state that this fighter can transit to.
        /// </summary>
        /// <returns>True if successfully registered. False indicates name duplication</returns>
        public bool RegisterState(State state)
        {
            if (CheckStateNameDuplication(state.name))
            {
                return false;
            }

            //add
            statesPotential.Add(state);

            return true;
        }

        /// <summary>
        /// Remove the state from potential states.
        /// </summary>
        /// <param name="stateName">name of the State instance want to remove</param>
        /// <returns>True if succeeded. False indicates there wasn't corresponding state</returns>
        public bool RemoveState(string stateName)
        {
            //remove all
            return statesPotential.RemoveAll(statePotential => statePotential.name == stateName)
                > 0;
        }

        /// <summary>
        /// Change state if able.
        /// </summary>
        /// <returns>True if succeeded.
        /// False indicates higher priority state running.</returns>
        private bool ChangeState(
            State stateRequested,
            bool interruption,
            bool systemInterruption = false
        )
        {
            //if higher priority state running
            if (stateCurrent.priority > stateRequested.priority)
            {
                //can't change
                return false;
            }

            //able to change

            if (!interruption)
            {
                //if not interruption, forget all stacks
                statesPrevious.Clear();
            }
            else
            {
                //if interruption, stack current state
                statesPrevious.Push(stateCurrent);

                //notice interruption
                stateCurrent.onInterrupted.Invoke(systemInterruption);
            }

            //change state
            stateCurrent = stateRequested;

            Debug.Log("state: " + stateCurrent.name + " started");

            //succeeded
            return true;
        }

        /// <summary>
        /// Check the state name want to add is not duplicated.
        /// Use for gurantee state name is unique.
        /// </summary>
        private bool CheckStateNameDuplication(string nameRequested)
        {
            return statesPotential.Any(state => state.name == nameRequested);
        }
    }
}
