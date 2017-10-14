using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemy_prefab;

    private List<Transform> spawn_positions;

	void Start ()
    {
        spawn_positions = new List<Transform>();

        var spawn_points = GameObject.FindGameObjectsWithTag("Enemy Spawn Point");

        foreach(GameObject spawn_point in spawn_points)
        {
            spawn_positions.Add(spawn_point.transform);
        }
	}
	


	// Update is called once per frame
	void Update ()
    {

	}



    public void GenerateEnemies(int _no_enemies, List<Vector3> _enemy_positions)
    {
        for (int i = 0; i < _no_enemies; i++)
        {
            Instantiate(enemy_prefab, _enemy_positions[i], enemy_prefab.transform.rotation);
        }
    }



    public List<Transform> GetSpawnPoints()
    {
        return spawn_positions;
    }
}
