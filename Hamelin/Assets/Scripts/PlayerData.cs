using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int health;
    public float[] position;
<<<<<<< HEAD
    public List<EnemySaveData> enemySaveData = new List<EnemySaveData>();
    public int score;
    public bool onRoof;

    public PlayerData(PlayerController3D player)
    {
<<<<<<< HEAD
        score = player.GetComponentInChildren<BugNetController>().getScore();
        enemySaveData = AllAgents.convertToSaveData();
        onRoof = player.getUpOnRoof();
=======
        enemySaveData = AllAgents.convertToSaveData();
>>>>>>> parent of aaec707 (Health UI Fix)
=======

    public PlayerData(PlayerController3D player)
    {
>>>>>>> parent of 2e5fbc7 (Merge branch 'SemiCompleteSaveSystem_Tobias' into main)
        health = player.health;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }

}
