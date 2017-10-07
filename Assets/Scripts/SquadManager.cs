using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour
{
    public List<AIMovement> squadies;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetSquadies();
    }

    void SetSquadies()
    {
        for (int i = 0; i < squadies.Count; i++)
        {
            squadies[i].Run(squadies);
        }
    }
}

