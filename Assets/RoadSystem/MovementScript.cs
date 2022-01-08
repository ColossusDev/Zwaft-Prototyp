using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float movementSpeed = 1;
    public float rotationSpeed = 120;
    public float stopDistance = 2.5f;
    public Vector3 destination = new Vector3(5, 5, 5);
    public bool destinationReached = false; 

    private void Update()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;

            float destinationDistance = destinationDirection.magnitude;

            if(destinationDistance >= stopDistance)
            {
                destinationReached = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
            else
            {
                destinationReached = true;
            }
        }
    }



    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        destinationReached = false;

    }



   
}
