using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPainUI : MonoBehaviour
{
    public GameObject painUI;

    public void ShowPain()
    {
        painUI.SetActive(true);
    }

    public void HidePain()
    {
        painUI.SetActive(false);
    }
}
