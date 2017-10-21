using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    [SerializeField] List<GameObject> formation_prefabs;

    private List<GameObject> formations;
    private List<List<GameObject>> all_formation_positions;
    private List<List<bool>> all_formation_positions_in_use;

    private List<List<int>> squad_one_pos_ids;
    private List<List<int>> squad_two_pos_ids;

    private GameObject player;

    private int formation_id;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    // generates the required formations based on initial squad size
    public void GenerateFormations(GameObject _player, int _squad_size)
    {
        formation_id = 0;

        formations = new List<GameObject>();
        all_formation_positions = new List<List<GameObject>>();
        all_formation_positions_in_use = new List<List<bool>>();

        squad_one_pos_ids = new List<List<int>>();
        squad_two_pos_ids = new List<List<int>>();

        player = _player;

        for(int i = 0; i < formation_prefabs.Count; i++)
        {
            var formation = Instantiate(formation_prefabs[i],
                new Vector3(_player.transform.position.x, 0.0f, _player.transform.position.y),
                formation_prefabs[i].transform.rotation);

            formation.GetComponent<NavMeshFollow>().SetTarget(_player.transform);

            List<GameObject> formation_positions = new List<GameObject>();
            List<bool> formation_positions_in_use = new List<bool>();

            foreach (Transform child in formation.transform)
            {
                formation_positions.Add(child.gameObject);
                formation_positions_in_use.Add(false);
            }

            formations.Add(formation);
            all_formation_positions.Add(formation_positions);
            all_formation_positions_in_use.Add(formation_positions_in_use);

            List<int> squad_one_ids = new List<int>();
            List<int> squad_two_ids = new List<int>();

            for (int j = 0; j < _squad_size; j++)
            {
                squad_one_ids.Add(0);
                squad_two_ids.Add(0);
            }

            squad_one_pos_ids.Add(squad_one_ids);
            squad_two_pos_ids.Add(squad_two_ids);
        }
    }



    public void CycleFormation()
    {
        formation_id += 1;

        if (formation_id == formation_prefabs.Count)
        {
            formation_id = 0;
        }
    }


    // NEED T TAKE INTO ACCOUNT DISTANCE
    public List<GameObject> GetFormationPositions(int _no_positions, int _squad)
    {
        List<GameObject> positions = new List<GameObject>();

        for(int i = 0; i < _no_positions; i++)
        {
            for (int j = 0; j < all_formation_positions[formation_id].Count; j++)
            {
                if (all_formation_positions_in_use[formation_id][j] == false)
                {
                    positions.Add(all_formation_positions[formation_id][j]);
                    all_formation_positions_in_use[formation_id][j] = true;

                    if (_squad == 1)
                    {
                        squad_one_pos_ids[formation_id][i] = j;
                        //Debug.Log(squad_one_pos_ids[formation_id][i]);
                    }

                    if (_squad == 2)
                    {
                        squad_two_pos_ids[formation_id][i] = j;
                        //Debug.Log(squad_two_pos_ids[formation_id][i]);
                    }

                    break;
                }
            }
        }

        return positions;
    }

    
    // Called before Formation is changed!
    public void ClearSquadOnePositions()
    {
        for(int i = 0; i < squad_one_pos_ids[formation_id].Count; i++)
        {
            all_formation_positions_in_use[formation_id][squad_one_pos_ids[formation_id][i]] = false;
        }
    }


    // Called before Formation is changed!
    public void ClearSquadTwoPositions()
    {
        for (int i = 0; i < squad_two_pos_ids[formation_id].Count; i++)
        {
            all_formation_positions_in_use[formation_id][squad_two_pos_ids[formation_id][i]] = false;
        }
    }
}