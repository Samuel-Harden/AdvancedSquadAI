using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverGenerator : MonoBehaviour
{
    public GameObject temp_object;

    public List<Vector3> cover_positions;

    public int no_cover_passes_across = 10;
    public int no_cover_passes_up = 10;

    // to allow for edges of the map, stops waypoints being set off the Nav Mesh
    private float edge_offset = 1;


    public RaycastHit[] hits;

    private void Start()
    {

    }

    public void GenerateCoverPoints(Vector3 _level_dimentions)
    {
        GetPositionsXZUp(_level_dimentions);

        GetPositionsXZDown(_level_dimentions);

        GetPositionsZXRight(_level_dimentions);

        GetPositionsZXLeft(_level_dimentions);
    }



    private void GetPositionsXZUp(Vector3 _level_dimentions)
    {
        float cover_position_spacing = _level_dimentions.x / no_cover_passes_across;

        transform.position = Vector3.zero;

        Debug.Log(transform.position);

        for (int i = 0; i < no_cover_passes_across; i++)
        {
            hits = Physics.RaycastAll(transform.position, transform.forward, _level_dimentions.z);

            int j = 0;
            while (j < hits.Length)
            {
                RaycastHit hit = hits[j];

                if (hit.point.x >= 0.5f && hit.point.x <= _level_dimentions.x - 0.5f)
                {
                    Instantiate(temp_object, new Vector3(hit.point.x, hit.point.y, (hit.point.z - 0.5f)), temp_object.transform.rotation);

                    cover_positions.Add(new Vector3(hit.point.x, hit.point.y, (hit.point.z - 0.5f)));
                }

                j++;
            }
            
            // Update position fornext raycast...
            transform.position = new Vector3(transform.position.x + cover_position_spacing, 0.0f, transform.position.z);
        }
    }



    private void GetPositionsXZDown(Vector3 _level_dimentions)
    {
        float cover_position_spacing = _level_dimentions.x / no_cover_passes_across;

        transform.position = new Vector3(0.0f, 0.0f, _level_dimentions.z);
        
        for (int i = 0; i < no_cover_passes_across; i++)
        {
            hits = Physics.RaycastAll(transform.position, -transform.forward, _level_dimentions.z);

            int j = 0;
            while (j < hits.Length)
            {
                RaycastHit hit = hits[j];

                if (hit.point.x >= 0.5f && hit.point.x <= _level_dimentions.x - 0.5f)
                {
                    Instantiate(temp_object, new Vector3(hit.point.x, hit.point.y, (hit.point.z + 0.5f)), temp_object.transform.rotation);

                    cover_positions.Add(new Vector3(hit.point.x, hit.point.y, (hit.point.z + 0.5f)));
                }

                j++;
            }

            // Update position for next raycast...
            transform.position = new Vector3(transform.position.x + cover_position_spacing, 0.0f, transform.position.z);
        }
    }



        private void GetPositionsZXRight(Vector3 _level_dimentions)
    {
        float cover_position_spacing = _level_dimentions.z / no_cover_passes_up;

        transform.position = Vector3.zero;
        
        for (int i = 0; i < no_cover_passes_up; i++)
        {
            hits = Physics.RaycastAll(transform.position, transform.right, _level_dimentions.z);

            int j = 0;
            while (j < hits.Length)
            {
                RaycastHit hit = hits[j];

                if (hit.point.z >= 0.5f && hit.point.z <= _level_dimentions.z - 0.5f)
                {
                    Instantiate(temp_object, new Vector3((hit.point.x - 0.5f), hit.point.y, hit.point.z), temp_object.transform.rotation);

                    cover_positions.Add(new Vector3((hit.point.x - 0.5f), hit.point.y, hit.point.z));
                }

                j++;
            }

            // Update position for next raycast...
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z + cover_position_spacing);
        }
    }



    private void GetPositionsZXLeft(Vector3 _level_dimentions)
    {
        float cover_position_spacing = _level_dimentions.z / no_cover_passes_up;

        transform.position = new Vector3(_level_dimentions.x, 0.0f, 0.0f);

        for (int i = 0; i < no_cover_passes_up; i++)
        {
            hits = Physics.RaycastAll(transform.position, -transform.right, _level_dimentions.z);

            int j = 0;
            while (j < hits.Length)
            {
                RaycastHit hit = hits[j];

                if (hit.point.z >= 0.5f && hit.point.z <= _level_dimentions.z - 0.5f)
                {
                    Instantiate(temp_object, new Vector3((hit.point.x + 0.5f), hit.point.y, hit.point.z), temp_object.transform.rotation);

                    cover_positions.Add(new Vector3((hit.point.x + 0.5f), hit.point.y, hit.point.z));
                }

                j++;
            }

            // Update position for next raycast...
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z + cover_position_spacing);
        }
    }
}
