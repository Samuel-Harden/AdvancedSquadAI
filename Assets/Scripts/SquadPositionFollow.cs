using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SquadPositionFollow : MonoBehaviour
{
    public GetTransform target;

    [SerializeField] float distance_to_target_min = 2.5f;
    [SerializeField] float distance_to_target_max = 10.0f;
    [SerializeField] float distance_stop_value = 0.2f;

    UnityEngine.AI.NavMeshAgent agent;

    private Vector3 target_pos;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        target_pos = target.ReturnTransform().position;

        transform.LookAt(target_pos);

        if (Vector3.Distance(transform.position, target_pos) > distance_to_target_min)
        {
            agent.SetDestination(Vector3.Lerp(transform.position, target_pos, distance_stop_value));
        }

        if (Vector3.Distance(transform.position, target_pos) > distance_to_target_max)
        {
            transform.position = Vector3.Lerp(transform.position, target_pos, 0.75f);
        }
    }
}
