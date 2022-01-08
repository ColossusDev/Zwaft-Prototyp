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
        movementScript.SetDestination(currentWaypoint.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (movementScript.destinationReached)
        {
            currentWaypoint = currentWaypoint.GetNextWaypoint();
            movementScript.SetDestination(currentWaypoint.GetPosition());
        }
    }
}
