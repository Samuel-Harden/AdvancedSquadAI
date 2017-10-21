using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelGenerator   level_generator;
    private SquadGenerator   squad_generator;
    private EnemyGenerator   enemy_generator;
    private FormationManager formation_manager;
    private CoverManager     cover_manager;


    public GameObject player;

    private List<GameObject> squadies;
    private List<GameObject> enemies;

    //Level Generation Variables
    public int level_width = 0;
    public int level_height = 0;
    public int grid_section_size = 20;

    // Cover Generator Variables
    public int no_cover_passes_up = 0;
    public int no_cover_passes_across = 0;
    public float cover_spacing = 0;

    // Squad Generation Variables
    public int no_of_squadies = 0;

    private int no_of_enemies = 0;


    // Use this for initialization
    void Start ()
    {
        int squad_size = 0;

        level_generator   = gameObject.GetComponent<LevelGenerator>();
        squad_generator   = gameObject.GetComponent<SquadGenerator>();
        enemy_generator   = gameObject.GetComponent<EnemyGenerator>();
        cover_manager     = gameObject.GetComponent<CoverManager>();
        formation_manager = gameObject.GetComponent<FormationManager>();

        squadies = new List<GameObject>();
        enemies  = new List<GameObject>();

        // Generate Squad
        if (no_of_squadies > 8)
            no_of_squadies = 8;

        no_of_enemies = no_of_squadies * 2;

        level_generator.GenerateNewLevel(level_height, level_width, grid_section_size);

        Vector3 level_size = new Vector3(level_width * grid_section_size, 0.0f, level_height * grid_section_size);

        // This is to stop duplicate cover spots spawning!
        if (cover_spacing <= 0)
            cover_spacing = 1;

        cover_manager.GenerateCoverPoints(level_size, no_cover_passes_across, no_cover_passes_up, cover_spacing);

        // Set Player position
        //player.transform.position = new Vector3(30.0f, 0.0f, 30.0f);

        // Position ID's
        cover_manager.SetSquads(no_of_squadies);

        squad_generator.GenerateSquad(no_of_squadies, cover_manager.StartupCover(no_of_squadies, player.transform.position), ref squadies);

        if(no_of_squadies%2 == 1) // If we have a remainder ie Squad sizes are not even
        {
            squad_size = no_of_squadies / 2 + 1; // We use the biggest squad size
        }

        if(no_of_squadies%2 == 0) // If they are even all good
        {
            squad_size = no_of_squadies / 2;
        }

        // Tell the formation manager about the largest squad size
        formation_manager.GenerateFormations(player, squad_size);

        enemy_generator.GenerateEnemies(no_of_enemies, cover_manager.GetEnemyPositions(no_of_enemies, enemy_generator.GetSpawnPoints()), ref enemies);
    }
}
