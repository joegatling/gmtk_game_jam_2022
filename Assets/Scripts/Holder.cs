using System;
using System.Collections;
using System.Collections.Generic;
using DungeonGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonGame
{
    public class Holder : MonoBehaviour
    {
        [SerializeField] private bool _autoPickUpItems = true;

        [SerializeField] private Transform _holdingParent = default;

        HoldableItem _currentHoldable = null;

        public bool isHoldingItem => _currentHoldable != null;
        HoldableItem currentHoldable => _currentHoldable;

        public Transform holdingParent => _holdingParent;

        Player _player = default;
        public Player player => _player;

        // Start is called before the first frame update
        void Start()
        {
            _player = gameObject.GetComponentInParent<Player>();
            GetComponent<PlayerInput>().onActionTriggered += HandleAction;

        }

        private void HandleAction(InputAction.CallbackContext context)
        {

            if (context.action.name.Equals("use", StringComparison.OrdinalIgnoreCase))
            {
                if(context.action.phase == InputActionPhase.Performed)
                {
                    if(isHoldingItem)
                    {
                        currentHoldable.Use(this);
                    }
                }
            }
        }


        // Update is called once per frame
        void Update()
        {
            

        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var holdableItem = hit.gameObject.GetComponent<HoldableItem>();

            if (holdableItem != null && _autoPickUpItems && holdableItem.canPickUp)
            {
                if (!isHoldingItem)
                {
                    StartHolding(holdableItem);
                }
            }
        }

        public void StartHolding(HoldableItem holdable)
        {
            if (isHoldingItem)
            {
                DropCurrentHoldable();
            }

            _currentHoldable = holdable;
            _currentHoldable.StartHolding(this);



            GetComponent<Animator>().SetBool("holding", true);

        }

        public void DropCurrentHoldable()
        {
            if (isHoldingItem)
            {
                GetComponent<Animator>().SetBool("holding", false);
                _currentHoldable.StopHolding();
                _currentHoldable = null;

            }
        }

        public void ThrowCurrentHoldable(Vector3 direction)
        {
            if (isHoldingItem)
            {
                StartCoroutine(IEThrowCurrentHoldable(direction));

            }
        }

        public IEnumerator IEThrowCurrentHoldable(Vector3 direction)
        {
            var throwHoldable = _currentHoldable;

            Animator animator = GetComponent<Animator>();
            animator.SetBool("holding", false);
            animator.SetTrigger("do_throw");

            _currentHoldable.StopHolding();
            _currentHoldable = null;

            throwHoldable.transform.position = _holdingParent.transform.position;

            Rigidbody r = throwHoldable.GetComponent<Rigidbody>();
            r.WakeUp();

            yield return null;

            
            r.AddForce(direction, ForceMode.VelocityChange);

            Debug.DrawLine(throwHoldable.transform.position, throwHoldable.transform.position + direction, Color.green, 10.0f);


        }

    }
}
