using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    public DynamicFootsteps dynFoot;

    private string colliderType;

    void Start()
    {
        dynFoot = gameObject.GetComponent<DynamicFootsteps>();
    }

    void OnCollisionEnter(Collision other)
    {
        SurfaceColliderType act = other.gameObject.GetComponent<Collider>().gameObject.GetComponent<SurfaceColliderType>();

        if (act)
        {
            colliderType = act.GetTerrainType();
            dynFoot.PlayDynamicFootstep(colliderType);
        }
    }
}
