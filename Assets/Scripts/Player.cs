
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DungeonGame
{
    public class Player : MonoBehaviour, IExplodable
    {
        [SerializeField] private float _movementSpeed = 1.0f;

        [SerializeField] private List<AudioClip> _footstepSounds = default;

        [SerializeField] private float _stunTime = 0.5f;

        private Vector2 _currentInput = Vector2.zero;

        private Animator _animator = default;
        //private NavMeshAgent _navMeshAgent = default;
        private CharacterController _characterController = default;

        private Vector3 _velocity = Vector3.zero;

        private Vector3 _facingDireciton = Vector3.left;
        public Vector3 facingDirection => _facingDireciton;

        public bool isStunned { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            //_navMeshAgent = GetComponent<NavMeshAgent>();
            _characterController = GetComponent<CharacterController>();


            GetComponent<PlayerInput>().onActionTriggered += HandleAction;
            
        }


        // Update is called once per frame
        void LateUpdate()
        {
            if (!isStunned)
            {
                _animator.SetFloat("x_facing", facingDirection.x);
                _animator.SetFloat("y_facing", facingDirection.z);
                _animator.SetFloat("speed", _currentInput.magnitude);

                //_navMeshAgent.destination = transform.position;
                //_navMeshAgent.updatePosition = false;
                //_navMeshAgent.updateRotation = false;

                _velocity = new Vector3(_currentInput.x, _velocity.y, _currentInput.y);

                if (_characterController.isGrounded)
                {
                    _velocity.y = 0.0f;
                }
                else
                {
                    _velocity += Physics.gravity * Time.deltaTime;
                }


                _characterController.Move(_velocity * _movementSpeed * Time.deltaTime);
                //_navMeshAgent.velocity = _characterController.velocity;
            }
        }


        private void HandleAction(InputAction.CallbackContext context)
        {
            if(context.action.name.Equals("locomotion", System.StringComparison.OrdinalIgnoreCase))
            {
                _currentInput = context.action.ReadValue<Vector2>();

                if(_currentInput.magnitude > 0.01f)
                {
                    _facingDireciton = new Vector3(_currentInput.x, 0.0f, _currentInput.y).normalized;
                }

            }
        }

        public void PlayFootstepSound()
        {
            GetComponent<AudioSource>().PlayOneShot(_footstepSounds[Random.Range(0, _footstepSounds.Count)]);
        }

        public void DoStun(bool kill = false)
        {
            if(!isStunned)
            {
                StartCoroutine(IEDoStun(kill));
            }
        }

        private IEnumerator IEDoStun(bool kill)
        {
            isStunned = true;
            _animator.SetTrigger("do_stunned");

            Holder holder = GetComponent<Holder>();
            holder.DropCurrentHoldable();


            yield return new WaitForSeconds(_stunTime);

            if (kill)
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
            else
            {

                _animator.SetTrigger("do_recover");

                isStunned = false;
            }
        }

        public void Explode(Bomb bomb)
        {
            Holder holder = GetComponent<Holder>();
            holder.DropCurrentHoldable();

            DoStun();
        }


    }
}