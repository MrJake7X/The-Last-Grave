using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour {

    public float tiempo;
    // Use this for initialization
    private void Update()
    {
        if (tiempo >= 2f)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            tiempo += Time.deltaTime;
        }
    }


}
