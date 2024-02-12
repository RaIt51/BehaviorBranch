using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Simulation.Objects.Fighters
{
    /// <summary>
    /// Listener of animation event
    /// </summary>
    public class FighterAnimationListener
    {
        public string eventName;
        public UnityAction onEvent;

        public FighterAnimationListener(string eventName, UnityAction onEvent)
        {
            this.eventName = eventName;
            this.onEvent = onEvent;
        }
    }

    /// <summary>
    /// Controls & listen animation of a fighter.
    /// </summary>
    [RequireComponent(typeof(Fighter))]
    public class FighterAnimationController : MonoBehaviour
    {
        /// <summary>
        ///Fighter that this controller controls
        /// </summary>
        private Fighter fighter;

        /// <summary>
        /// Animator of the fighter
        /// </summary>
        private Animator animator;

        List<FighterAnimationListener> listeners = new List<FighterAnimationListener>();

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Start animation
        /// </summary>
        /// <param name="animationTrigger">Animation trigger name that written in Animator</param>
        public void StartAnimation(string animationTrigger)
        {
            animator.SetTrigger(animationTrigger);
        }

        /// <summary>
        /// Start listening to animation event
        /// </summary>
        public void AddListener(FighterAnimationListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Stop listening to animation event
        /// </summary>
        /// <returns>Whether removement was successful</returns>
        public bool RemoveListener(FighterAnimationListener listener)
        {
            return listeners.Remove(listener);
        }

        /// <summary>
        /// Ignite event from Animator
        /// </summary>
        /// <param name="eventName">event name</param>
        public void IgniteEvent(string eventName)
        {
            //Reverse iterate because listener may be removed during iteration
            for (int cnt = listeners.Count - 1; cnt >= 0; cnt--)
            {
                if (listeners[cnt].eventName == eventName)
                {
                    listeners[cnt].onEvent();
                }
            }
        }
    }
}
