using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class AIMovement : MonoBehaviour
{
    public Transform formation_pos;
    public Vector3 target;
    UnityEngine.AI.NavMeshAgent agent;

    private bool form_up;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (form_up == true)
        {
            target = formation_pos.position;
        }

        if (Vector3.Distance(this.gameObject.transform.position, target) > 0.5f)
        {
            agent.SetDestination(target);
        }
    }


    public void MoveOrder(Vector3 _pos)
    {
        target = _pos;
        form_up = false;

        target.y = transform.position.y;
        transform.LookAt(target);
    }



    public void FollowPlayer()
    {
        target = formation_pos.position;
        target.y = transform.position.y;
        transform.LookAt(target);

        form_up = true;
    }
}