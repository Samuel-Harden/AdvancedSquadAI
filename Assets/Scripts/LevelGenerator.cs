using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Contains Section Pieces
    [SerializeField] GameObject[] standard_sections;
    //[SerializeField] GameObject[] depot_sections;
    [SerializeField] GameObject[] edge_sections;

    // Dictates Size of the City
    [SerializeField] int level_width = 10;
    [SerializeField] int level_height = 10;

    //[SerializeField] int total_no_depots = 10;

    // 0 = standard section
    // 1 = Depot section

    // Used to create the border for the city
    private int level_bottom_left_corner = 0;
    private int level_bottom_across = 1;
    private int level_bottom_right_corner = 2;
    private int level_left_up = 3;
    private int level_right_up = 4;
    private int level_top_left_corner = 5;
    private int level_top_across = 6;
    private int level_top_right_corner = 7;

    private int[,] level_grid;

    public int grid_section_size = 20;


    // Use this for initialization
    void Start()
    {
               
    }



    public void GenerateNewLevel()
    {
        level_grid = new int[level_height, level_width];

        GenerateLevelData();

        //GenerateDepotData();

        PopulateLevel();
    }



    private void GenerateLevelData()
    {
        for (int h = 0; h < level_height; h++)
        {
            for (int w = 0; w < level_width; w++)
            {
                EdgeSectionCheck(h, w);
            }
        }
    }



    private void EdgeSectionCheck(int h, int w)
    {
        // Remember the Grid is generate bottom left to top right,
        // across (w), then up (h) repeated

        // If this section is the bottom left section
        if (w == 0 && h == 0)
        {
            level_grid[h, w] = level_bottom_left_corner;
            return;
        }

        // if this id is a bottom across section
        if ((w != 0 && w != level_width - 1) && h == 0)
        {
            level_grid[h, w] = level_bottom_across;
            return;
        }

        // if this id the bottom right corner section
        if ((w == (level_width - 1) && h == 0))
        {
            level_grid[h, w] = level_bottom_right_corner;
            return;
        }

        // if this id is a left up section
        if (w == 0 && h > 0 && h < (level_height - 1))
        {
            level_grid[h, w] = level_left_up;
            return;
        }

        // if this id is a right up section
        if (w == (level_width - 1) && h > 0 && h < (level_height - 1))
        {
            level_grid[h, w] = level_right_up;
            return;
        }

        // if this id the top left corner section
        if (w == 0 && h == (level_height - 1))
        {
            level_grid[h, w] = level_top_left_corner;
            return;
        }

        // if this id is a top across section
        if (w > 0 && w < (level_width - 1) && h == (level_height - 1))
        {
            level_grid[h, w] = level_top_across;
            return;
        }

        // if this id is a top across section
        if (w == (level_width - 1) && h == (level_height - 1))
        {
            level_grid[h, w] = level_top_right_corner;
            return;
        }

        // if its not a edge section, set it to a grid section
        // (Depots are added after)
        else
            level_grid[h, w] = 0;
    }


    /*
    private void GenerateDepotData()
    {
        int w = 0;
        int h = 0;

        int depot_count = 0;

        while (depot_count < total_no_depots)
        {
            // 1 and -1 to exclude Edge Sections
            h = Random.Range(1, level_height - 1);

            w = Random.Range(1, level_width - 1);

            // if this section is not already a Depot
            if (level_grid[h, w] != 1)
            {
                level_grid[h, w] = 1;
                depot_count++;
            }
        }
    }*/



    private void PopulateLevel()
    {
        Vector3 section_pos = Vector3.zero;

        int section_pos_x = grid_section_size / 2;
        int section_pos_z = grid_section_size / 2;

        for (int h = 0; h < level_height; h++)
        {
            for (int w = 0; w < level_width; w++)
            {
                int random_standard_section = Random.Range(0, standard_sections.Length);
                //int random_depot_section = Random.Range(0, depot_sections.Length);

                section_pos = new Vector3(section_pos_x, 0.0f, section_pos_z);

                // if section is inner city
                if (h > 0 && w > 0 && (h < level_height - 1) && (w < level_width - 1))
                {
                    // Grid section
                    if (level_grid[h, w] == 0)
                    {
                        Instantiate(standard_sections[random_standard_section], section_pos, standard_sections[random_standard_section].transform.rotation);
                    }

                    // Depot section
                    else if (level_grid[h, w] == 1)
                    {
                        //Instantiate(depot_sections[random_depot_section], section_pos, depot_sections[random_depot_section].transform.rotation);
                    }
                }

                // if this is an edge section
                if (h == 0 || w == 0 || (h == level_height - 1) || (w == level_width - 1))
                {
                    GenerateEdgeSection(h, w, section_pos);
                }

                section_pos_x += grid_section_size;
            }

            section_pos_x = grid_section_size / 2;
            section_pos_z += grid_section_size;
        }
    }


    private void GenerateEdgeSection(int h, int w, Vector3 section_pos)
    {
        for (int i = 0; i < edge_sections.Length; i++)
        {
            if (i == level_grid[h, w])
            {
                Instantiate(edge_sections[i], section_pos, edge_sections[i].transform.rotation);
            }
        }
    }



    public Vector3 GetLevelSize()
    {
        return new Vector3(level_width * grid_section_size, 0.0f, level_height * grid_section_size);
    }
}

