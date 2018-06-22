using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidosCinematicaFinal : MonoBehaviour {

    private SoundPlayer sounds;

	void Start ()
    {
        sounds = GetComponent<SoundPlayer>();
	}
	
    public void SonidoPuerta()
    {
        sounds.Play(0, 0.5f);
    }

    public void SonidoTension()
    {
        sounds.Play(1, 1);
    }
}