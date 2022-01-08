using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class BikePathScript : MonoBehaviour
    {
        public PathCreator path;
        public EndOfPathInstruction endOfPathInstruction;
        public float watt = 0;
        public float speed = 0;
        float distanceTravelled;
        [SerializeField] float distanceFromMiddle = 3;

        public Collider windCollider;
        public Collider BikeCollider;

        public bool windSchatten = false;
        public GameObject toFollow;

        void Start()
        {
            Debug.Log("Bike spawned");

            if (path != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                path.pathUpdated += OnPathChanged;
            }
        }

        public void SetWindschatten(GameObject bike)
        {
            toFollow = bike;
            windSchatten = true;
        }

        public void RemoveWindschatten()
        {
            toFollow = null;
            windSchatten = false;
        }

        private void ManagePostionOnRoad()
        {
            float realwatt = watt;

            if (windSchatten)
            {
                realwatt = watt * 1.3f;
            }
            speed = (realwatt * 1.5f) / 100;

            if(distanceFromMiddle > 2.1f)
            {
                distanceFromMiddle -= 0.01f;
            }
            else if(distanceFromMiddle < 1.9f)
            {
                distanceFromMiddle += 0.01f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.GetComponent<BikePathScript>().SetWindschatten(this.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            other.gameObject.GetComponent<BikePathScript>().RemoveWindschatten();
        }

        void Update()
        {
            ManagePostionOnRoad();

            if (path != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = path.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = path.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                transform.Translate(Vector3.right * distanceFromMiddle + Vector3.up * 1);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = path.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}