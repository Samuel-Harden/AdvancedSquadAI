using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquadCommands : MonoBehaviour
{
    private Camera cam;
    private float distance;

    public float max_command_distance = 20.0f;
    public GameObject target_indicator;

    private List<AIMovement> squad_one;
    private List<AIMovement> squad_two;


    // Use this for initialization
    void Start()
    {
        distance = 0.0f;
        cam = GetComponent<Camera>();
        squad_one = new List<AIMovement>();
        squad_two = new List<AIMovement>();
    }



    // Update is called once per frame
    void Update()
    {
        CommandInput();
    }



    void CommandInput()
    {
        if (Input.GetButtonDown("TeamFormUp"))
        {
            //FollowCommand();
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

                // Identify object hit
                if (hit.collider.tag == "Full Cover")
                {
                    CoverCommands(hit);
                }

                else if (hit.collider.tag == "Low Cover")
                {
                    // Low Cover Commands to go here!
                }

                else if (hit.collider.tag == "Ground")
                {
                    //MoveCommands(hit);
                }
            }
        }
    }


    // When the Raycast has hit a 'Cover' Type object (Tag)
    private void CoverCommands(RaycastHit _hit)
    {
        if (Input.GetButtonDown("SquadOneAction"))
        {
            SquadOneCoverReset();
            SquadOneCoverOrder(_hit);
        }

        else if (Input.GetButtonDown("SquadTwoAction"))
        {
            SquadTwoCoverReset();
            SquadTwoCoverOrder(_hit);
        }

        else if (Input.GetButtonDown("TeamAction"))
        {
            SquadOneCoverReset();
            SquadTwoCoverReset();

            // Check Which Squad Memebr is closest
            /*if (Vector3.Distance(squad_one.transform.position, _hit.point) < Vector3.Distance(squad_two.transform.position, _hit.point))
            {
                // If Squadie One is closest...
                SquadOneCoverOrder(_hit);
                StartCoroutine(SquadTwoCoverOrderDelay(_hit));
            

            else
            {
                // if Squadie Two is closest...
                SquadTwoCoverOrder(_hit);
                StartCoroutine(SquadOneCoverOrderDelay(_hit));
            }*/
        }
    }


    /*
    private void FollowCommand()
    {
        SquadOneCoverReset();
        SquadTwoCoverReset();

        if (Vector3.Distance(squad_one.transform.position, transform.position) < Vector3.Distance(squad_two.transform.position, transform.position))
        {
            SquadOneFormUp();
            StartCoroutine(SquadTwoFormUpDelay());
        }

        else
        {
            SquadieTwoFormUp();
            StartCoroutine(SquadOneFormUpDelay());
        }
    }



    private void MoveCommands(RaycastHit _hit)
    {
        if (Input.GetButtonDown("SquadOneAction"))
        {
            SquadOneCoverReset();
            SquadOneMoveOrder(_hit);
        }

        else if (Input.GetButtonDown("SquadTwoAction"))
        {
            SquadTwoCoverReset();
            SquadTwoMoveOrder(_hit);
        }

        else if (Input.GetButtonDown("TeamAction"))
        {
            SquadOneCoverReset();
            SquadTwoCoverReset();

            if (Vector3.Distance(squad_one.transform.position, _hit.point) < Vector3.Distance(squad_two.transform.position, _hit.point))
            {
                SquadOneMoveOrder(_hit);

                StartCoroutine(SquadTwoMoveOrderDelay(_hit));
            }

            else
            {
                SquadTwoMoveOrder(_hit);

                StartCoroutine(SquadOneMoveOrderDelay(_hit));
            }
        }
    }



    private void SquadOneFormUp()
    {
        squad_one.FollowPlayer();
    }



    private IEnumerator SquadOneFormUpDelay()
    {
        yield return new WaitForSeconds(1);

        squad_one.FollowPlayer();
    }



    private void SquadieTwoFormUp()
    {
        squad_two.FollowPlayer();
    }



    private IEnumerator SquadTwoFormUpDelay()
    {
        yield return new WaitForSeconds(1);

        squad_two.FollowPlayer();
    }



    private void SquadOneMoveOrder(RaycastHit _hit)
    {
        squad_one.MoveOrder(_hit.point);
    }



    private IEnumerator SquadOneMoveOrderDelay(RaycastHit _hit)
    {
        yield return new WaitForSeconds(1);

        squad_one.MoveOrder(_hit.point);
    }



    private void SquadTwoMoveOrder(RaycastHit _hit)
    {
        squad_two.MoveOrder(_hit.point);
    }



    private IEnumerator SquadTwoMoveOrderDelay(RaycastHit _hit)
    {
        yield return new WaitForSeconds(1);

        squad_two.MoveOrder(_hit.point);
    }*/


    // 
    private void SquadOneCoverOrder(RaycastHit _hit)
    {
        // Set Squadie1 Move order
        /*SquadMemberMoveOrder(squad_one, _hit.collider.gameObject.GetComponent<CoverPositions>().GetPosition(_hit.point, ref squadie_one_pos_id));

        // Create Reference to previous Cover Object
        prev_cover_squadie_one = _hit.collider.gameObject;*/
    }


    private void SquadTwoCoverOrder(RaycastHit _hit)
    {
        // Set Squadie2 Move order
        /*SquadMemberMoveOrder(squad_two, _hit.collider.gameObject.GetComponent<CoverPositions>().GetPosition(_hit.point, ref squadie_two_pos_id));

        // Create Reference to previous Cover Object
        prev_cover_squadie_two = _hit.collider.gameObject;*/
    }



    private void SquadCoverOrder(RaycastHit _hit)
    {

    }



    private IEnumerator SquadOneCoverOrderDelay(RaycastHit _hit)
    {
        yield return new WaitForSeconds(1);
        SquadOneCoverOrder(_hit);
    }



    private IEnumerator SquadTwoCoverOrderDelay(RaycastHit _hit)
    {
        yield return new WaitForSeconds(1);
        SquadTwoCoverOrder(_hit);
    }



    private void SquadOneCoverReset()
    {
        /*if (prev_cover_squadie_one != null)
        {
            prev_cover_squadie_one.GetComponent<CoverPositions>().CoverInUseReset(squadie_one_pos_id);
        }*/
    }



    private void SquadTwoCoverReset()
    {
        /*if (prev_cover_squadie_two != null)
        {
            prev_cover_squadie_two.GetComponent<CoverPositions>().CoverInUseReset(squadie_two_pos_id);
        }*/
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


    // Move Squadie to given position
    void SquadMemberMoveOrder(AIMovement _squadie, Vector3 _position)
    {
        _squadie.MoveOrder(_position);
    }


    public void AddSquadieToSquadOne(AIMovement _squadie)
    {
        squad_one.Add(_squadie);
    }



    public void AddSquadieToSquadTwo(AIMovement _squadie)
    {
        squad_two.Add(_squadie);
    }
}

