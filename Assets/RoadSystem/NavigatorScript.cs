using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatorScript : MonoBehaviour
{
    MovementScript movementScript;
    public Waypoint currentWaypoint;

    private void Awake()
    {
        movementScript = GetComponent<MovementScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponent<MovementScript>();
        movementScript.SetDestination(currentWaypoint.GetPosition(movementScript.richtung, movementScript));
    }

    // Update is called once per frame
    void Update()
    {
        if (movementScript.destinationReached)
        {
            if (movementScript.richtung)
            {
                currentWaypoint = currentWaypoint.GetNextWaypoint();
                movementScript.SetDestination(currentWaypoint.GetPosition(movementScript.richtung, movementScript));
            }
            else
            {
                currentWaypoint = currentWaypoint.GetPrevWaypoint();
                movementScript.SetDestination(currentWaypoint.GetPosition(movementScript.richtung, movementScript));
            }
            
        }
    }
}
