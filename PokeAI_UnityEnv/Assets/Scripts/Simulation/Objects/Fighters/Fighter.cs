using System;
using UnityEngine;
using Simulation.Objects.Fighters.Actions;
using System.Collections.Generic;

namespace Simulation.Objects.Fighters
{
    /// <summary>
    /// Base class of fighting character object in the simulated world.
    /// </summary>
    public abstract class Fighter : BattleObject, Controllable, AttackingObject
    {
        public enum StaggerLevel
        {
            light,
            heavy,
            blown
        }

        /// <summary>
        /// Target fighter looking focusing on
        /// </summary>
        public BattleObject target
        {
            get { return _target; }
            protected set
            {
                cameraFighter.ChangeTarget(value);
                _target = value;
            }
        }

        private BattleObject _target;

        /// <summary>
        /// How many actions allocations able
        /// </summary>
        public const int ACTION_MAX = 3;

        public Vector3 attackingDirection
        {
            //simply forward
            get { return transform.forward; }
        }

        /// <summary>
        /// Memorize if action was invoked in the previous frame.
        /// </summary>
        private bool[] actionsPreviouslyInvoked = new bool[ACTION_MAX];

        /// <summary>
        /// Actions invoking in the current frame.
        /// </summary>
        private bool[] actionsCurrentlyInvoking = new bool[ACTION_MAX];

        /// <summary>
        /// Health
        /// </summary>
        public int hp { get; protected set; } = 100000000;

        /// <summary>
        /// Controls ActionCollider of this fighter
        /// </summary>
        public FighterActionColliderController actionColliderController { get; protected set; }

        /// <summary>
        /// State controller for this fighter.
        ///
        /// will be initialized in Awake()
        /// </summary>
        public FighterStateController stateController { get; protected set; }

        /// <summary>
        /// Animation controller for this fighter.
        /// </summary>
        public FighterAnimationController animationController { get; protected set; }

        /// <summary>
        /// MotionParameters currently using
        /// </summary>
        public MotionParametersFighter motionParametersCurrent
        {
            get { return stateController.motionParametersCurrent; }
        }

        [Header("Motion Parameters")]
        [Tooltip("Default motion parameters. Used when no specific parameters are given")]
        [SerializeField]
        private MotionParametersFighter motionParametersDefaultInitial;

        protected State stateCurrent
        {
            get { return stateController.stateCurrent; }
        }

        [Tooltip("From timer start to end; in seconds")]
        [SerializeField]
        private float staggerDurationLight = 0.7f;

        [Tooltip("From timer start to end; in seconds")]
        [SerializeField]
        private float staggerDurationHeavy = 1.7f;

        [Tooltip("From timer start to end; in seconds")]
        [SerializeField]
        private float staggerDurationBlown = 2.7f;

        [Tooltip("Blowing horizontal component of stagger force (normal direction from attack)")]
        [SerializeField]
        protected float staggerForceBlownHorizontal = 5f;

        [Tooltip("Blowing vertical component of stagger force (upwards)")]
        [SerializeField]
        protected float staggerForceBlownVertical = 5f;

        /// <summary>
        /// Controls stagger duration
        /// </summary>
        protected Timer timerStagger;

        [Header("Battle parameters")]
        [Tooltip("initial HP")]
        [SerializeField]
        protected int hpInitial = 100;

        [Header("Display")]
        [Tooltip("Nickname of this fighter to display")]
        [SerializeField]
        public string nickname = "unnamed";

        [Tooltip("Spiecies name of this fighter")]
        [SerializeField]
        private StringTranslated _nameFighter;

        /// <summary>
        /// Spiecies name of this fighter
        /// </summary>
        public string nameFighter
        {
            get { return _nameFighter.Get(Languages.languageDisplaying); }
        }

        [Header("Actions")]
        [Tooltip(
            "Actions allocated to this fighter. The index number corresponds to the key config"
        )]
        [SerializeField]
        protected FighterAction[] actions = new FighterAction[ACTION_MAX];

