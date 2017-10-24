using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    public GameObject bullet;
    public Transform bullet_spawn_point;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    public void FireRound()
    {
        Instantiate(bullet, bullet_spawn_point.position, bullet_spawn_point.rotation);
    }
}
