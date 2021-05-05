using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Main Author: Andreas Scherman
public class s_EnemyScoreCounter : MonoBehaviour
{

    private Text scoreCounter;

    public SphereCollider collider;
    public BugNetController bugNet;

    // Start is called before the first frame update
    void Start()
    {
       scoreCounter = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreCounter.text = bugNet.Score.ToString();

    }

}
