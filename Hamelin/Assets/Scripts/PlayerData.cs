using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int health;
    public float[] position;
    public List<EnemySaveData> enemySaveData = new List<EnemySaveData>();


    public PlayerData(PlayerController3D player)
    {

        enemySaveData = AllAgents.convertToSaveData();
        Debug.Log(health);
        Debug.Log("Current health: " + player.health);
        health = player.health;
        position = new float[3];

        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }

}
