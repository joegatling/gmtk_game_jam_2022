using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace DungeonGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 1.0f;

        private Vector2 _currentInput = Vector2.zero;

        private Animator _animator = default;
        private NavMeshAgent _navMeshAgent = default;
        private CharacterController _characterController = default;

        private Vector3 _velocity = Vector3.zero;
        

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _characterController = GetComponent<CharacterController>();


            Debug.Log(_characterController.isTrigger);

            GetComponent<PlayerInput>().onActionTriggered += HandleAction;
            
        }


        // Update is called once per frame
        void LateUpdate()
        {
            _animator.SetFloat("x_facing", _currentInput.x);
            _animator.SetFloat("y_facing", _currentInput.y);
            _animator.SetFloat("speed", _currentInput.magnitude);



            _navMeshAgent.destination = transform.position;
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = false;

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
            _navMeshAgent.velocity = _characterController.velocity;
        }


        private void HandleAction(InputAction.CallbackContext context)
        {
            if(context.action.name.Equals("locomotion", StringComparison.OrdinalIgnoreCase))
            {
                _currentInput = context.action.ReadValue<Vector2>();
            }
        }
    }
}