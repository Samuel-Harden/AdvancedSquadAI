using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float health = 10.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(health < 0)
        {
            Destroy(gameObject);
        }
	}



    public void DamageHealth(float _damage)
    {
        Debug.Log("Taking Damage");
        health -= _damage;
    }
}
