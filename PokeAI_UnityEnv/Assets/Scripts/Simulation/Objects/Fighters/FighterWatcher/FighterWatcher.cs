using UnityEngine;

namespace Simulation.Objects.Fighters
{
    [RequireComponent(typeof(Fighter))]
    public class FighterWatcher : MonoBehaviour
    {
        protected Fighter fighter;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
        }

        public float distanceFromTarget
        {
            get
            {
                Vector3 positionTarget = fighter.target.transform.position;
                Vector3 positionThisFighter = fighter.transform.position;
                return Vector3.Distance(positionTarget, positionThisFighter);
            }
        }

        public AttackingObject[] attackingObjects
        {
            get { return fighter.GetAttackingObjects(); }
        }

        public Vector3 position
        {
            get { return fighter.transform.position; }
        }

        public BattleObject target
        {
            get { return fighter.target; }
        }

        public string state
        {
            get { return fighter.stateController.stateCurrent.name; }
        }

        [SerializeField]
        protected float _irontailRange = 5f;
        public float irontailRange
        {
            get { return _irontailRange; }
        }
    }
}
