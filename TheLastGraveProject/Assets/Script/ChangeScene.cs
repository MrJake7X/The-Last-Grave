using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour {

    public Image panel;

    Color c = Color.black;

    private bool change;

    public void Start()
    {
        c.a = 0;
    }

    private void Update()
    {
        panel.color = c;
        //Debug.Log(panel.color.a);
        if(change)
        {
            GoToCript();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            change = true;
        }
    }

    public void GoToCript()
    {
        if (c.a < 1)
        {
            c.a += 1 * Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene(2);
            //Debug.Log("CAMBIA");
        }
    }
}