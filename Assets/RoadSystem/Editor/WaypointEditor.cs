using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[InitializeOnLoad()]
public class WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
    {
        if((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f; 
        }

        Gizmos.DrawSphere(waypoint.transform.position, .5f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2), waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

        if (waypoint.GetPrevWaypoint() != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
            Vector3 offsetTo = waypoint.GetPrevWaypoint().transform.right * waypoint.GetPrevWaypoint().width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.GetPrevWaypoint().transform.position + offsetTo);
        }

        if (waypoint.GetNextWaypoint() != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right * -waypoint.width / 2f;
            Vector3 offsetTo = waypoint.GetNextWaypoint().transform.right * -waypoint.GetNextWaypoint().width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.GetNextWaypoint().transform.position + offsetTo);
        }
    }
}
