using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    public GameObject temp_object;
    public bool test_cover_pos_markers = false;

    private List<Vector3> cover_positions;

    private List<bool> cover_positions_in_use;

    private List<int> squad_one_position_ids;
    private List<int> squad_two_position_ids;

    private int no_cover_passes_across = 0;
    private int no_cover_passes_up = 0;
    private float nav_mesh_offset = 0.5f;
    private float cover_spacing = 0.0f;

    public RaycastHit[] hits;

    private void Start()
    {
        cover_positions = new List<Vector3>();
        cover_positions_in_use = new List<bool>();

        squad_one_position_ids = new List<int>();
        squad_two_position_ids = new List<int>();
    }



    public void GenerateCoverPoints(Vector3 _level_dimentions, int _no_cover_passes_across, int _no_cover_passes_up, float _cover_spacing)
    {
        no_cover_passes_across = _no_cover_passes_across;
        no_cover_passes_up = _no_cover_passes_up;
        cover_spacing = _cover_spacing;

        GetPositionsXZUp(_level_dimentions);

        GetPositionsXZDown(_level_dimentions);

        GetPositionsZXRight(_level_dimentions);

        GetPositionsZXLeft(_level_dimentions);
    }



    private void GetPositionsXZUp(Vector3 _level_dimentions)
    {
        float cover_position_spacing = _level_dimentions.x / no_cover_passes_across;

        transform.position = Vector3.zero;

        for (int i = 0; i < no_cover_passes_across; i++)
        {
            hits = Physics.RaycastAll(transform.position, transform.forward, _level_dimentions.z);

            int k = 0;
            while (k < hits.Length)
            {
                RaycastHit hit = hits[k];

                // Offset for edge of Level
                if (hit.point.x >= 0.5f && hit.point.x <= _level_dimentions.x - 0.5f)
                {
                    float closest_position = 1000.0f;

                    // loop through all current cover Positions...
                    for (int j = 0; j < cover_positions.Count; j++)
                    {
                        // find the closest position relavent to this new hit point
                        if (Vector3.Distance(hit.point, cover_positions[j]) < closest_position)
                        {
                            closest_position = Vector3.Distance(hit.point, cover_positions[j]);
                        }
                    }

                    // Once we have closest position, as long as its further than 'cover spacing' away, we can set this as a cover spot
                    if (closest_position >= cover_spacing)
                    {
                        if(test_cover_pos_markers == true)
                        {
                            Instantiate(temp_object, new Vector3(hit.point.x, hit.point.y, (hit.point.z - nav_mesh_offset)), temp_object.transform.rotation);
                        }


                        cover_positions.Add(new Vector3(hit.point.x, hit.point.y, (hit.point.z - nav_mesh_offset)));

                        cover_positions_in_use.Add(false);
                    }
                }

                k++;
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

            int k = 0;
            while (k < hits.Length)
            {
                RaycastHit hit = hits[k];

                // Offset for edge of Level
                if (hit.point.x >= 0.5f && hit.point.x <= _level_dimentions.x - 0.5f)
                {
                    float closest_position = 1000.0f;

                    // loop through all current cover Positions...
                    for (int j = 0; j < cover_positions.Count; j++)
                    {
                        // find the closest position relavent to this new hit point
                        if (Vector3.Distance(hit.point, cover_positions[j]) < closest_position)
                        {
                            closest_position = Vector3.Distance(hit.point, cover_positions[j]);
                        }
                    }

                    // Once we have closest position, as long as its further than 0.5 away, we can set this as a cover spot
                    if (closest_position >= cover_spacing)
                    {
                        if (test_cover_pos_markers == true)
                        {
                            Instantiate(temp_object, new Vector3(hit.point.x, hit.point.y, (hit.point.z + nav_mesh_offset)), temp_object.transform.rotation);
                        }
                        

                        cover_positions.Add(new Vector3(hit.point.x, hit.point.y, (hit.point.z + nav_mesh_offset)));

                        cover_positions_in_use.Add(false);
                    }
                }

                k++;
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

            int k = 0;
            while (k < hits.Length)
            {
                RaycastHit hit = hits[k];

                // Offset for edge of Level
                if (hit.point.z >= 0.5f && hit.point.z <= _level_dimentions.z - 0.5f)
                {
                    float closest_position = 1000.0f;

                    // loop through all current cover Positions...
                    for (int j = 0; j < cover_positions.Count; j++)
                    {
                        // find the closest position relavent to this new hit point
                        if (Vector3.Distance(hit.point, cover_positions[j]) < closest_position)
                        {
                            closest_position = Vector3.Distance(hit.point, cover_positions[j]);
                        }
                    }

                    // Once we have closest position, as long as its further than 0.5 away, we can set this as a cover spot
                    if (closest_position >= cover_spacing)
                    {
                        if (test_cover_pos_markers == true)
                        {
                            Instantiate(temp_object, new Vector3((hit.point.x - 0.5f), hit.point.y, hit.point.z), temp_object.transform.rotation);
                        }

                        cover_positions.Add(new Vector3((hit.point.x - 0.5f), hit.point.y, hit.point.z));

                        cover_positions_in_use.Add(false);
                    }
                }

                k++;
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

            int k = 0;
            while (k < hits.Length)
            {
                RaycastHit hit = hits[k];

                // Offset for edge of Level
                if (hit.point.z >= 0.5f && hit.point.z <= _level_dimentions.z - 0.5f)
                {
                    float closest_position = 1000.0f;

                    // loop through all current cover Positions...
                    for (int j = 0; j < cover_positions.Count; j++)
                    {
                        // find the closest position relavent to this new hit point
                        if (Vector3.Distance(hit.point, cover_positions[j]) < closest_position)
                        {
                            closest_position = Vector3.Distance(hit.point, cover_positions[j]);
                        }
                    }

                    // Once we have closest position, as long as its further than 0.5 away, we can set this as a cover spot
                    if (closest_position >= cover_spacing)
                    {
                        if (test_cover_pos_markers == true)
                        {
                            Instantiate(temp_object, new Vector3((hit.point.x + 0.5f), hit.point.y, hit.point.z), temp_object.transform.rotation);
                        }

                        cover_positions.Add(new Vector3((hit.point.x + 0.5f), hit.point.y, hit.point.z));

                        cover_positions_in_use.Add(false);
                    }
                }

                k++;
            }

            // Update position for next raycast...
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z + cover_position_spacing);
        }
    }



    public List<Vector3> StartupCover(int _no_positions, Vector3 _player_pos)
    {
        List<Vector3> start_cover_positions = new List<Vector3>();

        float temp_pos = 1000.0f;

        int pos_id = 0;

        int squad_no = 0;
        int squad_one_pos = 0;
        int squad_two_pos = 0;

        // find a position for each squad member
        for (int i = 0; i < _no_positions; i++)
        {
            // check through all positions
            for (int j = 0; j < cover_positions.Count; j++)
            {
                // If this position is available?
                if (cover_positions_in_use[j] == false)
                {
                    // if the position is closer
                    if(temp_pos > (Vector3.Distance(_player_pos, cover_positions[j])))
                    {
                        temp_pos = Vector3.Distance(_player_pos, cover_positions[j]);

                        pos_id = j;
                    }
                }
            }

            if (squad_no < _no_positions / 2)
            {
                squad_one_position_ids[squad_one_pos] = pos_id;
                squad_one_pos++;
            }

            else
            {
                squad_two_position_ids[squad_two_pos] = pos_id;
                squad_two_pos++;
            }

            cover_positions_in_use[pos_id] = true;

            start_cover_positions.Add(cover_positions[pos_id]);

            // RESET VARIABLES
            pos_id = 0;
            temp_pos = 1000.0f;

            squad_no++;
        }

        return start_cover_positions;
    }



    public void SetSquads(int _no_of_squad_members)
    {
        for (int i = 0; i < _no_of_squad_members / 2; i++)
        {
            squad_one_position_ids.Add(0);
        }

        for (int i = _no_of_squad_members / 2; i < _no_of_squad_members; i++)
        {
            squad_two_position_ids.Add(0);
        }
    }



    public void ClearSquadPositions(int _squad)
    {
        if (_squad == 1)
        {
            for(int i = 0; i < squad_one_position_ids.Count; i++)
            {
                cover_positions_in_use[squad_one_position_ids[i]] = false;
            }
        }

        if (_squad == 2)
        {
            for (int i = 0; i < squad_two_position_ids.Count; i++)
            {
                cover_positions_in_use[squad_two_position_ids[i]] = false;
            }
        }
    }



    public List<Vector3> GetSquadPositions(RaycastHit _hit, int _squad)
    {
        List<Vector3> squad_new_positions = new List<Vector3>();

        int no_positions = 0;

        if (_squad == 1)
        {
            no_positions = squad_one_position_ids.Count;
        }

        if (_squad == 2)
        {
            no_positions = squad_two_position_ids.Count;
        }

        float temp_pos = 1000.0f;

        int pos_id = 0;
        int squad_pos = 0;

        // find a position for each squad member
        for (int i = 0; i < no_positions; i++)
        {
            // check through all positions
            for (int j = 0; j < cover_positions.Count; j++)
            {
                // If this position is available?
                if (cover_positions_in_use[j] == false)
                {
                    // if the position is closer
                    if (temp_pos > (Vector3.Distance(_hit.point, cover_positions[j])))
                    {
                        temp_pos = Vector3.Distance(_hit.point, cover_positions[j]);

                        pos_id = j;
                    }
                }
            }

            if (_squad == 1)
            {
                squad_one_position_ids[squad_pos] = pos_id;
                squad_pos++;
            }

            if (_squad == 2)
            {
                squad_two_position_ids[squad_pos] = pos_id;
                squad_pos++;
            }

            cover_positions_in_use[pos_id] = true;

            squad_new_positions.Add(cover_positions[pos_id]);

            // RESET VARIABLES
            pos_id = 0;
            temp_pos = 1000.0f;
        }

        return squad_new_positions;
    }
}