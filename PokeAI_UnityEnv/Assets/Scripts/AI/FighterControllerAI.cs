using System;
using Simulation;
using Simulation.Objects;
using Simulation.Objects.Fighters;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Base class of AI controller for Fighter
    /// </summary>
    [RequireComponent(typeof(FighterWatcher))]
    public abstract class FighterControllerAI : Controller
    {
        [SerializeField]
        protected Fighter fighterTarget;

        public FighterWatcher fighterWatcher
        {
            get { return fighterTarget.GetComponent<FighterWatcher>(); }
        }

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
