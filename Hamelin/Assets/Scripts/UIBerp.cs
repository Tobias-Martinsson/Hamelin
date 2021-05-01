using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBerp : MonoBehaviour
{
    private float time;

    void OnEnable()
    {
        time = 0.0f;
    }

    void Update()
    {
        if(time <= 1)
        {
            transform.localScale = Vector3.one * Mathfx.Berp(0f, 1f, time);
            time += Time.deltaTime;
        }
    }
}
