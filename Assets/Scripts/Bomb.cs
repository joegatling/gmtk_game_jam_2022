using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGame
{

    [RequireComponent(typeof(HoldableItem))]
    public class Bomb : MonoBehaviour
    {
        public System.Action<int> onExplode = delegate { };

        [SerializeField] float _throwSpeed = 1.0f;

        [Space]

        [SerializeField] float _fuseTime = 4.0f;
        [SerializeField] private Color _startColor = default;
        [SerializeField] private Color _endColor = default;

        [Space]
        [SerializeField] float _blastRadius = 4.0f;
        [SerializeField] LayerMask _explodableMask = default;

        [Header("Internal References")]
        [SerializeField] SpriteRenderer _mainRenderer = default;
        [SerializeField] private ParticleSystem _fuseParticles = default;
        [SerializeField] private ParticleSystem _explodeParticles = default;

        HoldableItem _holdableItem = default;
        public HoldableItem holdableItem => _holdableItem;

        // Start is called before the first frame update
        void Awake()
        {
            _holdableItem = GetComponent<HoldableItem>();
            _holdableItem.onUse += ThrowBomb;
            _holdableItem.onStartHolding += LightFuse;

            
        }

        private void Update()
        {
            //if(_holdableItem.isBeingHeld == false)
            //{

            //    if(GetComponent<Rigidbody>().IsSleeping())
            //    {
            //        _holdableItem.SetLayer("Objects");
            //        _holdableItem.canPickUp = true;
            //    }

            //}
        }


        [ContextMenu("Test Throw")]
        void TestThrow()
        {

            Vector3 direction = Vector3.right;
            direction.y = 1.0f;
            direction.Normalize();

            GetComponent<Rigidbody>().AddForce(direction*_throwSpeed, ForceMode.VelocityChange);
        }

        private void ThrowBomb(HoldableItem holdable, Holder holder)
        {

            Vector3 direction = holder.GetComponent<Player>().facingDirection;
            direction.y = 1.0f;
            direction.Normalize();

            Debug.DrawLine(holdable.transform.position, holdable.transform.position + direction, Color.red, 10.0f);

            holder.ThrowCurrentHoldable(direction * _throwSpeed);

            holdable.canPickUp = false;
        }

        private void LightFuse(HoldableItem holdable, Holder holder)
        {
            _fuseParticles.Play(true);

            StartCoroutine(IEExplodeAfterFuse(holder));

        }

        private IEnumerator IEExplodeAfterFuse(Holder holder)
        {
            float timeRemaining = _fuseTime;

            while(timeRemaining > 0)
            {
                float t = Mathf.Clamp01(timeRemaining / _fuseTime);
                _mainRenderer.color = Color.Lerp(_endColor, _startColor, t);

                timeRemaining -= Time.deltaTime;
                yield return null;
            }

            if (_holdableItem.isBeingHeld)
            {
                holder.DropCurrentHoldable();
            }

            _fuseParticles.Stop(true);
            _explodeParticles.Play(true);

            ExplodeObjects();

            _mainRenderer.enabled = false;

            Destroy(gameObject, 3.0f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _blastRadius);
        }

        private void ExplodeObjects()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit[] hits = Physics.SphereCastAll(ray, _blastRadius, 0.01f, _explodableMask, QueryTriggerInteraction.Ignore);

            foreach(var hit in hits)
            {
                IExplodable explodable = hit.collider.GetComponent<IExplodable>();

                if(explodable != null)
                {
                    explodable.Explode(this);
                }
            }

            onExplode(hits.Length);
        }
    }
}