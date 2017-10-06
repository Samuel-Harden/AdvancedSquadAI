using UnityEngine;

public class PlayerCommands : MonoBehaviour
{
    private Camera cam;
    public float max_command_distance = 20.0f;
    public GameObject target_indicator;

    public AIMovement squadie_one;
    public AIMovement squadie_two;

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
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // if the Ray hits something & is within range
            if (Physics.Raycast(ray, out hit) && CheckDistance(hit.point) == true)
            {
                // Identify object hit
                if(hit.collider.tag == "Full Cover")
                {
                    // Set TargetIndicator Hit Pos
                    SetTargetIndicator(hit.point);

                    Debug.Log(hit.point);

                    // Set Position for Squad Member One

                    SquadMemberMoveOrder(squadie_one, hit.collider.gameObject.GetComponent<CoverPositions>().GetPosition(hit.point));
                }
            }
        }
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

