using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text questText;
    public Text acceptText;
    public Text sQText;
    public Text quest1Text;
    public Text quest2Text;

    // Start is called before the first frame update
    void Start()
    {
        questText.text = "Hi, i need you to catch 10 pests, meet me at the bar";
        acceptText.text = "Press E to accept";
        sQText.text = "Catch 5 rats and 10 birds, then meet Josh in the bar";
        quest1Text.text = "5 / 10 rats";
        quest2Text.text = "5 / 10 rats";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
