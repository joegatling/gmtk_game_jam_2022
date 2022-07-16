using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    [SerializeField] private bool _autoPickUpItems = true;

    [SerializeField] private Transform _holdingParent = default;

    HoldableItem _currentHoldable = null;

    public bool isHoldingItem => _currentHoldable != null;
    HoldableItem currentHoldable => _currentHoldable;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var holdableItem = hit.gameObject.GetComponent<HoldableItem>();

        if (holdableItem != null)
        {
            if(!isHoldingItem)
            {

            }
        }
    }

    public void StartHolding(HoldableItem holdable)
    {
        if(isHoldingItem)
        {
            DropCurrentHoldable();
        }

        _currentHoldable = holdable;
        
    }

    public void DropCurrentHoldable()
    {
        
    }

    public void ThrowCurrentHoldable(Vector3 direction)
    { }
}
