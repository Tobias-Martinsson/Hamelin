using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatDrool : MonoBehaviour
{

    private ParticleSystem droolDrop;
    // Start is called before the first frame update
    void Start()
    {
        droolDrop = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
     
        
    }

    private void OnParticleCollision(GameObject other)
    {
        droolDrop.TriggerSubEmitter(0);
    }
}
