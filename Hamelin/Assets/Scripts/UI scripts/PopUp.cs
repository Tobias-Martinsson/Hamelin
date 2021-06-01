using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public GameObject popup;
    public Text ptext;
    
    // Start is called before the first frame update
    void Start()
    {
        DeActivatePopup();
    }

    // Update is called once per frame
    void Update()
    {
         
    }

     private void OnTriggerStay(Collider collider)
     {
        
         if(collider.gameObject.tag == "Player")
        {
            ptext = ptext.GetComponent<Text>();
            ptext.text = "Press E to climb";
            ActivatePopup();
        }
     }

    private void OnTriggerExit(Collider other)
    {
        DeActivatePopup();
    }

    private void ActivatePopup()
    {

        popup.SetActive(true);
    }

    private void DeActivatePopup()
    {
        if (popup.activeInHierarchy)
        {
            popup.SetActive(false);
        }

    }
}
