using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGame
{
    public class ExplodableWall : MonoBehaviour, IExplodable
    {
        [SerializeField] ParticleSystem _debrisParticles = default;
        [SerializeField] GameObject _wallMesh = default;
        [SerializeField] BoxCollider _boundsCollider = default;

        [Header("SFX")]
        [SerializeField] List<AudioClip> _explodeSounds = default;

        void Start()
        {
        }

        public void Explode(Bomb bomb)
        {
            _wallMesh.SetActive(false);
            _boundsCollider.enabled = false;
            _debrisParticles.Play(true);

            GetComponent<AudioSource>().PlayOneShot(_explodeSounds[Random.Range(0, _explodeSounds.Count)]);
        }

        void OnValidate()
        {
            _debrisParticles.transform.position = _boundsCollider.transform.TransformPoint(_boundsCollider.center);

            var shapeModule = _debrisParticles.shape;
            shapeModule.position = Vector3.zero;
            shapeModule.scale = _boundsCollider.bounds.size;

            
        }
    }
}