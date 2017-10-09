using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    LevelGenerator level_generator;
    CoverGenerator cover_generator;

    public int grid_section_width;
    public int grid_section_height;

	// Use this for initialization
	void Start ()
    {
        level_generator = gameObject.GetComponent<LevelGenerator>();
        cover_generator = gameObject.GetComponent<CoverGenerator>();

        level_generator.GenerateNewLevel();

        Vector3 level_size = level_generator.GetLevelSize();

        cover_generator.GenerateCoverPoints(level_size);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
