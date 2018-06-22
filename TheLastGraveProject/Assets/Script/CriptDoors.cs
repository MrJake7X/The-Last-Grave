using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriptDoors : MonoBehaviour
{
    public ParticleSystem bats;
    private Animator anim;
    private SoundPlayer sound;
    private bool soloUnaVez;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sound = GetComponentInChildren<SoundPlayer>();
        soloUnaVez = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !soloUnaVez)
        {
            bats.Play();
            anim.Play("Puerta");
            sound.Play(0, 1);
            sound.Play(1, 1);

            soloUnaVez = true;
        }
    }
}