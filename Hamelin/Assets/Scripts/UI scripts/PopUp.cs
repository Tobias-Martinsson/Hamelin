using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public GameObject popup;
    public Text ptext;
    public bool knowladder;
    
    // Start is called before the first frame update
    void Start()
    {
        DeActivatePopup();
        knowladder = false;
    }

    // Update is called once per frame
    void Update()
    {
         if(knowladder){
             if(Input.GetKeyDown(KeyCode.E)){
                DeActivatePopup();
                knowladder = false;
             }
         }
    }

     private void OnTriggerEnter(Collider collider)
     {
         if(this.gameObject.tag == "LadderTrigger"){
             
            ptext = ptext.GetComponent<Text>();
             ptext.text = "Press E to climb";
             ActivatePopup();
             knowladder = true;


         }
     }

     private void ActivatePopup(){
         if(!popup.activeInHierarchy){
             popup.SetActive(true);
         } else{
             DeActivatePopup();
         }
         
     }

     private void DeActivatePopup(){
         if(popup.activeInHierarchy){
             popup.SetActive(false);
         } 
         
     }



}
