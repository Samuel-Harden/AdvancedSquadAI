using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Vector3 reset_pos;
    private GameObject target;

    private bool tracking_target;
    private bool target_lined_up;

    private float shot_timer;

    public GameObject bullet;
    public Transform bullet_spawn_point;
    [SerializeField] float rot_speed = 5.0f;
    [SerializeField] float fire_rate = 2.0f;

    // Use this for initialization
    void Start ()
    {
        shot_timer = 2.0f;
        target_lined_up = false;
        tracking_target = false;
        reset_pos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {

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
        transform.rotation = Quaternion.Slerp(transform.rotation, new_rot, Time.deltaTime * rot_speed);
    }



    private bool TargetLinedUp()
    {
        float angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);

        if (angle < 2.0f)
        {
            target_lined_up = true;
            return true;
        }

        target_lined_up = false;
        return false;
    }



    public void FireWeapon()
    {
        if (tracking_target == true && target != null)
        {
            SmoothLook();

            if (TargetLinedUp() == true)
            {
                if (shot_timer > fire_rate)
                {
                    Instantiate(bullet, bullet_spawn_point.position, bullet_spawn_point.rotation);

                    shot_timer = 0.0f;
                }
            }
        }

        shot_timer += Time.deltaTime;
    }
}
