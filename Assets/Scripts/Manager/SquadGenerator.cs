using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadGenerator : MonoBehaviour
{
    public SquadCommands squad_commands;
    public GameObject squad_member_prefab;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void GenerateSquad(int _no_squad_members, List<Vector3> _squad_positions)
    {

        for (int i = 0; i < _no_squad_members; i++)
        {
            var squadie = Instantiate(squad_member_prefab, _squad_positions[i], squad_member_prefab.transform.rotation);

            if (i < _no_squad_members / 2)
            {
                squad_commands.AddSquadieToSquadOne(squadie.gameObject.GetComponent<AIMovement>());
            }

            else
                squad_commands.AddSquadieToSquadTwo(squadie.gameObject.GetComponent<AIMovement>());
        }
    }
}
