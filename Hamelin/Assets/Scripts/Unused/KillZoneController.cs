using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Author: Henrik Rud�n


public class KillZoneController : MonoBehaviour
{

    public GameObject killZonePrefab;
    public GameObject clouds;
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
        if (playerController.getUpOnRoof() && !killZone)
        {
            SpawnKillZone();
        }

        if (playerController.getUpOnRoof() && playerController.climbing && killZone)
        {
            DestroyKillZone();
        }
    }

    private void SpawnKillZone()
    {
        Debug.Log("Create Killzone");
        Instantiate(killZonePrefab, killZoneSpawnLcation);
        //turns on clouds / My
        clouds.SetActive(true);
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
        //turns of clouds / My
        clouds.SetActive(false);
        killZone = false;
        //Destroy(hook.gameObject);
    }
}
