using UnityEngine;
using System.Collections;


public class PlayerController : RaycastController {

    PlayerInput inputClass;

    float maxClimbingAngle = 80;
    float maxDescendAngle = 75;

    public CollisionInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;

    public override void Start()
    {
        if (GameControl.Instance.transitionTarget != null)
        {
            gameObject.transform.position = GameControl.Instance.transitionTarget.position;
        }

        if (GameControl.Instance.isSceneBeingLoaded)
        {
            PlayerState.Instance.localPlayerData = SaveLists.Instance.localCopyOfData;

            transform.position = new Vector3(SaveLists.Instance.localCopyOfData.PosX, SaveLists.Instance.localCopyOfData.PosX, transform.position.z);
        }

        inputClass = GetComponent<PlayerInput>();

        base.Start();
    }

    public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 moveAmount,Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.moveAmountOld = moveAmount;
        playerInput = input;

        if(moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount);
        }

        if(moveAmount.x != 0)
        {
            HorizontalCollision(ref moveAmount);
        }
        if (moveAmount.y != 0)
        {
            VerticalCollision(ref moveAmount);
        }
        
        transform.Translate(moveAmount);

        if(standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    void HorizontalCollision(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x);
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayerMaskHorizontal);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                if(i == 2 && hit.transform.tag == "NPCText")
                {
                    if(!DialogueController.Instance.curInDialogue)
                    {
                        print("not in dialogue");
                        if (inputClass.CheckAttack())
                        {
                            int conversationId = hit.transform.GetComponent<TextNPCTest>().conversationID;

                            DialogueController.Instance.StartDialogue(conversationId);
                        }
                        continue;
                    }
                }

                if (hit.transform.tag == "NPCText")
                {
                    continue;
                }

                    #region FLOOR&WALL_COLLISIONS

                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if(i == 0 && slopeAngle <= maxClimbingAngle)
                {
                    if(collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }

                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope(ref moveAmount, slopeAngle);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if(!collisions.climbingSlope || slopeAngle > maxClimbingAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if(collisions.climbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }

                #endregion
            }
        }
    }

    void VerticalCollision(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength =  Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayerMaskVertical);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if(hit)
            {
                if(hit.collider.tag == "JumpThrough")
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;//go through the next ray in the for loop
                    }
                    if(collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if(playerInput.y == -1)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.5f);
                        continue;//go through the next ray in the loop
                    }
                }

                if (hit.transform.tag == "NPCText")
                {
                    continue;
                }

                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if(collisions.climbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if(collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayerMaskHorizontal);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                moveAmount.x = (hit.distance - skinWidth) * directionX;
                collisions.slopeAngle = slopeAngle;
            }
        }
    }

    void ClimbSlope(ref Vector2 moveAmount, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if(moveAmount.y < climbmoveAmountY)
        { 
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);

            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector2 moveAmount)
    {
        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionLayerMaskHorizontal);

        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                    {
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float DescendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y -= DescendmoveAmountY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public Vector2 moveAmountOld;

        public float slopeAngle, slopeAngleOld;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
	
}
