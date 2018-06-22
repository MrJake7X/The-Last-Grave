using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

    public Image panel;

    Color c = Color.black;

    void Start ()
    {

	}

	void Update ()
    {
        panel.color = c;

        c.a -= 0.5f * Time.deltaTime;
	}
}
