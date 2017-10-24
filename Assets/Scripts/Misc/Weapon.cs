using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private WeaponFire weapon_fire;
    private Vector3 reset_pos;
    private GameObject target;

    private bool tracking_target;
    private bool target_lined_up;

    private float shot_timer;

    // Use this for initialization
    void Start ()
    {
        shot_timer = 2.0f;
        weapon_fire = GetComponentInChildren<WeaponFire>();
        target_lined_up = false;
        tracking_target = false;
        reset_pos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (tracking_target == true && target != null)
        {
            SmoothLook();

            if(TargetLinedUp() == true)
            {
                if (shot_timer > 2)
                {
                    weapon_fire.FireRound();
                    shot_timer = 0.0f;
                }
            }
        }

        shot_timer += Time.deltaTime;
	}


    public void SetTarget(GameObject _target)
    {
        target = _target;
        tracking_target = true;
    }


    public void ResetTarget()
    {
        transform.position = reset_pos;
        tracking_target = false;
    }



    private void SmoothLook()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion new_rot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, new_rot, Time.deltaTime * 5.0f);
    }



    private bool TargetLinedUp()
    {
        float angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);

        if (angle < 5.0f)
        {
            target_lined_up = true;
            return true;
        }

        target_lined_up = false;
        return false;
    }
}
