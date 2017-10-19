using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISight : MonoBehaviour
{
    public float view_radius;
    [Range(0,360)]
    public float view_angle;

    public LayerMask opposition_mask;
    public LayerMask obstacle_mask;
    public LayerMask cover_pos_mask;

    public List<Transform> visible_targets = new List<Transform>();
    public List<Transform> visible_cover_positions = new List<Transform>();

    public CoverManager cover_manager;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    IEnumerator FindTargetsWithDelay(float _delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(_delay);
            FindVisibleTargets();
        }
    }



    void FindVisibleTargets()
    {
        TargetCheck();
        CoverPositionsCheck();
    }



    void TargetCheck()
    {
        visible_targets.Clear();

        // Create a list of all objects in view radius
        Collider[] targets_in_view_radius = Physics.OverlapSphere(transform.position, view_radius, opposition_mask);

        // loop through them to see which can be seen
        for (int i = 0; i < targets_in_view_radius.Length; i++)
        {
            Transform target = targets_in_view_radius[i].transform;

            Vector3 dir_to_target = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dir_to_target) < view_angle / 2)
            {
                float dist_to_target = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dir_to_target, dist_to_target, obstacle_mask))
                {
                    visible_targets.Add(target);
                }
            }
        }
    }



    void CoverPositionsCheck()
    {
        visible_cover_positions.Clear();

        Collider[] cover_positions_in_view_raduis = Physics.OverlapSphere(transform.position, view_radius, cover_pos_mask);

        // loop through them to see which can be seen
        for (int i = 0; i < cover_positions_in_view_raduis.Length; i++)
        {
            Transform cover_pos = cover_positions_in_view_raduis[i].transform;

            Vector3 dir_to_target = (cover_pos.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dir_to_target) < view_angle / 2)
            {
                float dist_to_target = Vector3.Distance(transform.position, cover_pos.position);

                if (!Physics.Raycast(transform.position, dir_to_target, dist_to_target, obstacle_mask))
                {
                    visible_cover_positions.Add(cover_pos);
                }
            }
        }
    }



    public Vector3 DirectionFromAngle(float _angle_in_degrees, bool _angle_is_global)
    {
        if(!_angle_is_global)
        {
            _angle_in_degrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(_angle_in_degrees * Mathf.Deg2Rad), 0.0f, Mathf.Cos(_angle_in_degrees * Mathf.Deg2Rad));
    }



    public List<Transform> GetVisibleCoverPoints()
    {
        return visible_cover_positions;
    }
}
