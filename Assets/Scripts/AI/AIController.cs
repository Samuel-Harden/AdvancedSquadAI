using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour
{
    private Transform formation_pos;
    private Vector3 target_pos;
    private GameObject target_obj;
    UnityEngine.AI.NavMeshAgent agent;

    private List<GameObject> threats;
    public GameObject current_threat;

    private Animator animator;

    private float health;

    private bool form_up;
    private bool in_corner_cover;
    private bool in_cover;
    private bool moving_to_cover;
    private bool moving_to_position;
    private bool distance_check;
    private bool corner_check;

    public Weapon weapon;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        threats = new List<GameObject>();
        target_pos = transform.position;

        weapon = GetComponentInChildren<Weapon>();

        health = 100;

        distance_check = false;
        corner_check = false;

        //animator.SetBool("in_corner_cover", false);
        in_corner_cover = false;

        //animator.SetBool("in_cover", false);
        in_cover = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (form_up == true)
        {
            Following();
        }

        if (form_up == false)
        {
            Moving();
        }

        // If Iam not moving and am on a corner
        if (moving_to_cover == false && moving_to_position == false && in_corner_cover == true)
        {
            if(corner_check == false)
            {
                CornerOverWatch();
            }
        }

        if (health < 0)
        {
            Destroy(gameObject, 0.2f);
        }
    }



    void Following()
    {
        if (Vector3.Distance(transform.position, target_obj.transform.position) > 0.25f)
        {
            if(target_obj != null)
            agent.SetDestination(target_obj.transform.position);
        }
    }



    void Moving()
    {
        if (Vector3.Distance(transform.position, target_pos) > 0.25f)
        {
            agent.SetDestination(target_pos);
        }

        // Bool used so we only do the distance related stuff once!
        if (distance_check == false)
        {
            if (Vector3.Distance(transform.position, target_pos) < 0.25f)
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
                    //Debug.Log("Hit Something " + directions[i].direction);
                    break;
                }
            }
        }

        corner_check = true;
    }


    public void CoverOrder(Vector3 _pos)
    {
        target_pos = _pos;
        form_up = false;

        target_pos.y = transform.position.y;
        transform.LookAt(target_pos);

        moving_to_cover = true;
        moving_to_position = false;
        in_cover = false;

        distance_check = false;
        corner_check = false;
    }



    public void MoveOrder(Vector3 _pos)
    {
        target_pos = _pos;
        moving_to_position = true;

        form_up = false;
        moving_to_cover = false;
        in_cover = false;
        distance_check = false;
        corner_check = false;

        target_pos.y = transform.position.y;
        transform.LookAt(target_pos);
    }


    // PROB NEED TO DELETE OBSOLETE
    public void FollowPlayer()
    {
        target_pos = formation_pos.position;
        target_pos.y = transform.position.y;
        transform.LookAt(target_pos);

        form_up = true;
    }



    void EngageTarget()
    {
        Debug.Log("I should Be Shooting");
    }



    public void SetTarget(GameObject _target)
    {
        target_obj = _target;

        form_up = true;
    }



    public void UpdateThreats(List<GameObject> _threats)
    {
        threats = _threats;
        float distance;

        if(current_threat != null)
        {
            distance = Vector3.Distance(transform.position, current_threat.transform.position);
        }
        else
        {
            distance = 25;
        }

        for (int i = 0; i < threats.Count; i++)
        {
            if (Vector3.Distance(transform.position, threats[i].transform.position) < distance)
            {
                current_threat = threats[i];
            }
        }

        weapon.SetTarget(current_threat);
    }



    public void DamageHealth(float _damage)
    {
        health -= _damage;
    }



    public void InCornerCover(bool _in_corner_cover)
    {
        in_corner_cover = _in_corner_cover;
    }
}
