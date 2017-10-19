using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    [SerializeField] List<GameObject> formation_prefabs;

    private List<List<GameObject>> all_formation_positions;
    private List<List<bool>> all_formation_positions_in_use;
    private GameObject player;

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
        all_formation_positions = new List<List<GameObject>>();
        all_formation_positions_in_use = new List<List<bool>>();

        player = _player;

        var formation = Instantiate(formation_prefabs[0],
            new Vector3(_player.transform.position.x, 0.0f, _player.transform.position.y),
            formation_prefabs[0].transform.rotation);

        formation.GetComponent<NavMeshFollow>().SetTarget(_player.transform);

        List<GameObject> formation_positions = new List<GameObject>();
        List<bool> formation_positions_in_use = new List<bool>();

        foreach(Transform child in formation.transform)
        {
            formation_positions.Add(child.gameObject);
            formation_positions_in_use.Add(false);
            //Debug.Log(child.position);
        }

        all_formation_positions.Add(formation_positions);
        all_formation_positions_in_use.Add(formation_positions_in_use);

        formation.SetActive(true);
    }



    public void CycleFormation(int _squad_one_size, int _squad_two_size, int _squad_to_form_up)
    { 
        int _squad_size;

       if (_squad_to_form_up == 1)
        {
            _squad_size = _squad_one_size;
        }

       if (_squad_to_form_up == 2)
        {
            _squad_size = _squad_two_size;
        }

       if (_squad_to_form_up == 0)
        {
            _squad_size = _squad_one_size + _squad_two_size;
        }
    }



    public List<GameObject> GetFormationPositions(int _no_positions)
    {
        List<GameObject> positions = new List<GameObject>();

        for(int i = 0; i < _no_positions; i++)
        {
            positions.Add(all_formation_positions[0][i]);
        }
        return positions;
    }
}