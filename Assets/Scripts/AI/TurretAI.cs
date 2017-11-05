using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public float health = 10.0f;

    private List<GameObject> threats;
    private GameObject current_threat;

    private Weapon weapon;

    // Use this for initialization
    void Start()
    {
        weapon = GetComponent<Weapon>();
        threats = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            Destroy(transform.parent.gameObject);
        }

        EngageTarget();
    }



    private void EngageTarget()
    {
        if (current_threat != null)
        {
            weapon.FireWeapon();
        }
    }



    public void UpdateThreats(List<GameObject> _threats)
    {
        threats = _threats;
        float distance;

        if (current_threat == null)
        {
            current_threat = _threats[0];
        }

        distance = Vector3.Distance(transform.position, current_threat.transform.position);

        for (int i = 0; i < threats.Count; i++)
        {
            if (Vector3.Distance(transform.position, threats[i].transform.position) < distance)
            {
                current_threat = threats[i];
            }
        }

        weapon.SetTarget(current_threat);
    }



    public void DamageHealth(float _damage)
    {
        Debug.Log("Taking Damage");
        health -= _damage;
    }
}
