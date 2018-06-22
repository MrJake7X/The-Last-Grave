using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreeperAnimationEvents : MonoBehaviour
{
    private CreeperBehaviour creeper;

	// Use this for initialization
	void Start ()
    {
        creeper = GetComponentInParent<CreeperBehaviour>();
	}

    public void EndAnimation()
    {
        creeper.Beam();
    }
}