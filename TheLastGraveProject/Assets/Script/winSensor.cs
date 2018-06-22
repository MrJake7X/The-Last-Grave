using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winSensor : MonoBehaviour
{
    public UIControler hud;
    // Use this for initialization

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            hud.OpenWinPanel();
        }

    }
}
