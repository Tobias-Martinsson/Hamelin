using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneController : MonoBehaviour
{

    public GameObject killZonePrefab;
    public Transform killZoneSpawnLcation;
    private bool killZone = false;

    public GameObject player;
    private PlayerController3D playerController;


    void Start()
    {
        playerController = player.GetComponent<PlayerController3D>();
    }

    void Update()
    {
        if (playerController.upOnRoof && !killZone)
        {
            SpawnKillZone();
        }

        if (playerController.upOnRoof && playerController.allowClimb && killZone)
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
