using UnityEngine;
using UnityEngine.UI;
using Simulation.Objects;

namespace Simulation.UI
{
    /// <summary>
    /// HP bar that shows the ratio of the hp
    /// </summary>
    public class HPBar : MonoBehaviour
    {
        [Tooltip("Battle object that this HP bar indicates. GetHPRatio() should be implemented.")]
        [SerializeField]
        private BattleObject targetObject;

        [Tooltip("HP fill that shows ratio")]
        [SerializeField]
        private Image imageFiller;

        private void Update()
        {
            UpdateUI();
        }

        /// <summary>
        /// Updating UI, getting HP ratio
        /// </summary>
        private void UpdateUI()
        {
            //get
            float ratio = targetObject.GetHPRatio();

            //show
            imageFiller.fillAmount = ratio;
        }
    }
}
