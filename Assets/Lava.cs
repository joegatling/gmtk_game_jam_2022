using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGame
{
    public class Lava : MonoBehaviour
    {
        public Player _player;

        public AnimationCurve _speedCurve;

        public float distanceToPlayer = 1000;


        private float _time;

        public AudioSource _lavaAudioSource;
        public AudioSource _musicAudioSource;


        // Start is called before the first frame update
        void Start()
        {
            _time = 0.0f;
            //_audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            _time += Time.deltaTime;
            //transform.LookAt(_player.transform);

            var q = Quaternion.LookRotation(_player.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 1.0f * Time.deltaTime);

            transform.Translate(Vector3.forward * _speedCurve.Evaluate(_time) * Time.deltaTime, Space.Self);

            distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);

            if(_lavaAudioSource)
            {
                _lavaAudioSource.volume = 1.0f -  Mathf.Clamp01(distanceToPlayer / 15.0f);
            }
            if (_musicAudioSource)
            {
                _musicAudioSource.volume = 1.0f - Mathf.Clamp01(distanceToPlayer / 10.0f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Debug.Log("Kill Player");
                enabled = false;

                var player = other.gameObject.GetComponent<Player>();
                player.DoStun(true);
            }
        }
    }
}
