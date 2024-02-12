using System;
using Logistics.PythonConnection;

namespace AI.BehaviourBranch.Logistics
{
    /// <summary>
    /// Connect with Python and receive new BehaviourBranch
    /// </summary>
    public class BranchConnector : PythonConnector
    {
        protected virtual void Start()
        {
            StartConnection();
        }

        private void OnDisable()
        {
            StopConnection();
        }

        public void SendCommand(string command)
        {
            Command instance = new Command() { command = command };

            Send("command", instance);
        }

        [Serializable]
        private class Command : DataClass
        {
            public string command;
        }
    }
}
