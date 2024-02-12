using UnityEngine;

namespace Simulation.Objects.Fighters.Actions
{
    /// <summary>
    /// /// Actions that a fighter can perform.
    /// Hold one instance for each action.
    /// </summary>
    public abstract class FighterAction : ScriptableObject
    {
        /// <summary>
        /// If the action is working
        /// </summary>
        public bool isWorking { get; protected set; } = false;

        /// <summary>
        /// Fighter holds this action
        /// </summary>
        public Fighter fighter { get; protected set; }

        //for innerly setting
        [SerializeField]
        private StringTranslated _name;

        /// <summary>
        /// Name of this action
        /// </summary>
        public string nameAction
        {
            get { return _name.Get(Languages.languageDisplaying); }
        }

        /// <summary>
        /// State instance for this action.
        /// This is the same instance with the fighter holding.
        /// </summary>
        protected State[] states;

        /// <summary>
        /// Initialize settings
        /// </summary>
        public virtual void Initialize(Fighter fighter)
        {
            this.fighter = fighter;
        }

        /// <summary>
        /// Start the action
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Continu the action. Call this from second frame.
        /// </summary>
        public virtual void Continue() { }

        /// <summary>
        /// End the action
        /// </summary>
        public virtual void End() { }

        /// <summary>
        /// Should call this once per frame
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Get State instances all used for this action. Make one if none.
        /// </summary>
        public abstract State[] GetStates();
    }
}
