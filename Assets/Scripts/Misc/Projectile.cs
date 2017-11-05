using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float bullet_speed = 5.0f;
    public float damage = 5.0f;
    public float life_span = 8.0f;
    private float despawn_timer;

	// Use this for initialization
	void Start ()
    {
        despawn_timer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bullet_speed);

        despawn_timer += Time.deltaTime;

        if(despawn_timer > life_span)
        {
            Destroy(gameObject);
        }
	}



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Squadie")
        {
            other.GetComponent<AIController>().DamageHealth(damage);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            //other.GetComponent<BlahBlah>().DamageHealth(damage);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<TurretAI>().DamageHealth(damage);
            Destroy(gameObject);
        }
    }
}
