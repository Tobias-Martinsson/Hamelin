using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneController : MonoBehaviour
{

    public GameObject killZonePrefab;
    public Transform killZoneSpawnLcation;
    private bool killZone = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !killZone)
        {
            SpawnKillZone();
        }

        if (Input.GetKeyDown(KeyCode.P) && killZone)
        {
            DestroyKillZone();
        }
    }

    private void SpawnKillZone()
    {
        Debug.Log("Create Killzone");
        Instantiate(killZonePrefab, killZoneSpawnLcation);
        killZone = true;
        //Instantiate(killZonePrefab, new Vector3(0, 0, 0), Quaternion.identity, killZoneSpawnLcation);
    }
    private void DestroyKillZone()
    {
        Debug.Log("Destroy Killzone");
        foreach (Transform child in killZoneSpawnLcation.transform)
        {
            Destroy(child.gameObject);
        }
        killZone = false;
        //Destroy(hook.gameObject);
    }
}
