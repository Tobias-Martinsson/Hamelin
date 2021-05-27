using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public BoxCollider col;
    public GameObject clouds;
    private bool killZone = false;
    public GameObject player;
    private PlayerController3D playerController;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        playerController = player.GetComponent<PlayerController3D>();
    }

    private void Start()
    {
        col.enabled = !col.enabled;
    }

    void Update()
    {
        if (playerController.getUpOnRoof() && !killZone)
        {
            SpawnKillZone();

        }

        if (!playerController.getUpOnRoof() && killZone)
        {
            DestroyKillZone();

        }
    }

    private void SpawnKillZone()
    {
        Debug.Log("Spawn Killzone");
        col.enabled = !col.enabled;
        //turns on clouds / My
        clouds.SetActive(true);
        killZone = true;
    }
    private void DestroyKillZone()
    {
        Debug.Log("Destroy Killzone");
        col.enabled = !col.enabled;
        //turns of clouds / My
        clouds.SetActive(false);
        killZone = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit");
            player.GetComponent<PlayerController3D>().KillZoneCollision();
        }
    }
}
