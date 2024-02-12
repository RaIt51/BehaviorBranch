using UnityEngine;

namespace AI.BehaviourBranch.Logistics
{
    /// <summary>
    /// Connect with Python and receive new BehaviourBranch
    /// </summary>
    public class InputTesterConnector : BranchConnector
    {
        [SerializeField]
        private string[] branchJson = new string[10];

        protected override void Start() { }

        protected override void Update()
        {
            base.Update();

            for (int cnt = 0; cnt < 10; cnt++)
            {
                if (Input.GetKeyDown((KeyCode)(cnt + 48)))
                {
                    string data = "<s>branch!{\"nodes\":" + branchJson[cnt] + "}<e>";
                    Debug.Log(data);
                    OnDataReceived(data);
                }
            }
        }
    }
}
