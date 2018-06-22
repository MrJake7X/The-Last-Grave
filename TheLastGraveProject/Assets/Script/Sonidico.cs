using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonidico : MonoBehaviour
{

    private SoundPlayer sound;
    private float sonidico = 4;
    private float tsonidico = 0;
    // Update is called once per frame

    private void Start()
    {
        sound = GetComponent<SoundPlayer>();
    }
    void Update ()
    {
		if(tsonidico >= sonidico)
        {
            sound.Play(Random.Range(1, 4), 0.2f);
            tsonidico = 0;
        }
        else
        {
            tsonidico += Time.deltaTime;
        }
	}
}
