using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour
{
    private Transform formation_pos;
    private Vector3 target_pos;
    private GameObject target_obj;
    private UnityEngine.AI.NavMeshAgent agent;
    private List<GameObject> threats;
    private GameObject current_threat;
    private Weapon weapon;
    private Animator animator;

    private float health;

    private bool form_up;
    private bool in_cover;
    private bool moving_to_cover;
    private bool moving_to_position;
    private bool distance_check;

    // Use this for initialization
    void Start()
    {
        agent      = GetComponent<NavMeshAgent>();
        weapon     = GetComponentInChildren<Weapon>();
        animator   = GetComponent<Animator>();
        threats    = new List<GameObject>();
        target_pos = transform.position;

        health = 100;

        distance_check = false;

        //animator.SetBool("in_cover", false);
        in_cover = true;

    }

    // Update is called once per frame
    void Update()
    {
        FollowingOrders();

        // Placeholder for the moment!
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }



    private void FollowingOrders()
    {
        if (form_up == true)
        {
            Following();
        }

        if (form_up == false)
        {
            Moving();
        }

        if (threats.Count != 0)
        {
            EngageTarget();
        }
    }



    private void Following()
    {
        if (Vector3.Distance(transform.position, target_obj.transform.position) > 0.25f)
        {
            if(target_obj != null)
            agent.SetDestination(target_obj.transform.position);
        }
    }



    private void Moving()
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
    }



    public void MoveOrder(Vector3 _pos)
    {
        target_pos = _pos;
        moving_to_position = true;

        form_up = false;
        moving_to_cover = false;
        in_cover = false;
        distance_check = false;

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



    private void EngageTarget()
    {
        if (current_threat != null)
        {
            transform.LookAt(new Vector3(current_threat.transform.position.x,
                transform.position.y, current_threat.transform.position.z));
            weapon.FireWeapon();
        }
    }


    // For Movement not enemy
    public void SetTarget(GameObject _target)
    {
        target_obj = _target;

        form_up = true;
    }



    public void UpdateThreats(List<GameObject> _threats)
    {
        threats = _threats;
        float distance;

        if (current_threat == null)
        {
            current_threat = _threats[0];
        }

        distance = Vector3.Distance(transform.position, current_threat.transform.position);

        RaycastHit hit;

        for (int i = 0; i < threats.Count; i++)
        {
            if (Vector3.Distance(transform.position, threats[i].transform.position) < distance)
            {
                Physics.Linecast(transform.position, threats[i].transform.position, out hit);

                if (hit.collider.CompareTag("Enemy"))
                {
                    current_threat = threats[i];
                }
            }
        }

        weapon.SetTarget(current_threat);
    }



    public void DamageHealth(float _damage)
    {
        health -= _damage;
    }
}
