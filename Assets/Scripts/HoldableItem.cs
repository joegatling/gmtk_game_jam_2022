using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;

namespace DungeonGame
{
    public class HoldableItem : MonoBehaviour
    {
        public System.Action<HoldableItem, Holder> onStartHolding = delegate { };
        public System.Action<HoldableItem, Holder> onUse = delegate { };
        public System.Action<HoldableItem> onStopHolding = delegate { };

        [HideInInspector][SerializeField] private DecalProjector _shadowProjector = default;

        IConstraint _holdingConstraint = null;

        List<Collider> _allColliders = default;

        public bool canPickUp { get; set; }
        public bool isBeingHeld { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            _allColliders = new List<Collider>(GetComponentsInChildren<Collider>());
            canPickUp = true;
        }


        // Update is called once per frame
        void Update()
        {

        }

        public void StartHolding(Holder holder)
        {
            if (_shadowProjector != null)
            {
                _shadowProjector.gameObject.SetActive(false);
            }

            _holdingConstraint = gameObject.AddComponent<PositionConstraint>();
            _holdingConstraint.weight = 0;
            _holdingConstraint.constraintActive = true;

            ConstraintSource cs = new ConstraintSource();
            cs.weight = 1;
            cs.sourceTransform = holder.holdingParent;

            _holdingConstraint.AddSource(cs);

            DOVirtual.Float(_holdingConstraint.weight, 1.0f, 0.2f, (x) => _holdingConstraint.weight = x).SetEase(Ease.InOutQuad);

            Rigidbody r = GetComponent<Rigidbody>();
            r.isKinematic = true;

            SetLayer("Ignore Player");

            isBeingHeld = true;
            onStartHolding(this, holder);
        }

        public void StopHolding()
        {
            if (_shadowProjector != null)
            {
                _shadowProjector.gameObject.SetActive(true);
            }

            _holdingConstraint.constraintActive = false;
            Destroy(_holdingConstraint as Component);
            _holdingConstraint = null;

            Rigidbody r = GetComponent<Rigidbody>();
            r.isKinematic = false;

            isBeingHeld = false;
            onStopHolding(this);
        }

        public void SetLayer(string layerName)
        {
            var layerNumber = LayerMask.NameToLayer(layerName);
            foreach (var c in _allColliders)
            {
                c.gameObject.layer = layerNumber;
            }
        }

        private void OnValidate()
        {
            _shadowProjector = GetComponentInChildren<DecalProjector>();
        }

        public void Use(Holder holder)
        {
            onUse(this, holder);
        }

    }
}