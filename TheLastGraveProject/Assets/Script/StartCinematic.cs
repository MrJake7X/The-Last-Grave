using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCinematic : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.GetComponent<PlayerAnimatorManager>().StartCinematic();
            Destroy(gameObject);
        }
    }
}