using UnityEngine;
using Simulation.Objects;
using Simulation.Objects.Fighters;

namespace Simulation.Controllers.Fighters
{
    /// <summary>
    /// Empty object enables player controls a Fighter via keyboard/game-pad.
    /// </summary>
    public abstract class FighterManualController : Controller
    {
        [Header("who to control.")]
        [SerializeField]
        protected Fighter fighterTarget;

        [Header("key/pad-button config")]
        [SerializeField]
        private string runForwardKey = "w";

        [SerializeField]
        private string runBackwardKey = "s";

        [SerializeField]
        private string runLeftKey = "a";

        [SerializeField]
        private string runRightKey = "d";

        [SerializeField]
        private string[] actionKeys = { "space", "j", "k" };

        [Header("gamepad config")]
        [SerializeField]
        private string gamepadHorizontalAxis = "Horizontal";

        [SerializeField]
        private string gamepadVerticalAxis = "Vertical";

        [Header("Parameters")]
        [SerializeField]
        private float stickThreasold = 0.1f;

        private void Update()
        {
            HandleInput();
        }

        /// <summary>
        /// Check input and command Figher
        /// </summary>
        private void HandleInput()
        {
            HandleMovementInput();
            HandleActionInput();
        }

        /// <summary>
        /// About run/turn input.
        /// Gets running ratio and destination angle.
        /// </summary>
        private void HandleMovementInput()
        {
            //get input information
            bool noInput = false;

            float padVertiacal = Input.GetAxis(gamepadVerticalAxis);
            float padHorizontal = Input.GetAxis(gamepadHorizontalAxis);

            float ratio = Mathf.Sqrt(padVertiacal * padVertiacal + padHorizontal * padHorizontal);
            float angle = 0;

            if (ratio >= stickThreasold)
            {
                //pad used
                angle = Mathf.Atan2(padVertiacal, padHorizontal) * Mathf.Rad2Deg;
                noInput = true;
            }
            else
            {
                //pad not used.
                angle = 0f;
                if (Input.GetKey(runForwardKey))
                {
                    ratio = 1f;
                    angle += 90f;

                    noInput = true;
                }
                if (Input.GetKey(runBackwardKey))
                {
                    ratio = 1f;
                    angle += -90f;

                    noInput = true;
                }
                if (Input.GetKey(runLeftKey))
                {
                    ratio = 1f;
                    angle += 180f;

                    noInput = true;
                }
                if (Input.GetKey(runRightKey))
                {
                    ratio = 1f;
                    angle += 0f;

                    noInput = true;
                }
            }

            if (noInput)
            {
                Move(ratio, angle);
            }
        }

        /// <summary>
        /// About action input.
        /// </summary>
        private void HandleActionInput()
        {
            //check each action key
            for (int i = 0; i < actionKeys.Length; i++)
            {
                if (Input.GetKey(actionKeys[i]))
                {
                    fighterTarget.InvokeAction(i);
                }
            }
        }

        /// <summary>
        /// Actually moves the Fighter
        /// </summary>
        protected abstract void Move(float ratio, float angle);

        /// <summary>
        /// Set target
        /// </summary>
        public override void AllocateTarget(Controllable target)
        {
            //check it is a fighter
            if (target is Fighter)
            {
                fighterTarget = (Fighter)target;
            }
            else
            {
                Debug.LogError("Cannot control non-fighter object");
            }
        }
    }
}
