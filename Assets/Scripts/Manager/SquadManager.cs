using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour
{
    public GameObject squad_member_prefab;
    private int no_squad_members = 0;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    public void GenerateSquad(int _no_squad_members, List<Vector3> _squad_positions)
    {
        no_squad_members = _no_squad_members;

        for (int i = 0; i < no_squad_members; i++)
        {
            Instantiate(squad_member_prefab, _squad_positions[i], squad_member_prefab.transform.rotation);
        }
    }
}
