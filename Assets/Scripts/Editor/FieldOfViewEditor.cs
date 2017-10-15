using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (AISight))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        AISight ai_sight = (AISight)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(ai_sight.transform.position, Vector3.up, Vector3.forward, 360, ai_sight.view_radius);
        Vector3 view_angle_a = ai_sight.DirectionFromAngle(-ai_sight.view_angle / 2, false);
        Vector3 view_angle_b = ai_sight.DirectionFromAngle(ai_sight.view_angle / 2, false);

        Handles.DrawLine(ai_sight.transform.position, ai_sight.transform.position + view_angle_a * ai_sight.view_radius);
        Handles.DrawLine(ai_sight.transform.position, ai_sight.transform.position + view_angle_b * ai_sight.view_radius);

        Handles.color = Color.red;
        foreach (Transform visible_target in ai_sight.visible_targets)
        {
            Handles.DrawLine(ai_sight.transform.position, visible_target.position);
        }

        Handles.color = Color.green;
        foreach (Transform visible_cover_pos in ai_sight.visible_cover_positions)
        {
            Handles.DrawLine(ai_sight.transform.position, visible_cover_pos.position);
        }
    }
}
