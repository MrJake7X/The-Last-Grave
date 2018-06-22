using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCinematic2 : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.GetComponent<PlayerAnimatorManager>().StartCinematic2();
            Destroy(gameObject);
        }
    }
}