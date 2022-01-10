using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{

    public int bikeAmount = 5;
    public GameObject prefabDummy;
    public Waypoint startWaypoint;

    public float secondsBetweenBikes = 1f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (bikeAmount >= 1)
            {
                GameObject go = Instantiate(prefabDummy);
                go.GetComponent<MovementScript>().movementSpeed = Random.Range(15f, 25f);
                int rng = Random.Range(0, 2);
                if (rng==0)
                {
                    go.GetComponent<MovementScript>().richtung = false;
                }
                else
                {
                    go.GetComponent<MovementScript>().richtung = true;
                }
                go.GetComponent<NavigatorScript>().currentWaypoint = startWaypoint;
                bikeAmount -= 1;
            }
            timer = secondsBetweenBikes;
        }
    }
}
