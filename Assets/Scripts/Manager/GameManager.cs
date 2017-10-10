﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelGenerator level_generator;
    private CoverManager cover_manager;
    private SquadManager squad_manager;


    public GameObject player;

    //Level Generation Variables
    public int level_width = 0;
    public int level_height = 0;
    public int grid_section_size = 20;

    // Cover Generator Variables
    public int no_cover_passes_up = 0;
    public int no_cover_passes_across = 0;
    public float cover_spacing = 0;

    // Squad Generation Variables
    public int no_of_squad_members = 0;
        

	// Use this for initialization
	void Start ()
    {
        level_generator = gameObject.GetComponent<LevelGenerator>();
        cover_manager = gameObject.GetComponent<CoverManager>();
        squad_manager = gameObject.GetComponent<SquadManager>();

        level_generator.GenerateNewLevel(level_height, level_width, grid_section_size);

        Vector3 level_size = new Vector3(level_width * grid_section_size, 0.0f, level_height * grid_section_size);

        cover_manager.GenerateCoverPoints(level_size, no_cover_passes_across, no_cover_passes_up, cover_spacing);

        // Set Player position
        player.transform.position = new Vector3(30.0f, 0.0f, 30.0f);

        // Generate Squad
        if (no_of_squad_members > 6)
            no_of_squad_members = 6;

        squad_manager.GenerateSquad(no_of_squad_members, cover_manager.StartupCover(no_of_squad_members, player.transform.position));
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
