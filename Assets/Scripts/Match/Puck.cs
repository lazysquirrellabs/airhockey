using AirHockey.Utils;
using UnityEngine;

namespace AirHockey.Match
{
    public class Puck : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private AudioSource[] _audioSources;
        [SerializeField] private AudioClip[] _normalHitClips;
        [SerializeField] private AudioClip[] _hardHitClips;
        [SerializeField, Range(0,100)] private float _hardHitTreshold;

        #endregion

        #region Fields

        private int _lastAudioSourceIndex;

        #endregion

        #region Public

        public void Regroup(Vector2 position)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.position = position;
            _rigidbody2D.angularVelocity = 0f;
            _rigidbody2D.rotation = 0f;
        }

        public void StopMoving()
        {
            _rigidbody2D.isKinematic = true;
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.angularVelocity = 0f;
        }

        #endregion

        #region Event handlers

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_lastAudioSourceIndex == _audioSources.Length)
                _lastAudioSourceIndex = 0;

            var hardHit = other.relativeVelocity.magnitude >= _hardHitTreshold;
            var hitClip = hardHit ? _hardHitClips.Random() : _normalHitClips.Random();
            
            var audioSource = _audioSources[_lastAudioSourceIndex];
            audioSource.PlayOneShot(hitClip);
        }

        #endregion
    }
}