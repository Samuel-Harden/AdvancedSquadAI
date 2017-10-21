using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NavMeshFollow : MonoBehaviour
{
    public float follow_distance = 0.5f;
    private Transform target;
    private UnityEngine.AI.NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > follow_distance)
            {
                agent.SetDestination(target.position);
            }

            if (Vector3.Distance(transform.position, target.position) > 5.0f)
            {
                transform.position = target.position;
            }
        }
	}



    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
