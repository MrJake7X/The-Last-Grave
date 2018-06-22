using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoPisada : MonoBehaviour {

    private SoundPlayer sound;

	void Start ()
    {
        sound = GetComponent<SoundPlayer>();
	}
    
    public void Pisar()
    {
        sound.Play(0, 1);
    }
}