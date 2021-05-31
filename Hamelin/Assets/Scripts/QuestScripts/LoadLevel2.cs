using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Main Author: Tobias Martinsson
public class LoadLevel2 : MonoBehaviour
{
    public BoxCollider col;
    private void Awake()
    {
       col = GetComponent<BoxCollider>();
       gameObject.SetActive(false);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit");
            PlayerPrefs.SetInt("loaded", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
