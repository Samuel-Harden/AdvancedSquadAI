using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIMovement : MonoBehaviour
{
    public Transform player;
    public Vector3 target;
    UnityEngine.AI.NavMeshAgent agent;

    private bool follow_player;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        target = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(follow_player == true)
        {
            target = player.position;
        }
        agent.SetDestination(target);
	}


    public void MoveOrder(Vector3 _pos)
    {
        target = _pos;
        follow_player = false;
    }



    public void FollowPlayer()
    {
        follow_player = true;
    }
}
