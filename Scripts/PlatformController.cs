using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformController : RaycastController {

    public LayerMask PassengerLayerMask;

    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public float speed;
    public bool cyclic;
    public float waitTime;
    [Range(0,2)]//clamps waitTime between 0 and 2
    public float easeAmount;

    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, PlayerController> passengerDictionary = new Dictionary<Transform, PlayerController>();
	// Use this for initialization
	public override void Start () {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];
        for(int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
        UpdateRaycastOrigins();

        Vector3 velocity = CalculatePlatformMovement();

        CalculatePassageMovement(velocity);

        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
	}

    float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 CalculatePlatformMovement()
    {
        if(Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }
        fromWaypointIndex %= globalWaypoints.Length;
        int toWayPointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWayPointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWayPointIndex], easedPercentBetweenWaypoints);

        if(percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if(!cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach(PassengerMovement passenger in passengerMovement)
        {
            if(!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<PlayerController>());
            }

            if(passenger.moveBeforePlatform == beforeMovePlatform)
            {
               passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    void CalculatePassageMovement(Vector3 velocity)
    {
        HashSet<Transform> movePassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertical moving platform
        if(velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for(int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, PassengerLayerMask);

                if (hit && hit.distance != 0)
                {
                    if(!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                    
                }
            }
        }

        //Horizontal moving platform
        if(velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for(int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, PassengerLayerMask);

                if (hit && hit.distance != 0)
                {
                    if(hit.distance == 0)
                    {
                        continue;// skips to the next ray.
                    }

                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }

                }
            }
        }

        // passenger is on top of a horizontally or descending moving platform.
        if(directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i); ;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, PassengerLayerMask);

                if (hit)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }

                }
            }
        }
    }

    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement (Transform _transform, Vector3 _velocity,bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    void OnDrawGizmos()
    {
        if(localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = 0.3f;

            for(int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointsPos = (Application.isPlaying)? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointsPos - Vector3.up * size, globalWaypointsPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointsPos - Vector3.left * size, globalWaypointsPos + Vector3.left * size);
            }
        }
    }
}
