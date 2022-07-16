using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;

public class HoldableItem : MonoBehaviour
{
    [HideInInspector] [SerializeField] private DecalProjector _shadowProjector = default;

    ParentConstraint _holdingConstraint = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp(Holder holder)
    {
        if (_shadowProjector != null)
        {
            _shadowProjector.gameObject.SetActive(false);
        }

        _holdingConstraint = gameObject.AddComponent<ParentConstraint>();
        //_holdingConstraint.AddSource()

    }

    public void PutDown()
    {
        if (_shadowProjector != null)
        {
            _shadowProjector.gameObject.SetActive(true);
        }
    }

    private void OnValidate()
    {
        _shadowProjector = GetComponentInChildren<DecalProjector>();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    var holdableItem = collision.gameObject.GetComponent<HoldableItem>();

    //    if (holdableItem != null)
    //    {
    //        Debug.Log("Holdable");
    //    }
    //}
}
