
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private AudioSource _audioSource = default;

        [Header("SFX")]
        [SerializeField] private List<AudioClip> _explosionSounds = default;
        [SerializeField] private AudioClip _fuseSound = default;
        [SerializeField] private AudioClip _throwSound = default;
        [SerializeField] private AudioClip _pickupSound = default;

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

            _audioSource.PlayOneShot(_pickupSound);
            _audioSource.PlayOneShot(_throwSound);

            Debug.DrawLine(holdable.transform.position, holdable.transform.position + direction, Color.red, 10.0f);

            holder.ThrowCurrentHoldable(direction * _throwSpeed);

            holdable.canPickUp = false;
        }

        private void LightFuse(HoldableItem holdable, Holder holder)
        {
            _fuseParticles.Play(true);
            _audioSource.PlayOneShot(_fuseSound);

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

            _audioSource.Stop();
            _audioSource.PlayOneShot(_explosionSounds[Random.Range(0, _explosionSounds.Count)]);

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
                Debug.Log(hit.collider.name);
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