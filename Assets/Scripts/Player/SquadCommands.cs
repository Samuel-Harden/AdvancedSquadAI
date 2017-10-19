using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquadCommands : MonoBehaviour
{
    private Camera cam;
    private float distance;

    public float max_command_distance = 20.0f;
    public GameObject target_indicator;

    private List<AIController> squad_one;
    private List<AIController> squad_two;

    public CoverManager cover_manager;
    public FormationManager formation_manager;

    private int squad_1 = 1;
    private int squad_2 = 2;


    // Use this for initialization
    void Start()
    {
        distance = 0.0f;
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
            if (Physics.Raycast(ray, out hit) && CheckDistance(hit.point) == true)
            {
                // Set TargetIndicator Hit Pos
                SetTargetIndicator(hit.point);

                Vector3 hit_ground = new Vector3(hit.point.x, 0.0f, hit.point.z);

                // Identify object hit
                if (hit.collider.tag == "Full Cover" || hit.collider.tag == "Low Cover")
                {
                    CoverCommands(hit_ground);
                }

                else if (hit.collider.tag == "Low Cover")
                {
                    // Low Cover Commands to go here!
                }

                else if (hit.collider.tag == "Ground")
                {
                    MoveCommands(hit_ground);
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
            SquadMoveOrder(_hit, squad_1);
        }

        else if (Input.GetButtonDown("SquadTwoAction"))
        {
            SquadCoverReset(squad_2);
            SquadMoveOrder(_hit, squad_2);
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
                SquadMoveOrder(_hit, squad_1);

                StartCoroutine(SquadMoveOrderDelay(_hit, squad_2));
            }

            else
            {
                SquadMoveOrder(_hit, squad_2);

                StartCoroutine(SquadMoveOrderDelay(_hit, squad_1));
            }
        }
    }



    private void SquadMoveOrder(Vector3 _hit, int _squad)
    {
        if (_squad == 1)
        {
            for (int i = 0; i < squad_one.Count; i++)
            {
                squad_one[i].MoveOrder(_hit);
            }
        }

        if (_squad == 2)
        {
            for (int i = 0; i < squad_two.Count; i++)
            {
                squad_two[i].MoveOrder(_hit);
            }
        }
    }


    // SAME FUNCTION AS ABOVE, JUST WITH A DELAY
    private IEnumerator SquadMoveOrderDelay(Vector3 _hit, int _squad)
    {
        yield return new WaitForSeconds(1);

        if (_squad == 1)
        {
            for (int i = 0; i < squad_one.Count; i++)
            {
                squad_one[i].MoveOrder(_hit);
            }
        }

        if (_squad == 2)
        {
            for (int i = 0; i < squad_two.Count; i++)
            {
                squad_two[i].MoveOrder(_hit);
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



    bool CheckDistance(Vector3 _hit_pos)
    {
        distance = Vector3.Distance(cam.transform.position, _hit_pos);

        // if distance is too big, return false
        if (distance > max_command_distance)
        {
            return false;
        }

        // distance is good
        else return true;
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

