using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerAnimationEvents : MonoBehaviour
{
    private CrawlerBehaviour crawler;

	// Use this for initialization
	void Start ()
    {
        crawler = GetComponentInParent<CrawlerBehaviour>();
	}

    public void TimeToAttack()
    {
        crawler.Attack();
    }
}
