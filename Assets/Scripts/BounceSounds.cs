using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSounds : MonoBehaviour
{
    [SerializeField] float _minVelocity = 0.1f;
    [SerializeField] float _minVolume = 0.1f;
    [SerializeField] float _velocityVolumeMultiplier = 0.1f;
    [SerializeField] private List<AudioClip> _sounds;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] LayerMask _layerMask;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > _minVelocity)
        {
            if (_layerMask == (_layerMask | 1 << collision.gameObject.layer))
            {
                float velocity = collision.relativeVelocity.magnitude;
                float volume = Mathf.Clamp01((velocity - _minVelocity) * _velocityVolumeMultiplier);

                _audioSource.PlayOneShot(_sounds[Random.Range(0, _sounds.Count)], volume);
            }
        }
    }
}
