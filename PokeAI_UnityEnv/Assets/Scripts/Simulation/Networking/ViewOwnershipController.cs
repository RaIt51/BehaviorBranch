using Photon.Pun;
using UnityEngine;

namespace Simulation
{
    /// <summary>
    /// Controls the view ownership of this object
    /// </summary>
    public class ViewOwnershipController : MonoBehaviour
    {
        /// <summary>
        /// Photon view of this object
        /// </summary>
        private PhotonView photonView
        {
            get { return GetComponent<PhotonView>(); }
        }

        /// <summary>
        /// Take view ownership of this object
        /// </summary>
        public void TakeOwnership()
        {
            if (!photonView.IsMine)
            {
                photonView.RequestOwnership();
            }
        }
    }
}
