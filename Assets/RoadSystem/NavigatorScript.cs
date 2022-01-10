using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatorScript : MonoBehaviour
{
    MovementScript movementScript;
    public Waypoint currentWaypoint;
    bool changedRoad = false;

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
            int rngRoadChange = 0;
            if (!changedRoad && currentWaypoint.crossing)
            {
                rngRoadChange = Random.Range(0, 2);
                if (rngRoadChange == 1)
                {
                    currentWaypoint = currentWaypoint.GetConnectiongRoadWaypoint();
                    changedRoad = true;
                }
            }
            if(rngRoadChange == 0 && movementScript.richtung)
            {
                currentWaypoint = currentWaypoint.GetNextWaypoint();
                changedRoad = false;
            }
            else if(rngRoadChange == 0)
            {
                currentWaypoint = currentWaypoint.GetPrevWaypoint();
                changedRoad = false;
            }

            movementScript.SetDestination(currentWaypoint.GetPosition(movementScript.richtung, movementScript));
        }
    }
}
