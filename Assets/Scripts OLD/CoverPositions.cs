using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPositions : MonoBehaviour
{
    [SerializeField] List<Transform> cover_positions;
    [SerializeField] List<bool> cover_positions_in_use;

	// Use this for initialization
	void Start ()
    {
        // Set up list of Cover Positions
        foreach(Transform cover_pos in transform)
        {
            cover_positions.Add(cover_pos);
        }

        for(int i = 0; i < cover_positions.Count; i++)
        {
            cover_positions_in_use.Add(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    
    public Vector3 GetPosition(Vector3 _order_position, ref int _pos_id)
    {
        // Some Arbitrary number
        float temp = 1000;

        int pos_id = 0;

        // Loop through positions
        for (int i = 0; i < cover_positions.Count; i++)
        {
            // if position is NOT in use
            if(cover_positions_in_use[i] == false)
            {
                // set the temp pos to be this position
                temp = Vector3.Distance(_order_position, cover_positions[i].position);

                pos_id = i;
            }
        }
        
        // Now loop through all positions
        for(int i = 0; i < cover_positions.Count; i++)
        {
            // is this position in use? skip if it is
            if (cover_positions_in_use[i] == false)
            {
                // if the last position is greater... we have a new waypoint (as its closer)
                if (temp > (Vector3.Distance(_order_position, cover_positions[i].position)))
                {
                    // set this to be the new position
                    temp = Vector3.Distance(_order_position, cover_positions[i].position);
                    pos_id = i;
                }
            }
        }

        cover_positions_in_use[pos_id] = true;

        Debug.Log(pos_id);

        _pos_id = pos_id;

        return cover_positions[pos_id].transform.position;
    }


    // Reset Cover in use
    public void CoverInUseReset(int _pos_id)
    {
        Debug.Log("Reset");
        cover_positions_in_use[_pos_id] = false;
    }
}


