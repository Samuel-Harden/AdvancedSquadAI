using UnityEngine;

public class PlayerCommannd : MonoBehaviour
{

    public GameObject targetIndicator;
    public float maxCommandDistance = 20.0f;

    private Camera cam;



	// Use this for initialization
	void Start ()
    {
        cam = GetComponent<Camera>();
	}
	


	// Update is called once per frame
	void Update ()
    {
        commandInput();
	}



    void commandInput()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // if the Ray hits something & is within range
            if (Physics.Raycast(ray, out hit) && checkDistance(hit.point) == true)
            {
                // Pass through the position that was hit
                setTarget(hit.point);
            }
        }
    }



    bool checkDistance(Vector3 hitPos)
    {
        float distance = Vector3.Distance(cam.transform.position, hitPos);

        // if distance is too big, return false
        if (distance > maxCommandDistance)
        {
            return false;
        }

        // distance is good
        else return true;
    }



    void setTarget(Vector3 hitPos)
    {
        targetIndicator.transform.position = hitPos;
    }
}
