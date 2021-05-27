using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySaveData 
{
    public float[] position;
    public float[] rotation;
    public string name;

    public EnemySaveData(SomeAgent agent)
    {
        name = agent.name;
        
        position = new float[3];
        position[0] = agent.transform.position.x;
        position[1] = agent.transform.position.y;
        position[2] = agent.transform.position.z;

        rotation = new float[4];
        rotation[0] = agent.transform.rotation.w;
        rotation[1] = agent.transform.rotation.x;
        rotation[2] = agent.transform.rotation.y;
        rotation[3] = agent.transform.rotation.z;

    }
}
