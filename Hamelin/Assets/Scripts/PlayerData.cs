using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int health;
    public float[] position;
    public List<EnemySaveData> enemySaveData = new List<EnemySaveData>();
    public int score;
    public bool onRoof;

    public PlayerData(PlayerController3D player)
    {
        score = player.GetComponentInChildren<BugNetController>().getScore();
        enemySaveData = AllAgents.convertToSaveData();
        onRoof = player.getUpOnRoof();
        health = player.health;
        position = new float[3];

        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }

}
