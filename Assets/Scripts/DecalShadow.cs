using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
[ExecuteAlways]
public class DecalShadow : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private DecalProjector _decal = default;
    

    // Start is called before the first frame update
    void Start()
    {
        _decal = GetComponent<DecalProjector>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, _decal.size.z, _layerMask.value, QueryTriggerInteraction.Ignore))
        {
            float distance = Mathf.Clamp01(Vector3.Distance(transform.position, hitInfo.point) / _decal.size.z);

            _decal.fadeFactor = 1.0f - (distance * distance * distance);

        }
        else
        {
            _decal.fadeFactor = 0.0f;
        }
    }
}
