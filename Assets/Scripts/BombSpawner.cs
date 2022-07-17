using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGame
{
    public class BombSpawner : MonoBehaviour
    {
        public enum SpawnType
        {
            Continuous,
            AfterPreviousExplodes,
            AfterPreviousCollected

        }
        [SerializeField] private Bomb _optionalInitialBomb = null;

        [Space]

        [SerializeField] private SpawnType _spawnType = SpawnType.AfterPreviousCollected;
        [SerializeField] private float _delay = 3.0f;

        [Space]
        [SerializeField] private float _launchSpeed = 1.0f;

        [Space]
        [SerializeField] private Bomb _bombPrefab = default;

        [Header("SFX")]
        [SerializeField] private AudioClip _spawnSound = default;

        Bomb _lastSpawnedBomb = null;

        private void Start()
        {
            if(_optionalInitialBomb != null)
            {
                _lastSpawnedBomb = _optionalInitialBomb;
                RegisterCurrentBomb();
            }
            else
            {
                SpawnBomb();
            }
        }

        void SpawnBomb()
        {
            if(_bombPrefab != null)
            {
                if(_lastSpawnedBomb != null)
                {
                    _lastSpawnedBomb.holdableItem.onStartHolding -= OnStartHolding;
                    _lastSpawnedBomb.onExplode -= OnBombExplode;
                    _lastSpawnedBomb = null;
                }

                _lastSpawnedBomb = Instantiate<Bomb>(_bombPrefab);
                _lastSpawnedBomb.transform.SetParent(transform, false);
                _lastSpawnedBomb.transform.rotation = Quaternion.identity;

                GetComponent<AudioSource>().PlayOneShot(_spawnSound);

                _lastSpawnedBomb.transform.position = transform.position;
                var r = _lastSpawnedBomb.gameObject.GetComponent<Rigidbody>();
                r.AddForce(transform.forward * _launchSpeed, ForceMode.VelocityChange);

                RegisterCurrentBomb();
            }
        }

        void RegisterCurrentBomb()
        {
            if (_spawnType == SpawnType.Continuous)
            {
                StartCoroutine(IESpawnBombAfterDelay());
            }
            else if (_spawnType == SpawnType.AfterPreviousExplodes)
            {
                _lastSpawnedBomb.onExplode += OnBombExplode;
            }
            else if (_spawnType == SpawnType.AfterPreviousCollected)
            {
                _lastSpawnedBomb.holdableItem.onStartHolding += OnStartHolding;
            }
        }

        void UnregisterCurrentBomb()
        {
            _lastSpawnedBomb.holdableItem.onStartHolding -= OnStartHolding;
            _lastSpawnedBomb.onExplode -= OnBombExplode;
            _lastSpawnedBomb = null;
        }

        private void OnBombExplode(int numberOfObjectsDestroyed)
        {
            UnregisterCurrentBomb();

            StartCoroutine(IESpawnBombAfterDelay());
    }

        private void OnStartHolding(HoldableItem holdable, Holder holder)
        {
            UnregisterCurrentBomb();

            StartCoroutine(IESpawnBombAfterDelay());
        }

        IEnumerator IESpawnBombAfterDelay()
        {
            yield return new WaitForSeconds(_delay);

            SpawnBomb();
        }


    }
}