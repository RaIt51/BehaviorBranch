using UnityEngine;

namespace Simulation.Objects.Fighters.Actions
{
    /// <summary>
    /// Shoot collider bolt straightly forward.
    /// </summary>
    [CreateAssetMenu(fileName = "Thunderbolt", menuName = "ScriptableObjects/Actions/Thunderbolt")]
    public class Thunderbolt : FighterAction
    {
        const string stateName = "Thunderbolt";

        /// <summary>
        /// Bullet that currently shooting
        /// </summary>
        private ThunderboltBullet bullet;

        [SerializeField]
        private ThunderboltProperty property;

        public override void Initialize(Fighter fighter)
        {
            base.Initialize(fighter);

            states = new State[1];
        }

        /// <summary>
        /// Start this action
        /// </summary>
        public override void Start()
        {
            //cancel if working
            if (isWorking)
                return;

            base.Start();

            //start state
            bool allowed = fighter.stateController.RequestState(stateName);

            if (!allowed)
            {
                //state change failed
                //-> stop
                return;
            }

            //from lanching until collider disappears
            isWorking = true;

            //shoot
            Launch();
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

                //register interruption delegate
                states[0].onInterrupted.AddListener(onInterrupted);
            }

            return states;
        }

        /// <summary>
        /// Shoot the ThunderboltBullet
        /// </summary>
        private void Launch()
        {
            //generate object
            ThunderboltBullet bullet = GenerationManager.Instance
                .GenerateObject(
                    property.bulletPrefab.gameObject,
                    fighter.positionLauncher,
                    fighter.transform.rotation,
                    GenerationManager.Instance.parentBattleObjects
                )
                .GetComponent<ThunderboltBullet>();

            //set
            Vector3 direction = fighter.transform.forward;
            bullet.Initialize(
                fighter,
                property.damage,
                property.speed,
                direction,
                property.maxDistance
            );
            bullet.RegisterOnDisappear(OnBulletDisappear);

            //register to the fighter
            fighter.RegisterAttackingObject(bullet);
        }

        /// <summary>
        /// Called when the bullet disappears
        /// </summary>
        private void OnBulletDisappear()
        {
            //stop action
            StopAction();

            //forget
            bullet = null;
        }

        /// <summary>
        /// Control when the action is interrupted.
        /// </summary>
        private void onInterrupted(bool systemInterruption)
        {
            //system interruption
            //-> it will resume automatically
            if (systemInterruption)
                return;

            //interrupted by other action
            //-> stop this action

            //erace bullet
            Destroy(bullet.gameObject);

            //stop action
            isWorking = false;
        }

        /// <summary>
        /// /// Stop this action
        /// </summary>
        private void StopAction()
        {
            //stop action
            isWorking = false;

            //stop state
            fighter.stateController.EndState(stateName);
        }
    }
}
