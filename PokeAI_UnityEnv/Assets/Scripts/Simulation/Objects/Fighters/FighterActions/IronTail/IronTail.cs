using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Simulation.Objects.Fighters.Actions
{
    /// <summary>
    /// IronTail action
    /// </summary>
    [CreateAssetMenu(fileName = "IronTail", menuName = "ScriptableObjects/Actions/IronTail")]
    public class IronTail : FighterAction
    {
        //state names
        private const string stateName = "IronTail";

        [Tooltip("Property of this action")]
        [SerializeField]
        private IronTailProperty property;

        private FighterAnimationListener animationFinishedListener;

        private int colliderId;

        private bool flagHitted = false;

        public override void Initialize(Fighter fighter)
        {
            base.Initialize(fighter);

            states = new State[1];
        }

        public override State[] GetStates()
        {
            if (states[0] == null)
            {
                //no state made
                //-> make one.

                states[0] = new State(
                    stateName,
                    new State.MovementAllowance(false, false, false, false),
                    fighter.stateController.priorityAction,
                    false
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
            bool allowed = fighter.stateController.RequestState(stateName);

            //if not allowed...
            if (!allowed)
            {
                //...stop
                return;
            }

            //... allowed
            //-> start action
            isWorking = true;

            //start animation
            fighter.animationController.StartAnimation(property.animationTrigger);

            //listen to finish
            animationFinishedListener = new FighterAnimationListener(
                property.animationEventFinished,
                FinishAction
            );
            fighter.animationController.AddListener(animationFinishedListener);

            //generate collider
            colliderId = fighter.actionColliderController.GenerateCollider(
                this,
                FighterActionColliderController.ColliderPart.tail,
                OnHit
            );

            //reset flag
            flagHitted = false;

            //cut gravity for animation
            fighter.SetGravity(false);
        }

        private void OnHit(Fighter fighterHitted)
        {
            //avoid duplication
            if (flagHitted)
                return;

            fighterHitted.TakeDamage(property.damage);
            fighterHitted.Stagger(
                Fighter.StaggerLevel.blown,
                blowerPosition: fighter.transform.position
            );

            flagHitted = true;
        }

        /// <summary>
        /// Finish action and go to idle state
        /// </summary>
        private void FinishAction()
        {
            //stop animation
            fighter.animationController.RemoveListener(animationFinishedListener);

            //stop this action
            isWorking = false;

            //to idle
            fighter.stateController.EndState(stateName);

            //return gravity
            fighter.SetGravity(true);

            //destroy collider
            fighter.actionColliderController.DeleteCollider(colliderId);

            //quit state
            fighter.stateController.EndState(stateName);
        }
    }
}
