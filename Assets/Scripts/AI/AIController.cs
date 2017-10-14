﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour
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
    private bool corner_check;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = this.gameObject.transform.position;

        distance_check = false;
        corner_check = false;

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

        Moving();

        // If Iam not moving and am on a corner
        if (moving_to_cover == false && moving_to_position == false && in_corner_cover == true)
        {
            if(corner_check == false)
            {
                CornerOverWatch();
            }
        }
    }



    void Moving()
    {
        if (Vector3.Distance(this.gameObject.transform.position, target) > 0.5f)
        {
            agent.SetDestination(target);
        }

        // Bool used so we only do the distance related stuff once!
        if (distance_check == false)
        {
            if (Vector3.Distance(this.gameObject.transform.position, target) < 0.5f)
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
    }



    void CornerOverWatch()
    {
        // Identify corner Side, Raycast front, back, left, right
        RaycastHit hit;
        Vector3 direction = Vector3.forward;

        List<Ray> directions = new List<Ray>();

        Ray ray = new Ray(transform.position, Vector3.forward);
        directions.Add(ray);
        ray     = new Ray(transform.position, -Vector3.forward);
        directions.Add(ray);
        ray     = new Ray(transform.position, Vector3.right);
        directions.Add(ray);
        ray     = new Ray(transform.position, -Vector3.right);
        directions.Add(ray);

        for (int i = 0; i < directions.Count; i++)
        {
            if (Physics.Raycast(directions[i], out hit, 1.0f))
            {
                // Only Actors have rigidbodies
                // If we have hit something without one, its cover!
                if (hit.rigidbody == null)
                {
                    Debug.Log("Hit Something " + directions[i].direction);
                    break;
                }
            }
        }

        corner_check = true;
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
        corner_check = false;
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
        corner_check = false;
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
