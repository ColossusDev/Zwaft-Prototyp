using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint prevWaypoint;
    [SerializeField] private Waypoint nextWaypoint;

    [SerializeField] private Waypoint prevAlternativWaypoint;
    [SerializeField] private Waypoint nextAlternativWaypoint;

    public float width = 10f;

    public float elevation;

    public Vector3 GetPosition(bool direction, MovementScript agent)
    {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        if (direction)
        {
            if (agent.crossPosition > 0.5f || agent.crossPosition < 0.01f)
            {
                float crossing = Random.Range(0f, 0.4f);
                agent.crossPosition = crossing;
            }
            return Vector3.Lerp(minBound, maxBound, agent.crossPosition);
        }
        else
        {
            if (agent.crossPosition > 1f || agent.crossPosition < 0.5f)
            {
                float crossing = Random.Range(0.6f, 1f);
                agent.crossPosition = crossing;
            }
            return Vector3.Lerp(minBound, maxBound, agent.crossPosition);
        }
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
        return nextWaypoint;
    }
    public void SetNextWaypoint(Waypoint wp)
    {
        nextWaypoint = wp;
    }
    public Waypoint GetPrevAlternativWaypoint()
    {
        return prevAlternativWaypoint;
    }
    public Waypoint GetNextAlternativWaypoint()
    {
        return nextAlternativWaypoint;
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
