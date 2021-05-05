using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Author: Andreas Scherman
public class s_SpawnCar : MonoBehaviour
{
    private Vector3 startPos;
    private float startTimer;

    public float timer;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        //car = gameObject.GetComponent<GameObject>();
        startTimer = timer;
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            this.transform.Translate(Vector3.forward *speed);
        }
        else if (timer <= 0)
        {
            timer += startTimer;
            this.transform.position = startPos;
        }
        
    }
}
