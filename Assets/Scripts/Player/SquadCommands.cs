﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquadCommands : MonoBehaviour
{
    private Camera cam;
    private float distance;

    public float max_command_distance = 20.0f;
    public GameObject target_indicator;

    public GameObject position_test;

    private List<AIController> squad_one;
    private List<AIController> squad_two;

    public CoverManager cover_manager;
    public FormationManager formation_manager;

    public LayerMask cover_mask;

    private int squad_1 = 1;
    private int squad_2 = 2;

    private int obstacle_layer;

    private Vector3 prev_hit_location;


    // Use this for initialization
    void Start()
    {
        distance = 20.0f;
        obstacle_layer = 11;
        prev_hit_location = Vector3.zero;
        cam = GetComponent<Camera>();
        squad_one = new List<AIController>();
        squad_two = new List<AIController>();
    }



    // Update is called once per frame
    private void LateUpdate()
    {
        UserInput();
    }



    void UserInput()
    {
        if (Input.GetButtonDown("TeamFormUp"))
        {
            FollowCommand();
        }


        if (Input.GetButtonDown("TeamFormation"))
        {
            formation_manager.CycleFormation(squad_one.Count, squad_two.Count, 0);
        }

        if (Input.GetButtonDown("SquadOneFormation"))
        {
            formation_manager.CycleFormation(squad_one.Count, squad_two.Count, 1);
        }

        if (Input.GetButtonDown("SquadTwoFormation"))
        {
            formation_manager.CycleFormation(squad_one.Count, squad_two.Count, 2);
        }

        if (Input.GetButtonDown("SquadOneAction") || Input.GetButtonDown("SquadTwoAction") || Input.GetButtonDown("TeamAction"))
        {

            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // if the Ray hits something & is within range
            if (Physics.Raycast(ray, out hit, distance, obstacle_layer))
            {
                if (hit.collider.tag == "Ground")
                {
                    DistanceToCoverCheck(ref hit);
                }


                // Set TargetIndicator Hit Pos
                SetTargetIndicator(hit.point);

                Vector3 hit_pos = new Vector3(hit.point.x, 0.0f, hit.point.z);

                // Identify object hit
                if (hit.collider.tag == "Full Cover" || hit.collider.tag == "Low Cover")
                {
                    Debug.Log("cheese");
                    CoverCommands(hit_pos);
                }

                else if (hit.collider.tag == "Low Cover")
                {
                    // Low Cover Commands to go here!
                }

                else if (hit.collider.tag == "Ground")
                {
                    MoveCommands(hit_pos);
                }
            }
        }
    }


    // When the Raycast has hit a 'Cover' Type object (Tag)
    private void CoverCommands(Vector3 _hit)
    {
        if (Input.GetButtonDown("SquadOneAction"))
        {
            SquadCoverReset(squad_1);
            SquadGetCover(_hit, squad_1);
        }

        else if (Input.GetButtonDown("SquadTwoAction"))
        {
            SquadCoverReset(squad_2);
            SquadGetCover(_hit, squad_2);
        }

        else if (Input.GetButtonDown("TeamAction"))
        {
            SquadCoverReset(1);
            SquadCoverReset(2);

            float squad_one_avg_dist = 0;
            float squad_two_avg_dist = 0;

            for (int i = 0; i < squad_one.Count; i++)
            {
                squad_one_avg_dist += Vector3.Distance(squad_one[i].GetComponent<Transform>().position, _hit);
            }

            for (int i = 0; i < squad_two.Count; i++)
            {
                squad_two_avg_dist += Vector3.Distance(squad_two[i].GetComponent<Transform>().position, _hit);
            }

            // Check Which Squad Memebr is closest
            if (squad_one_avg_dist < squad_two_avg_dist)
                {
                // If Squadie One is closest...
                SquadGetCover(_hit, squad_1);
                StartCoroutine(SquadGetCoverDelay(_hit, squad_2));
            }

            else
            {
                // if Squadie Two is closest...
                SquadGetCover(_hit, squad_2);
                StartCoroutine(SquadGetCoverDelay(_hit, squad_1));
            }
        }
    }


    
    private void FollowCommand()
    {
        SquadCoverReset(squad_1);
        SquadCoverReset(squad_2);

        float squad_one_avg_dist = 0;
        float squad_two_avg_dist = 0;

        for (int i = 0; i < squad_one.Count; i++)
        {
            squad_one_avg_dist += Vector3.Distance(squad_one[i].GetComponent<Transform>().position, transform.position);
        }

        for (int i = 0; i < squad_two.Count; i++)
        {
            squad_two_avg_dist += Vector3.Distance(squad_two[i].GetComponent<Transform>().position, transform.position);
        }

        if (squad_one_avg_dist < squad_two_avg_dist)
        {
            SquadGetFormationPositions(squad_one.Count, squad_1);
            StartCoroutine(SquadGetFormationPositionsDelay(squad_two.Count, squad_2));
        }

        else
        {
            SquadGetFormationPositions(squad_two.Count, squad_2);
            StartCoroutine(SquadGetFormationPositionsDelay(squad_one.Count, squad_1));
        }
    }



    private void MoveCommands(Vector3 _hit)
    {
        if (Input.GetButtonDown("SquadOneAction"))
        {
            SquadCoverReset(squad_1);
            SquadMoveOrder(GenerateMovePositions(_hit, squad_two), squad_1);
        }

        else if (Input.GetButtonDown("SquadTwoAction"))
        {
            SquadCoverReset(squad_2);
            SquadMoveOrder(GenerateMovePositions(_hit, squad_two), squad_2);
        }

        else if (Input.GetButtonDown("TeamAction"))
        {
            SquadCoverReset(squad_1);
            SquadCoverReset(squad_2);

            float squad_one_avg_dist = 0;
            float squad_two_avg_dist = 0;

            for (int i = 0; i < squad_one.Count; i++)
            {
                squad_one_avg_dist += Vector3.Distance(squad_one[i].GetComponent<Transform>().position, _hit);
            }

            for (int i = 0; i < squad_two.Count; i++)
            {
                squad_two_avg_dist += Vector3.Distance(squad_two[i].GetComponent<Transform>().position, _hit);
            }

            if (squad_one_avg_dist < squad_two_avg_dist)
            {
                SquadMoveOrder(GenerateMovePositions(_hit, squad_two), squad_1);

                StartCoroutine(SquadMoveOrderDelay(GenerateMovePositions(_hit, squad_two), squad_2));
            }

            else
            {
                SquadMoveOrder(GenerateMovePositions(_hit, squad_two), squad_2);

                StartCoroutine(SquadMoveOrderDelay(GenerateMovePositions(_hit, squad_one), squad_1));
            }
        }
    }



    private void DistanceToCoverCheck(ref RaycastHit _hit)
    {
        float angle = 0;
        float distance = 3.2f;

        for (int i = 0; i < 8; i++)
        {
            float radians = angle * Mathf.Deg2Rad;

            float x = (_hit.point.x + distance * Mathf.Cos(radians));
            float z = (_hit.point.z + distance * Mathf.Sin(radians));

            Vector3 new_pos = new Vector3(x, 0.1f, z);

            RaycastHit hit;

            if (Physics.Linecast(_hit.point, new_pos, out hit, cover_mask))
            {
                if(hit.collider.tag == "Full Cover")
                {
                    _hit = hit;
                    return;
                }
            }

            //Debug.DrawLine(_hit.point, new_pos, Color.red, 60.0f);
            //Instantiate(position_test, new_pos, position_test.transform.rotation);

            angle += 45.0f;
        }
    }


    // Creates positions for squadies to move to if order to move to a location
    private List<Vector3> GenerateMovePositions(Vector3 _hit, List<AIController> _squad)
    {
        List<Vector3> positions = new List<Vector3>();

        float angle = 0;
        float distance = 2.0f;

        // IF the whole Team is rallying on this position,
        // we need 2x the number of points, so this creates an offset
        if(_hit == prev_hit_location)
        {
            angle = (360 / _squad.Count) / 2;
        }

        for (int i = 0; i < _squad.Count; i++)
        {
            float radians = angle * Mathf.Deg2Rad;

            float x = (_hit.x + distance * Mathf.Cos(radians));
            float z = (_hit.z + distance * Mathf.Sin(radians));

            Vector3 new_pos = new Vector3(x, 0.0f, z);

            // USED FOR TESTING // 
            //Instantiate(position_test, new_pos, position_test.transform.rotation);

            positions.Add(new_pos);

            angle += 360 / _squad.Count;
        }

        prev_hit_location = _hit;

        return positions;
    }



    private void SquadMoveOrder(List<Vector3> _positions, int _squad)
    {
        if (_squad == 1)
        {
            for (int i = 0; i < squad_one.Count; i++)
            {
                squad_one[i].MoveOrder(_positions[i]);
            }
        }

        if (_squad == 2)
        {
            for (int i = 0; i < squad_two.Count; i++)
            {
                squad_two[i].MoveOrder(_positions[i]);
            }
        }
    }


    // SAME FUNCTION AS ABOVE, JUST WITH A DELAY
    private IEnumerator SquadMoveOrderDelay(List<Vector3> _positions, int _squad)
    {
        yield return new WaitForSeconds(1);

        if (_squad == 1)
        {
            for (int i = 0; i < squad_one.Count; i++)
            {
                squad_one[i].MoveOrder(_positions[i]);
            }
        }

        if (_squad == 2)
        {
            for (int i = 0; i < squad_two.Count; i++)
            {
                squad_two[i].MoveOrder(_positions[i]);
            }
        }
    }


 
    private void SquadGetCover(Vector3 _hit, int _squad)
    {
        // Set Squad Move order
        SquadMoveToCover(cover_manager.GetSquadPositions(_hit, _squad), _squad);
    }



    private IEnumerator SquadGetCoverDelay(Vector3 _hit, int _squad)
    {
        yield return new WaitForSeconds(1);

        // Set Squad Move order
        SquadMoveToCover(cover_manager.GetSquadPositions(_hit, _squad), _squad);
    }



    private void SquadGetFormationPositions(int _no_positions, int _squad)
    {
        SquadFormUp(formation_manager.GetFormationPositions(_no_positions), _squad);
    }



    private IEnumerator SquadGetFormationPositionsDelay(int _no_positions, int _squad)
    {
        yield return new WaitForSeconds(1);

        SquadFormUp(formation_manager.GetFormationPositions(_no_positions), _squad);
    }



    private void SquadCoverReset(int _squad)
    {
        if(_squad == 1)
        {
            cover_manager.ClearSquadPositions(_squad);
        }

        else
            cover_manager.ClearSquadPositions(_squad);
    }



    void SetTargetIndicator(Vector3 _hit_pos)
    {
        target_indicator.transform.position = _hit_pos;
    }


    // Move Squad to given positions
    void SquadMoveToCover(List<Vector3> _positions, int _squad)
    {
        if (_squad == 1)
        {
            for(int i = 0; i < squad_one.Count; i++)
            {
                squad_one[i].CoverOrder(_positions[i]);
            }
        }

        else if (_squad == 2)
        {
            for (int i = 0; i < squad_two.Count; i++)
            {
                squad_two[i].CoverOrder(_positions[i]);
            }
        }
    }



    // Move Squad into formation
    void SquadFormUp(List<GameObject> _positions, int _squad)
    {
        if (_squad == 1)
        {
            for (int i = 0; i < squad_one.Count; i++)
            {
                squad_one[i].SetTarget(_positions[i]);
            }
        }

        else if (_squad == 2)
        {
            for (int i = 0; i < squad_two.Count; i++)
            {
                squad_two[i].SetTarget(_positions[i]);
            }
        }
    }



    // Called from Squad Generator to add members to squads
    public void AddSquadieToSquadOne(AIController _squadie)
    {
        squad_one.Add(_squadie);
    }


    // Called from Squad Generator to add members to squads
    public void AddSquadieToSquadTwo(AIController _squadie)
    {
        squad_two.Add(_squadie);
    }
}

