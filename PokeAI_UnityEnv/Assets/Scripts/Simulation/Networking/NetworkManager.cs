using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Simulation.Objects;

namespace Simulation
{
    /// <summary>
    /// Controls networking
    /// </summary>
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static string roomID = "test";
        public int myNumber { get; private set; } = -1;

        [Header("Objects")]
        [Tooltip(
            "client allocated to the index corresponding to their player number. This needs to have `controllable` interface"
        )]
        [SerializeField]
        protected GameObject[] controllables;

        [Tooltip("the index correspoinding to their player number will be instantiated")]
        [SerializeField]
        protected Controller[] controllers;

        private PhotonEntity myEntity;

        protected virtual void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            PhotonNetwork.JoinOrCreateRoom(roomID, new RoomOptions(), TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            //get my(client) number
            myEntity = PhotonNetwork
                .Instantiate("PhotonEntity", Vector3.zero, Quaternion.identity)
                .GetComponent<PhotonEntity>();

            WaitForPlayerNumber();
        }

        /// <summary>
        /// Discontinually repeatly called until player number is assigned
        /// </summary>
        protected virtual void WaitForPlayerNumber()
        {
            myNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();

            Debug.Log("Player number: " + myNumber);

            if (myNumber == -1)
            {
                //not assigned yet

                Invoke(nameof(WaitForPlayerNumber), 0.1f);
            }
            else
            {
                //assigned

                RideOnPlayerObject(myNumber);
            }
        }

        /// Let client control their player object
        /// </summary>
        /// <param name="number"></param>
        protected void RideOnPlayerObject(int number)
        {
            //rideon
            Controllable controllable = controllables[number].GetComponent<Controllable>();
            if (controllable == null)
            {
                Debug.LogError("Controllable not found");
                return;
            }
            controllable.RideOn();

            //give control
            Controller controller = Instantiate(controllers[number].gameObject)
                .GetComponent<Controller>();
            controller.AllocateTarget(controllable);
        }
    }
}
