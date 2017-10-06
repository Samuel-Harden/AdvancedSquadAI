using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPositions : MonoBehaviour
{
    [SerializeField] List<Transform> cover_positions;

	// Use this for initialization
	void Start ()
    {
        // Set up list of Cover Positions
        foreach(Transform cover_pos in transform)
        {
            cover_positions.Add(cover_pos);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    
    public Vector3 GetPosition(Vector3 _order_position)
    {
        Debug.Log(_order_position);
        // Get distance of 1st position
        float temp = Vector3.Distance(_order_position, cover_positions[0].position);

        int pos_id = 0;

        for(int i = 0; i < cover_positions.Count; i++)
        {
            // if the last position is greater... we have a new waypoint (as its closer)
            if (temp > (Vector3.Distance(_order_position, cover_positions[i].position)))
            {
                temp = Vector3.Distance(_order_position, cover_positions[i].position);

                // set this to be the new position
                pos_id = i;
            }
        }

        Debug.Log(pos_id);
        return cover_positions[pos_id].transform.position;
    }
}


