using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    public AudioClip[] clips;

    public void Play(int num, float vol)
    {
        GameObject obj = new GameObject();
        obj.transform.position = transform.position;
        obj.name = "Sonidico_" + clips[num].name;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = clips[num];
        source.volume = vol;
        source.spatialBlend = 1;
        source.Play();

        Destroy(obj, clips[num].length);
    }
}
