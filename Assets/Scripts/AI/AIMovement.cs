using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class AIMovement : MonoBehaviour
{
    public Transform formation_pos;
    public Vector3 target;
    UnityEngine.AI.NavMeshAgent agent;

    private Animator animator;

    private bool form_up;

    private bool in_corner_cover;
    private bool in_cover;
    private bool moving_to_cover;
    private bool moving_to_position;

    private bool distance_check;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = this.gameObject.transform.position;

        distance_check = false;

        //animator.SetBool("in_corner_cover", false);
        in_corner_cover = false;

        //animator.SetBool("in_cover", false);
        in_cover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (form_up == true)
        {
            target = formation_pos.position;
        }

        if (Vector3.Distance(this.gameObject.transform.position, target) > 0.75f)
        {
            agent.SetDestination(target);
        }

        // Bool used so we only do the distance related stuff once!
        if (distance_check == false)
        {
            if (Vector3.Distance(this.gameObject.transform.position, target) < 0.75f)
            {
                if (moving_to_cover == true)
                {
                    moving_to_cover = false;

                    in_cover = true;
                }

                if (moving_to_position == true)
                {
                    moving_to_position = false;
                }

                distance_check = true;
            }
        }

        if (moving_to_cover == false && moving_to_position == false && in_corner_cover)
        {
            Debug.Log("I should be peeking around the corner!");
        }
    }


    public void CoverOrder(Vector3 _pos)
    {
        Debug.Log("Cover Order");

        target = _pos;
        form_up = false;

        target.y = transform.position.y;
        transform.LookAt(target);

        moving_to_cover = true;
        moving_to_position = false;
        in_cover = false;

        distance_check = false;
    }



    public void MoveOrder(Vector3 _pos)
    {
        Debug.Log("Move Order");

        target = _pos;
        form_up = false;

        target.y = transform.position.y;
        transform.LookAt(target);

        moving_to_cover = false;
        moving_to_position = true;
        in_cover = false;

        distance_check = false;
    }



    public void FollowPlayer()
    {
        target = formation_pos.position;
        target.y = transform.position.y;
        transform.LookAt(target);

        form_up = true;
    }



    public void InCornerCover(bool _in_corner_cover)
    {
        in_corner_cover = _in_corner_cover;
    }
}