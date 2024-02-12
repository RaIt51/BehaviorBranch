using UnityEngine;
using UnityEngine.Events;
using Simulation.Objects.Fighters;

namespace Simulation.Objects
{
    /// <summary>
    /// Collider object that triggers when bullet hits something
    /// </summary>
    public abstract class Bullet : MonoBehaviour, AttackingObject
    {
        /// <summary>
        /// Used if needed
        /// </summary>
        protected float speed;

        /// <summary>
        /// Used if needed
        /// </summary>
        protected Vector3 direction;

        /// <summary>
        /// Used if needed
        /// </summary>
        protected int damage = 0;

        /// <summary>
        /// How far this flew currently
        /// </summary>
        protected float flewDistance = 0;

        /// <summary>
        /// Max distance this can fly
        /// </summary>
        protected float maxDistance = 100;

        /// <summary>
        /// How much the hitter staggered when hit with this
        /// </summary>
        protected Fighter.StaggerLevel staggerLevel = Fighter.StaggerLevel.light;

        /// <summary>
        /// Who lanched this
        /// </summary>
        public BattleObject lancher { get; protected set; }

        /// <summary>
        /// Called when this disappears
        /// </summary>
        private UnityEvent onDisappear = new UnityEvent();

        public Vector3 attackingDirection
        {
            get { return direction; }
        }

        protected virtual void Start() { }

        protected virtual void Update()
        {
            Fly();

            //disappear if flew too far
            if (flewDistance >= maxDistance)
            {
                Disappear();
            }
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            //don't hit lancher
            if (other.gameObject == lancher.gameObject)
                return;

            //hitted with fighter
            if (other.gameObject.GetComponent<Fighter>() is Fighter fighter)
            {
                OnFighterHit(fighter);
            }

            OnOtherObjectHit(other.gameObject);
        }

        /// <summary>
        /// The action will be called when this disappears
        /// </summary>
        public void RegisterOnDisappear(UnityAction action)
        {
            onDisappear.AddListener(action);
        }

        /// <summary>
        /// Determines how bullet flies
        /// </summary>
        protected abstract void Fly();

        /// <summary>
        /// Fky straightly forward + no acceleration
        /// </summary>
        protected void FlyUniformLinearMotion()
        {
            float flyingDistance = speed * Time.deltaTime;

            //move
            transform.position += direction.normalized * flyingDistance;

            flewDistance += flyingDistance;
        }

        /// <summary>
        /// Called when this hits a fighter
        /// </summary>
        protected virtual void OnFighterHit(Fighter fighter)
        {
            fighter.TakeDamage(damage);
            fighter.Stagger(staggerLevel);
        }

        /// <summary>
        /// When this hits other object than handled classes
        /// </summary>
        protected virtual void OnOtherObjectHit(GameObject hitter)
        {
            Disappear();
        }

        /// <summary>
        /// Called when this disappears
        /// </summary>
        protected virtual void Disappear()
        {
            //trigger disappear event
            onDisappear.Invoke();

            //disappear from the world
            Destroy(gameObject);
        }
    }
}
