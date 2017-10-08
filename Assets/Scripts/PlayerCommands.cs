using UnityEngine;
using System.Collections;

public class PlayerCommands : MonoBehaviour
{
    private Camera cam;
    public float max_command_distance = 20.0f;
    public GameObject target_indicator;

    public AIMovement squadie_one;
    public AIMovement squadie_two;

    public SquadPositionFollow squad_pos_follow;

    private GameObject prev_cover_squadie_one;
    private GameObject prev_cover_squadie_two;

    private int squadie_one_pos_id;
    private int squadie_two_pos_id;

    private float distance;



    // Use this for initialization
    void Start()
    {
        distance = 0.0f;
        cam = GetComponent<Camera>();
    }



    // Update is called once per frame
    void Update()
    {
        CommandInput();
    }



    void CommandInput()
    {
        if (Input.GetButtonUp("SquadFormUp"))
        {
            FollowCommand();
        }

        if (Input.GetButtonUp("Squadie1Action") || Input.GetButtonUp("Squadie2Action") || Input.GetButtonUp("SquadAction"))
        {

            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // if the Ray hits something & is within range
            if (Physics.Raycast(ray, out hit) && CheckDistance(hit.point) == true)
            {
                // Identify object hit
                if (hit.collider.tag == "Full Cover")
                {
                    CoverCommands(hit);
                }

                else if (hit.collider.tag == "Low Cover")
                {

                }

                else if (hit.collider.tag == "Ground")
                {
                    MoveCommands(hit);
                }
            }
        }
    }



    private void FollowCommand()
    {
        if (Vector3.Distance(squadie_one.transform.position, transform.position) < Vector3.Distance(squadie_two.transform.position, transform.position))
        {
            SquadieOneFormUp();

            StartCoroutine(SquadieTwoFormUpDelay());
        }

        else
        {
            SquadieTwoFormUp();

            StartCoroutine(SquadieOneFormUpDelay());
        }
    }



    private void CoverCommands(RaycastHit _hit)
    {
        // Set TargetIndicator Hit Pos
        SetTargetIndicator(_hit.point);

        if (Input.GetButtonUp("Squadie1Action"))
        {
            SquadieOneCoverOrder(_hit);
        }

        else if (Input.GetButtonUp("Squadie2Action"))
        {
            SquadieTwoCoverOrder(_hit);
        }

        else if (Input.GetButtonUp("SquadAction"))
        {
            if (Vector3.Distance(squadie_one.transform.position, _hit.point) < Vector3.Distance(squadie_two.transform.position, _hit.point))
            {
                SquadieOneCoverOrder(_hit);

                StartCoroutine(SquadieTwoCoverOrderDelay(_hit));
            }

            else
            {
                SquadieTwoCoverOrder(_hit);

                StartCoroutine(SquadieOneCoverOrderDelay(_hit));
            }
        }
    }



    private void MoveCommands(RaycastHit _hit)
    {
        // Set TargetIndicator Hit Pos
        SetTargetIndicator(_hit.point);

        if (Input.GetButtonUp("Squadie1Action"))
        {
            SquadieOneMoveOrder(_hit);
        }

        else if (Input.GetButtonUp("Squadie2Action"))
        {
            SquadieTwoMoveOrder(_hit);
        }

        else if (Input.GetButtonUp("SquadAction"))
        {
            if (Vector3.Distance(squadie_one.transform.position, _hit.point) < Vector3.Distance(squadie_two.transform.position, _hit.point))
            {
                SquadieOneMoveOrder(_hit);
                StartCoroutine(SquadieTwoMoveOrderDelay(_hit));
            }

            else
            {
                SquadieTwoMoveOrder(_hit);
                StartCoroutine(SquadieOneMoveOrderDelay(_hit));
            }
        }
    }



    private void SquadieOneFormUp()
    {
        squadie_one.FollowPlayer();

        if (prev_cover_squadie_one != null)
        {
            prev_cover_squadie_one.GetComponent<CoverPositions>().CoverInUseReset(squadie_one_pos_id);
        }
    }