        [Header("Markers")]
        [Tooltip("Position of foot. Should be down from center")]
        [SerializeField]
        protected Transform _markerFoot;

        /// <summary>
        /// Position of foot.
        /// </summary>
        public Vector3 positionFoot
        {
            get { return _markerFoot.position; }
        }

        [Tooltip(
            "Where detection ray of center starts. Used for landing detection and y-axis adjustment"
        )]
        [SerializeField]
        protected Transform _markerDetectionRayStartCenter;

        /// <summary>
        /// Where detection ray of center starts.
        /// </summary>
        public Vector3 positionDetectionRayStartCenter
        {
            get { return _markerDetectionRayStartCenter.position; }
        }

        [Tooltip("Where lanching attack objects")]
        [SerializeField]
        protected Transform _markerLauncher;

        /// <summary>
        /// Where lanching attack objects
        /// </summary>
        public Vector3 positionLauncher
        {
            get { return _markerLauncher.position; }
        }

        [Header("Objects")]
        [SerializeField]
        private CameraFighter cameraFighter;

        [SerializeField]
        [Tooltip("Set ActionColliders for this fighter")]
        private FighterActionColliderController.ActionColliderPrefabs actionColliders;

        [Header("Debug")]
        [Tooltip("Show debug interface in the scene view.")]
        [SerializeField]
        protected bool shouldShowDebugInterface = false;

        /// <summary>
        /// If this fighter is allowed to run.
        /// </summary>
        protected bool canTurn
        {
            get { return stateCurrent.movementAllowance.turning; }
        }

        /// <summary>
        /// If this fighter is allowed to run.
        /// </summary>
        protected bool canRun
        {
            get { return stateCurrent.movementAllowance.running; }
        }

        /// <summary>
        /// If this fighter is allowed to jump.
        /// </summary>
        protected bool canJump
        {
            get { return stateCurrent.movementAllowance.jumping; }
        }

        /// <summary>
        /// True if this fighter is runned in this frame.
        /// </summary>
        public bool runnedThisFrame { get; protected set; } = false;

        protected override void Awake()
        {
            base.Awake();

            //set initial hp
            hp = hpInitial;

            //initialize state controller instance.
            stateController = new FighterStateController(motionParametersDefaultInitial);

            //initialize action collider controller
            actionColliderController = new FighterActionColliderController(actionColliders);
        }

        protected override void Start()
        {
            base.Start();

            //report arrival to manager
            FightersManager.Instance.RegisterFighter(this);

            //get animation controller
            animationController = GetComponent<FighterAnimationController>();

            //initialize actions
            InitializeActions();

            //add to attacking objects
            attackingObjects.Add(this);
        }

        protected override void Update()
        {
            base.Update();

            //reset memory for single frame
            ActivateActions();

            //update actions
            UpdateActions();

            //update parameters for frame
            UpdateParameters();
        }

        /// <summary>
        /// Let client control this object
        /// </summary>
        public void RideOn()
        {
            //give visual
            ToggleCamera(true);

            //network control
            ViewOwnershipController viewOwnershipController =
                GetComponent<ViewOwnershipController>();
            if (viewOwnershipController != null)
            {
                viewOwnershipController.TakeOwnership();
            }
        }

        /// <summary>
        /// Set target
        /// </summary>
        public virtual void SetTarget(BattleObject target)
        {
            this.target = target;
        }

        /// <summary>
        /// Activate/Deactivate camera
        /// </summary>
        public void ToggleCamera(bool onOff)
        {
            cameraFighter.gameObject.SetActive(onOff);
        }

        /// <summary>
        /// Run forward.
        /// </summary>
        /// <param name="rate">0: stop, 1: full speed</param>
        public virtual void Run(float rate)
        {
            //skip if not allowed
            if (!canRun)
            {
                return;
            }

            //move
            transform.position +=
                transform.forward * motionParametersCurrent.runningSpeed * Time.deltaTime * rate;

            //set flag
            runnedThisFrame = true;
        }

