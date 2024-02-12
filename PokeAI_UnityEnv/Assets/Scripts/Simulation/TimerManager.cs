using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    /// <summary>
    /// Manage all timers used in this system
    /// </summary>
    public class TimerManager: SingletonMonoBehaviour<TimerManager>
    {
        /// <summary>
        /// timers currently running
        /// </summary>
        private List<Timer> timers = new List<Timer>();

        private void Update()
        {
            AdvanceTimers();
        }

        /// <summary>
        /// Register new timer.
        /// 
        /// Won't be added when duplicated.
        /// </summary>
        public void RegisterTimer(Timer timer)
        {
            //if already registered...
            if (timers.Contains(timer))
            {
                //...do nothing
                return;
            }

            timers.Add(timer);
        }

        public void UnregisterTimer(Timer timer)
        {
            timers.Remove(timer);
        }

        /// <summary>
        /// Advance all timers.
        /// 
        /// Delete if there is a timer that has finished.
        /// </summary>
        private void AdvanceTimers()
        {
            //for all timers
            for (int cnt = 0; cnt < timers.Count; cnt++)
            {
                //advance timer
                //if finished...
                timers[cnt].Advance(Time.deltaTime);
            }
        }
    }
}