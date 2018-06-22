using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AITest : MonoBehaviour
{
    public LayerMask mask;
    public float ditance = Mathf.Infinity;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if(Physics.Raycast(ray, out hit, ditance, mask))
            {
                //Debug.Log(hit.point);
                agent.SetDestination(hit.point);
            }
        }
	}
}