        /// <summary>
        /// Rotate by y-axis, based on world coordinate.This will continue to turn until reach the destination.
        /// </summary>
        /// <param name="degreeDestination">horizontal angle (on zx plane, x+axis:0, z+axis:90) of destination.
        /// Doesn't have to be (-180, 180], because will be converted innerly</param>
        /// <returns>Turned angle</returns>
        public virtual float Turn(float degreeDestination)
        {
            //skip if not allowed
            if (!canTurn)
            {
                return 0f;
            }

            //>>limit angle
            float currentDegreeLimited = Utils.VectorToHorizontalAngle(transform.forward);
            float degreeDestinationLimited = Utils.LimitAngle(degreeDestination);

            //plus this to currentDegree to reach destination
            float degreeToTurn = Utils.LimitAngle(degreeDestinationLimited - currentDegreeLimited);
            //<<

            //if not turning
            if (Mathf.Approximately(degreeToTurn, 0))
            {
                //don't turn
                return 0f;
            }

            //turn
            int plusMinus = degreeToTurn > 0 ? 1 : -1;
            float delta =
                Mathf.Min(
                    Mathf.Abs(degreeToTurn),
                    motionParametersCurrent.turningSpeed * Time.deltaTime
                ) * plusMinus;
            transform.Rotate(0, -delta, 0);

            //return current degree
            return delta;
        }

        /// <summary>
        /// Rotate by y-axis, based on world coordinate.This will continue to turn until reach the destination.
        /// </summary>
        /// <param name="degreeDestination">Direction to look at. Only the horizontal component will be used</param>
        /// <returns>Turned angle</returns>
        public float Turn(Vector3 directionDestination)
        {
            return Turn(Utils.VectorToHorizontalAngle(directionDestination));
        }

        /// <summary>
        /// Turn and run in the same time.
        /// </summary>
        /// <param name="rate">[0, 1] rate for running speed</param>
        /// <param name="degreeDestination">destination degree to run. If fromTarget == true, relative degree</param>
        /// <param name="fromTarget">if true, will use TurnFromTarget()</param>
        /// <returns></returns>
        public virtual float TurnAndRun(
            float rate,
            float degreeDestination,
            bool fromTarget = false
        )
        {
            float turned = 0f;
            float degreeDestinationAbsolute = degreeDestination;
            if (fromTarget)
            {
                turned = TurnFromTarget(degreeDestination);
                degreeDestinationAbsolute = ConvertDegreeFromTargetToAbsolute(degreeDestination);
            }
            else
            {
                turned = Turn(degreeDestination);
                degreeDestinationAbsolute = degreeDestination;
            }

            //run only for parallel component
            float currentDegree = Utils.VectorToHorizontalAngle(transform.forward);
            float diffrence = Utils.LimitAngle(degreeDestinationAbsolute - currentDegree);
            float correction = Mathf.Cos(diffrence * Mathf.Deg2Rad);
            if (correction < 0)
            {
                correction = 0;
            }
            Run(rate * correction);

            return turned;
        }

        /// <summary>
        /// Rotate by y-axis, based on to-target pole coordinate.
        /// </summary>
        /// <param name="degreeRelativeDestination">Degrees shifted to lefthand-side from to-target vector. minus refers righthand-side</param>
        /// <returns>Current relative degree (-180, 180]</returns>
        public virtual float TurnFromTarget(float relativeDegreeDestination)
        {
            //turn
            return Turn(ConvertDegreeFromTargetToAbsolute(relativeDegreeDestination));
        }

        /// <summary>
        /// Invoke action. Keep calling for frames when keep inputing.
        /// Stop calling when stop inputing.
        /// </summary>
        /// <param name="actionNumber">Number of the action invoking</param>
        public bool InvokeAction(int actionNumber)
        {
            //skip if not allowed
            if (!stateCurrent.movementAllowance.action && actionNumber != stateCurrent.actionNumber)
            {
                return false;
            }

            actionsCurrentlyInvoking[actionNumber] = true;
            return true;
        }

        public bool StartAction(int actionNumber)
        {
            if (actionNumber == stateCurrent.actionNumber)
            {
                return false;
            }

            return InvokeAction(actionNumber);
        }

