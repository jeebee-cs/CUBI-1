using System.Collections;
using System.Collections.Generic;
using System.Threading;
using static DrawArrow;
using UnityEngine;

public enum AIState
{
    RandomMovement,
    MoveToTarget,
    HoverAroundTarget,
    Flee,
    returnInPlayerRange
}

public class AI : MonoBehaviour
{
    public float movementSpeed = 3f;
    public float fleeSpeed = 6f;
    public float changeDirectionInterval = 2f;
    public float headToTargetInterval = 10f;
    public float fleeDistance = 5f;
    public float radiusAllowed = 20f;
    private GameObject currentTarget;
    public GameObject playerToFleeFrom;
    private float changeDirectionTimer;
    private float headTowardTargetTimer;
    private Vector3 actualDirection;
    private Vector3 randomDirection;
    private AIState currentState = AIState.RandomMovement;

    void Start()
    {
        changeDirectionTimer = changeDirectionInterval;
        headTowardTargetTimer = headToTargetInterval;
        actualDirection = new Vector3(0,0,0);
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.RandomMovement:
                RandomMovement();
                break;
            case AIState.MoveToTarget:
                MoveToTarget();
                break;
            case AIState.HoverAroundTarget:
                HoverAroundTarget();
                break;
            case AIState.Flee:
                Flee();
                break;
            case AIState.returnInPlayerRange:
                returnInPlayerRange();
                break;
        }
    }

    public void SetRadiusAllowed(float newRadius)
    {
        radiusAllowed = newRadius;
    }

    void RandomMovement()
    {
        // Smoothly transition from the current direction to the random direction
        actualDirection = Vector3.Lerp(actualDirection,randomDirection, changeDirectionInterval*Time.deltaTime);
        transform.Translate(actualDirection * movementSpeed * Time.deltaTime);

        // Update timers
        changeDirectionTimer -= Time.deltaTime;
        headTowardTargetTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            // change direction every 2 seconds
            actualDirection = randomDirection;
            GetRandomDirection();
            changeDirectionTimer = changeDirectionInterval;
        }

        if (headTowardTargetTimer <= 0f)
        {
            //each 10 seconds, the AI will chose a new target and head toward it
            headTowardTargetTimer = headToTargetInterval;
            SearchNewTarget();
        }

        // Check if player is too close
        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) <= fleeDistance)
        {
            currentState = AIState.Flee;
        }

        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) > radiusAllowed)
        {
            currentState = AIState.returnInPlayerRange;
        }
    }

    void MoveToTarget()
    {
        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, movementSpeed * Time.deltaTime);

        // Check if reached the target
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= 1f)
        {
            currentState = AIState.HoverAroundTarget;
        }
    }

    void HoverAroundTarget()
    {
        // Calculate a random point within a 1 unit radius sphere around the target
        Vector3 randomPoint = Random.insideUnitSphere * 1f + currentTarget.transform.position;

        // Move towards the random point
        transform.position = Vector3.MoveTowards(transform.position, randomPoint, movementSpeed * Time.deltaTime);

        // Check if player is too close
        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) <= fleeDistance)
        {
            currentState = AIState.Flee;
        }
    }


    void Flee()
    {
        // Flee from the player
        Vector3 fleeDirection = transform.position - playerToFleeFrom.transform.position;
        fleeDirection.y = 0f; // We don't want the AI to flee upwards
        transform.Translate(fleeDirection.normalized * fleeSpeed * Time.deltaTime);

        // Check if player is far enough
        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) > fleeDistance * 1.5f)
        {
            currentState = AIState.RandomMovement;
        }
    }

    void GetRandomDirection()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void SearchNewTarget()
    {
        // Find all objects in the scene with the specified component
        Dream[] targets = GameObject.FindObjectsOfType<Dream>();

        // If there are no targets, return
        if (targets.Length == 0)
        {
            return;
        }

        // Filter out targets based on distance to player and dream type
        List<Dream> validTargets = new List<Dream>();
        foreach (Dream target in targets)
        {
            // Check if the dream type is GOOD
            if (target.dreamType == DreamType.GOOD)
            {
                float distanceToPlayer = Vector3.Distance(target.transform.position, playerToFleeFrom.transform.position);

                // Check if the distance is within the allowed radius - valid dreams are only targeted if in the action range of the AI
                if (distanceToPlayer <= radiusAllowed)
                {
                    validTargets.Add(target);
                }
            }
        }

        // If there are no valid targets, return
        if (validTargets.Count == 0)
        {
            return;
        }

        // Select a random target from the list of valid targets
        int randomIndex = Random.Range(0, validTargets.Count);
        currentTarget = validTargets[randomIndex].gameObject;
        currentState = AIState.MoveToTarget;
    }


    void returnInPlayerRange()
    {
        Vector3 returnDirection = playerToFleeFrom.transform.position - transform.position;
        randomDirection = returnDirection.normalized;
        actualDirection = Vector3.Lerp(actualDirection,randomDirection, 0.1f);
        transform.Translate(actualDirection * movementSpeed * Time.deltaTime);

        // Check if player is far enough
        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) < radiusAllowed)
        {
            currentState = AIState.RandomMovement;
        }
    }

    void OnDrawGizmos()
    {
        ForGizmo(transform.position, actualDirection);
        ForGizmo(transform.position, randomDirection);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerToFleeFrom.transform.position, radiusAllowed);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerToFleeFrom.transform.position, fleeDistance);
    }
}
