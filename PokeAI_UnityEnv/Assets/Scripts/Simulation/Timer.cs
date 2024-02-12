using UnityEngine.Events;

namespace Simulation
{
    /// <summary>
    /// timer that call registered function after given time
    ///
    /// TimerManager manages all timers.
    /// Need to call Unregister() when deleting this Timer
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// Left time to call the function.
        /// Seconds.
        /// </summary>
        private float time;

        /// <summary>
        /// Function to call when the time comes.
        /// </summary>
        private UnityAction action;

        /// <summary>
        /// Shows wheter the action is ignited or not.
        /// this will be reset when time is set.
        /// </summary>
        public bool flagIgnited { get; private set; } = false;

        /// <summary>
        /// Timer finished or not
        /// </summary>
        public bool finishedFlag
        {
            get { return time <= 0; }
        }

        /// <param name="time">Time to set</param>
        /// <param name="action">function called when time comes</param>
        public Timer(float time, UnityAction action)
        {
            SetTime(time);
            this.action = action;
        }

        /// <summary>
        /// Advance timer.
        ///
        /// The function is called when the time comes.
        /// </summary>
        /// <return>Timer finished or not</return>
        public bool Advance(float deltaTime)
        {
            time -= deltaTime;
            if ((time <= 0) && !flagIgnited)
            {
                if (action != null) //null guard
                {
                    action();
                }

                //remember that the action is ignited
                //for not repeating action()
                flagIgnited = true;
            }

            return finishedFlag;
        }

        /// <summary>
        /// Newly set time.
        ///
        /// Also put into TimerManager.
        /// </summary>
        public void SetTime(float time)
        {
            this.time = time;
            flagIgnited = false;

            Register();
        }

        /// <summary>
        /// Newly set action
        /// </summary>
        public void SetAction(UnityAction action)
        {
            this.action = action;
        }

        /// <summary>
        /// Unregister this timer from TimerManager
        /// </summary>
        public void Unregister()
        {
            TimerManager.Instance.UnregisterTimer(this);
        }

        /// <summary>
        /// Register this timer to TimerManager
        /// </summary>
        public void Register()
        {
            TimerManager.Instance.RegisterTimer(this);
        }
    }
}
