using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class BikePathScript : MonoBehaviour
    {
        public PathCreator path;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 0;
        float distanceTravelled;
        float distanceFromMiddle = 1;

        void Start()
        {
            if (path != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                path.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
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