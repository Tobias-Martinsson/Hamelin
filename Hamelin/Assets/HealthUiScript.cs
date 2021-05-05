using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUiScript : MonoBehaviour
{
    public GameObject player;
    private Text healthText;
    int playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponent<PlayerController3D>().health;
        healthText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = player.GetComponent<PlayerController3D>().health;
        healthText.text = "Health: " + playerHealth.ToString();
    }
}