    private IEnumerator SquadieOneFormUpDelay()
    {
        yield return new WaitForSeconds(1);

        squadie_one.FollowPlayer();

        if (prev_cover_squadie_one != null)
        {
            prev_cover_squadie_one.GetComponent<CoverPositions>().CoverInUseReset(squadie_one_pos_id);
        }
    }



    private void SquadieTwoFormUp()
    {
        squadie_two.FollowPlayer();

        if (prev_cover_squadie_two != null)
        {
            prev_cover_squadie_two.GetComponent<CoverPositions>().CoverInUseReset(squadie_two_pos_id);
        }
    }



    private IEnumerator SquadieTwoFormUpDelay()
    {
        yield return new WaitForSeconds(1);

        squadie_two.FollowPlayer();

        if (prev_cover_squadie_two != null)
        {
            prev_cover_squadie_two.GetComponent<CoverPositions>().CoverInUseReset(squadie_two_pos_id);
        }
    }



    private void SquadieOneMoveOrder(RaycastHit _hit)
    {
        // Move to position, find nearest cover if in danger
        squadie_one.MoveOrder(_hit.point);

        if (prev_cover_squadie_one != null)
        {
            prev_cover_squadie_one.GetComponent<CoverPositions>().CoverInUseReset(squadie_one_pos_id);
        }
    }



    private IEnumerator SquadieOneMoveOrderDelay(RaycastHit _hit)
    {
        yield return new WaitForSeconds(1);
        // Move to position, find nearest cover if in danger
        squadie_one.MoveOrder(_hit.point);

        if (prev_cover_squadie_one != null)
        {
            prev_cover_squadie_one.GetComponent<CoverPositions>().CoverInUseReset(squadie_one_pos_id);
        }
    }



    private void SquadieTwoMoveOrder(RaycastHit _hit)
    {
        // Move to position, find nearest cover if in danger
        squadie_two.MoveOrder(_hit.point);

        if (prev_cover_squadie_two != null)
        {
            prev_cover_squadie_two.GetComponent<CoverPositions>().CoverInUseReset(squadie_two_pos_id);
        }
    }



    private IEnumerator SquadieTwoMoveOrderDelay(RaycastHit _hit)
    {
        yield return new WaitForSeconds(1);
        // Move to position, find nearest cover if in danger
        squadie_two.MoveOrder(_hit.point);

        if (prev_cover_squadie_two != null)
        {
            prev_cover_squadie_two.GetComponent<CoverPositions>().CoverInUseReset(squadie_two_pos_id);
        }
    }



    private void SquadieOneCoverOrder(RaycastHit _hit)
    {
        if (prev_cover_squadie_one != null)
        {
            prev_cover_squadie_one.GetComponent<CoverPositions>().CoverInUseReset(squadie_one_pos_id);
        }

        // Set Squadie1 Move order
        SquadMemberMoveOrder(squadie_one, _hit.collider.gameObject.GetComponent<CoverPositions>().GetPosition(_hit.point, ref squadie_one_pos_id));

        // Create Reference to previous Cover Object
        prev_cover_squadie_one = _hit.collider.gameObject;
    }


    private void SquadieTwoCoverOrder(RaycastHit _hit)
    {
        if (prev_cover_squadie_two != null)
        {
            prev_cover_squadie_two.GetComponent<CoverPositions>().CoverInUseReset(squadie_two_pos_id);
        }

        // Set Squadie1 Move order
        SquadMemberMoveOrder(squadie_two, _hit.collider.gameObject.GetComponent<CoverPositions>().GetPosition(_hit.point, ref squadie_two_pos_id));

        // Create Reference to previous Cover Object
        prev_cover_squadie_two = _hit.collider.gameObject;
    }



    private IEnumerator SquadieOneCoverOrderDelay(RaycastHit _hit)
    {
        yield return new WaitForSeconds(1);
        SquadieOneCoverOrder(_hit);
    }



    private IEnumerator SquadieTwoCoverOrderDelay(RaycastHit _hit)
    {
        yield return new WaitForSeconds(1);
        SquadieTwoCoverOrder(_hit);
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


    void SquadMemberMoveOrder(AIMovement _squadie, Vector3 _position)
    {
        _squadie.MoveOrder(_position);
    }
}

