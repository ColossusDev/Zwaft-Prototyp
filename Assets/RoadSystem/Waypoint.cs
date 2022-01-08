using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint prevWaypoint;
    [SerializeField] private Waypoint nextWaypoint;

    [Range(4f, 10f)]
    public float width = 10f;

    public float elevation;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0.3f, 0.7f));
    }

    public Waypoint GetPrevWaypoint()
    {
        return prevWaypoint;
    }
    public void SetPrevWaypoint(Waypoint wp)
    {
        prevWaypoint = wp;
    }
    public Waypoint GetNextWaypoint()
    {
        return prevWaypoint;
    }
    public void SetNextWaypoint(Waypoint wp)
    {
        nextWaypoint = wp;
    }
    public void CalculateAllValues()
    {
        if (nextWaypoint != null)
        {
            float yDiff = nextWaypoint.transform.position.y - transform.position.y;
            elevation = yDiff;
        }
    }
}
