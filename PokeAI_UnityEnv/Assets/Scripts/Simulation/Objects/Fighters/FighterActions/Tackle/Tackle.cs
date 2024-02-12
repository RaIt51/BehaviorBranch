using UnityEngine;

namespace Simulation.Objects.Fighters.Actions
{
    /// <summary>
    /// Tackle action.
    ///
    /// Fastly, straightly run to front.
    /// If hit a BattleObject, give damage and this fighter knockback.
    /// If not, smoothly turn to idle state
    /// </summary>
    [CreateAssetMenu(fileName = "Tackle", menuName = "ScriptableObjects/Actions/Tackle")]
    public class Tackle : FighterAction
    {
        //state names
        private const string stateNameDashing = "Dashing";
        private const string stateNameKnockback = "DashKnockback";

        [Tooltip("Property of this action")]
        [SerializeField]
        private TackleProperty property;

        /// <summary>
        /// Motion parameters for dashing
        /// </summary>
        private MotionParametersFighter motionParametersDashing;

        /// <summary>
        /// Collider ID of this action generated
        /// </summary>
        private int colliderId;

        /// <summary>
        /// Timer for keep dashing
        /// </summary>
        private Timer timerDashing;

        /// <summary>
        /// Timer for handling knockback
        /// </summary>
        private Timer timerKnockback;

        public override void Initialize(Fighter fighter)
        {
            base.Initialize(fighter);

            states = new State[2];

            //motion parameters
            motionParametersDashing = new MotionParametersFighter(
                property.speed,
                property.turnSpeed
            );
        }

        public override State[] GetStates()
        {
            if (states[0] == null)
            {
                //no state made
                //-> make

                states[0] = new State(
                    stateNameDashing,
                    new State.MovementAllowance(true, false, false, false),
                    fighter.stateController.priorityAction,
                    false,
                    false
                );

                //set motion parameters
                states[0].SetMotionParameters(motionParametersDashing);

                states[1] = new State(
                    stateNameKnockback,
                    new State.MovementAllowance(false, false, false, false),
                    fighter.stateController.priorityStagger,
                    false,
                    true
                );
            }

            return states;
        }

        public override void Start()
        {
            //cancel already working
            if (isWorking)
                return;

            base.Start();

            //start this acting state
            bool allowed = fighter.stateController.RequestState(stateNameDashing);

            //if not allowed...
            if (!allowed)
            {
                //...stop
                return;
            }

            //... allowed
            //-> start dashing
            isWorking = true;

            //generate collider
            colliderId = fighter.actionColliderController.GenerateCollider(
                this,
                FighterActionColliderController.ColliderPart.body,
                OnHit
            );

            //start timer
            //finish timer when timer finished
            timerDashing = new Timer(property.duration, FinishAction);
        }

        public override void Continue()
        {
            base.Continue();

            if (!isWorking)
                return;

            //update timer
            timerDashing.SetTime(property.duration);
        }

        public override void Update()
        {
            base.Update();

            // skip if this action not working
            if (!isWorking)
                return;

            //if now dashing...
            if (fighter.stateController.stateCurrent.name == stateNameDashing)
            {
                //...make the fighter running
                EnsureRunning();
            }
        }

        /// <summary>
        /// Called when hit another Fighter
        /// </summary>
        /// <param name="fighterHitted">Fighter hitted</param>
        private void OnHit(Fighter fighterHitted)
        {
            //fix if penetrated
            FixPenetration(fighterHitted);

            //give damage
            fighterHitted.TakeDamage((int)property.damage);

            //blow hitted fighter
            Vector3 blowerPosition = fighterHitted.transform.position;
            fighterHitted.Stagger(Fighter.StaggerLevel.blown, blowerPosition: blowerPosition);

            //stop dashing timer
            timerDashing.Unregister();

            //This fighter knockback
            Knockback(fighterHitted);
        }

        /// <summary>
        /// This fighter get knockback after hitting another fighter
        /// </summary>
        private void Knockback(Fighter fighterHitted)
        {
            //change state
            // will be unmovable by this
            fighter.stateController.RequestState(stateNameKnockback);

            //move
            fighter.KnockBack(
                fighterHitted.transform.position,
                property.knockbackAccelerationHorizontal,
                property.knockbackAccelerationUp
            );

            //start timer
            timerKnockback = new Timer(property.knockbackDuration, FinishAction);
        }

        /// <summary>
        /// Immediately finish this action
        /// </summary>
        private void FinishAction()
        {
            //stop dashing
            isWorking = false;

            //stop collider
            fighter.actionColliderController.DeleteCollider(colliderId);

            //quit all possible state
            fighter.stateController.EndState(stateNameDashing);
            fighter.stateController.EndState(stateNameKnockback);

            //unregister timers
            if (timerDashing != null)
            {
                timerDashing.Unregister();
            }
            if (timerKnockback != null)
            {
                timerKnockback.Unregister();
            }
        }

        /// <summary>
        /// Ensure running while this action working
        /// </summary>
        private void EnsureRunning()
        {
            //if player didn't command running...
            if (!fighter.runnedThisFrame)
            {
                //...make fighter run
                fighter.Run(1f);
            }
        }

        /// <summary>
        /// Fix the relative positions of this fighter and hitted fighter
        ///     when this fighter penetrated the hitted fighter because of descrete movement
        /// </summary>
        /// <param name="fighterHitted"></param>
        private void FixPenetration(Fighter fighterHitted)
        {
            //check penetration
            Vector3 relative = fighterHitted.transform.position - fighter.transform.position;
            //if this->hitted direction is same as this.forward...
            if (Vector3.Dot(relative, fighter.transform.forward) > 0)
            {
                //...means not penetrated
                return;
            }

            //...penetrated

            //fix penetration
            //move this fighter back
            fighter.transform.position += relative * 2;
        }
    }
}
