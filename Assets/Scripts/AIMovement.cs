using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIMovement : MonoBehaviour
{
    public Vector3 target;
    UnityEngine.AI.NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        target = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        agent.SetDestination(target);
	}


    public void MoveOrder(Vector3 _pos)
    {
        target = _pos;
    }
}