        /// <summary>
        /// Invoke action. Keep calling for frames when keep inputing.
        /// Stop calling when stop inputing.
        /// </summary>
        /// <param name="actionType">Type of the action invoking</param>
        /// <returns>if the action is invoked</returns>
        public bool InvokeAction(Type actionType)
        {
            Debug.Assert(actionType.IsSubclassOf(typeof(FighterAction)));

            for (int actionNumber = 0; actionNumber < ACTION_MAX; actionNumber++)
            {
                if (actions[actionNumber] != null && actions[actionNumber].GetType() == actionType)
                {
                    return InvokeAction(actionNumber);
                }
            }

            return false;
        }

        public bool StartAction(Type actionType)
        {
            Debug.Assert(actionType.IsSubclassOf(typeof(FighterAction)));

            for (int actionNumber = 0; actionNumber < ACTION_MAX; actionNumber++)
            {
                if (actions[actionNumber] != null && actions[actionNumber].GetType() == actionType)
                {
                    return StartAction(actionNumber);
                }
            }

            return false;
        }

        /// <summary>
        /// Update actions logics required to be called once per frame.
        /// </summary>
        public void UpdateActions()
        {
            for (int actionNumber = 0; actionNumber < ACTION_MAX; actionNumber++)
            {
                if (actions[actionNumber] != null)
                {
                    actions[actionNumber].Update();
                }
            }
        }

        /// <summary>
        /// Take damage. Called by other objects
        /// </summary>
        public void TakeDamage(int damageRequested)
        {
            hp -= damageRequested;
        }

        public override float GetHPRatio()
        {
            return (float)hp / hpInitial;
        }

        /// <summary>
        /// Start staggering.
        ///
        /// Controled by state controlling.
        /// If there is higher priority state, this will be ignored.
        /// </summary>
        /// <param name="staggerLevel">Level of staggering</param>
        /// <param name="flagResetTimer">If true, reset timer if already running</param>
        /// <param name="blowerObject">Where the object's position this fighter hit against</param>
        /// <returns>if the staggering successfully started</returns>
        public bool Stagger(
            StaggerLevel staggerLevel,
            bool flagResetTimer = false,
            Vector3? blowerPosition = null
        )
        {
            //request state and get if succeeded or not
            bool permission = stateController.RequestState(FighterStateController.stateNameStagger);

            //if not succeeded...
            if (!permission)
            {
                /// <summary>
                ///...early return
                /// </summary>
                return false;
            }

            //set stagger timer
            float duration;
            switch (staggerLevel)
            {
                case StaggerLevel.light:
                    duration = staggerDurationLight;
                    break;
                case StaggerLevel.heavy:
                    duration = staggerDurationHeavy;
                    break;
                case StaggerLevel.blown:
                    duration = staggerDurationBlown;
                    break;
                default:
                    throw new Exception("Unknown stagger level");
            }
            SetStaggerTimer(duration, flagResetTimer);

            //if blown, knock back
            if (staggerLevel == StaggerLevel.blown)
            {
                Debug.Assert(blowerPosition != null);
                Vector3 blowerPositionNotNull = blowerPosition ?? Vector3.zero;
                KnockBack(
                    blowerPositionNotNull,
                    staggerForceBlownHorizontal,
                    staggerForceBlownVertical
                );
            }

            return true;
        }

        /// <summary>
        /// On/Off gravity
        /// </summary>
        /// <param name="onOff">true:pn, false:off</param>
        public void SetGravity(bool onOff)
        {
            GetComponent<Rigidbody>().useGravity = onOff;
        }

