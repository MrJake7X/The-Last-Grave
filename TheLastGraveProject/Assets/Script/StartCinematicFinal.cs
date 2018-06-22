using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCinematicFinal : MonoBehaviour {

    public Animator anim;

    private void Start()
    {
        anim.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<PlayerAnimatorManager>().StartFinal();
            anim.enabled = true;
            Destroy(gameObject);
        }
    }
}
