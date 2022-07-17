using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CameraTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera _camera = default;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _camera.MoveToTopOfPrioritySubqueue();
        }
    }

    private void OnValidate()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;

        if(_camera == null)
        {
            _camera = GetComponentInParent<CinemachineVirtualCamera>();
        }
    }
}
