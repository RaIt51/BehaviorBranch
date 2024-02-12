using UnityEngine.Events;

namespace Simulation.Objects.Fighters
{
    public class State
    {
        /// <summary>
        /// Level that this fighter is allowed to perform basical move.
        /// </summary>
        public enum MovableLevel
        {
            free,
            limited,
            stop
        }

        /// <summary>
        /// what basical movement is allowed
        /// </summary>
        public struct MovementAllowance
        {
            /// <summary>
            /// Initializes what action is allowed
            /// </summary>
            public MovementAllowance(bool running, bool jumping, bool turning, bool action)
            {
                this.running = running;
                this.jumping = jumping;
                this.turning = turning;
                this.action = action;
            }

            public bool running;
            public bool jumping;
            public bool turning;
            public bool action;

            /// <summary>
            /// Returns if all allowance is true.
            /// </summary>
            public bool allAllowed
            {
                get { return running && jumping && turning && action; }
            }

            /// <summary>
            /// Returns if all allowance is false.
            /// </summary>
            public bool allProhibited
            {
                get { return !running && !jumping && !turning && !action; }
            }
        }

        /// <summary>
        /// name of the state
        /// </summary>
        public string name;

        /// <summary>
        /// name of the sub state.
        ///
        /// As like "running" during "idle".
        /// This doesn't actually indicates any restriction.
        /// </summary>
        public string subState = "none";

        /// <summary>
        /// Priority level.
        /// If upcomming priority level is >= this, this state will be interruped.
        ///
        /// (rule)
        /// Idle: 1
        /// Highest priority (by game sequence demand): 100
        /// Stagger interruption: 70
        /// </summary>
        public int priority = 1;

        /// <summary>
        /// If this state was made by interruption, Revert to previous state when this state finished.
        /// If false, switch to idle.
        /// </summary>
        public bool previousContinuity = false;

        /// <summary>
        /// Returns movable level of this state.
        /// </summary>
        public MovableLevel movable
        {
            get
            {
                if (movementAllowance.allAllowed)
                {
                    return MovableLevel.free;
                }
                else if (movementAllowance.allProhibited)
                {
                    return MovableLevel.stop;
                }
                else
                {
                    return MovableLevel.limited;
                }
            }
        }

        /// <summary>
        /// Contains specific movement allowance for this state.
        /// </summary>
        public   MovementAllowance movementAllowance;

        /// <summary>
        /// Called when this state interrupted by higher priority state.
        /// </summary>
        public UnityEvent<bool> onInterrupted = new UnityEvent<bool>();

        /// <summary>
        /// Corresponding action index registered to this fighter.
        /// If not related to action, -1.
        /// </summary>
        public int actionNumber = -1;

        /// <summary>
        /// When this state starts, the fighter wear the default motion parameters
        /// </summary>
        public bool useDefaultMotionParameters = true;

        /// <summary>
        /// Motion parameters used while this state
        /// 
        /// If useDefaultMotionParameters is true, this will be ignored.    
        /// </summary>
        public MotionParametersFighter motionParameters { get; private set; }

        public State(
            string name,
            MovementAllowance movementAllowance,
            int priority,
            bool previousContinuity,
            bool useDefaultMotionParameters = true
        )
        {
            this.name = name;
            this.movementAllowance = movementAllowance;
            this.priority = priority;
            this.previousContinuity = previousContinuity;
            this.useDefaultMotionParameters = useDefaultMotionParameters;
        }

        /// <summary>
        /// Will be called when this state is interrupted by higher priority state.
        /// </summary>
        public void RegisterOnInterrupted(UnityAction<bool> action)
        {
            onInterrupted.AddListener(action);
        }

        /// <summary>
        /// Unregister onInterrupted action
        /// </summary>
        public void UnregisterOnInterrupted(UnityAction<bool> action)
        {
            onInterrupted.RemoveListener(action);
        }

        /// <summary>
        /// Set motion parameters used while this state
        /// 
        /// If useDefaultMotionParameters is true, this will be ignored.
        /// </summary>
        public void SetMotionParameters(MotionParametersFighter motionParameters)
        {
            this.motionParameters = motionParameters;
        }
    }
}