        /// <summary>
        /// Start knock back motion
        /// </summary>
        /// <param name="opponentPosition">where the object's position this fighter hit against</param>
        /// <param name="forceHorizontal">force horizontal value (normal direction from attack)</param>
        /// <param name="forceVertical">force vertical value (upwards)</param>
        public void KnockBack(Vector3 oppopentPosition, float forceHorizontal, float forceVertical)
        {
            //normal vector
            Vector3 normal = this.transform.position - oppopentPosition;

            //make it to horizontal
            normal.y = 0;

            //normalize
            normal.Normalize();

            //make knockback velocity change vector
            Vector3 knockbackVelocityChange = normal * forceHorizontal + Vector3.up * forceVertical;

            //change velocity
            this.GetComponent<Rigidbody>()
                .AddForce(knockbackVelocityChange, ForceMode.VelocityChange);
        }

        /// <summary>
        /// convert to-target relative degree to absolute world-coordinate degree
        /// </summary>
        /// <param name="relativeDegree"> to-target relative degree</param>
        /// <returns>absolute world-coordinate degree (horizontal angle)</returns>
        private float ConvertDegreeFromTargetToAbsolute(float relativeDegree)
        {
            //to-target horizontal angle
            Vector3 toTarget = target.transform.position - transform.position;
            float angleToTarget = Utils.VectorToHorizontalAngle(toTarget);

            //shift
            //note: Unity is left-handed, y-up
            return angleToTarget + relativeDegree;
        }

        /// <summary>
        /// Invoke corresponding action event.
        /// </summary>
        private void ActivateActions()
        {
            for (int actionNumber = 0; actionNumber < ACTION_MAX; actionNumber++)
            {
                //skip if no action allocated
                if (actions[actionNumber] == null)
                {
                    continue;
                }

                //if not invoking
                if (!actionsCurrentlyInvoking[actionNumber])
                {
                    //if previously invoked
                    if (actionsPreviouslyInvoked[actionNumber])
                    {
                        //invoked -> not invoked: end
                        actions[actionNumber].End();
                    }
                }
                else
                {
                    if (!actionsPreviouslyInvoked[actionNumber])
                    {
                        //not invoked -> invoked: start
                        actions[actionNumber].Start();
                    }
                    else
                    {
                        //invoked -> invoked: continue
                        actions[actionNumber].Continue();
                    }
                }

                //move memory
                actionsPreviouslyInvoked[actionNumber] = actionsCurrentlyInvoking[actionNumber];

                //reset memory
                actionsCurrentlyInvoking[actionNumber] = false;
            }
        }

        /// <summary>
        /// Initialize Actions registered
        /// </summary>
        private void InitializeActions()
        {
            //initialize actions' property
            for (int cnt = 0; cnt < ACTION_MAX; cnt++)
            {
                if (actions[cnt] != null)
                {
                    actions[cnt] = Instantiate(actions[cnt]);
                    actions[cnt].Initialize(this);
                }
            }

            //register actions states
            for (int cnt = 0; cnt < ACTION_MAX; cnt++)
            {
                if (actions[cnt] != null)
                {
                    //aciton existing

                    State[] statesRequested = actions[cnt].GetStates();

                    foreach (State state in statesRequested)
                    {
                        //remember action index
                        state.actionNumber = cnt;

                        bool resultRegistering = stateController.RegisterState(state);

                        //should be able to register
                        //(no duplication)
                        Debug.Assert(resultRegistering);
                    }
                }
            }
        }

        /// <summary>
        /// Set stagger timer
        ///
        /// Reset when already running, or start new one.
        /// </summary>
        private void SetStaggerTimer(float duration, bool flagResetRunning)
        {
            //if timer already running...
            if ((timerStagger != null) && flagResetRunning)
            {
                //...reset
                timerStagger.SetTime(duration);
            }
            //if not running...
            else
            {
                //...start new one
                timerStagger = new Timer(duration, OnStaggetTimerIgnited);
            }
        }

        /// <summary>
        /// Invoked when stagger timer ran out
        /// </summary>
        private void OnStaggetTimerIgnited()
        {
            //erace timer
            timerStagger = null;

            //stop staggering
            stateController.EndState(FighterStateController.stateNameStagger);
        }

        /// <summary>
        /// Update parameters for frame in Update()
        /// </summary>
        private void UpdateParameters()
        {
            //reset runned flag
            //this will be set true if runned in this frame
            runnedThisFrame = false;
        }
    }
}
