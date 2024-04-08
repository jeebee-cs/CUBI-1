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
    Flee
}

public class AI : MonoBehaviour
{
    public float movementSpeed = 3f;

    public float fleeSpeed = 6f;
    public float changeDirectionInterval = 2f;
    public float headToTargetInterval = 10f;
    public float fleeDistance = 5f;
    public GameObject currentTarget;
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
        }
    }

    void RandomMovement()
    {
         float interpolationFactor = 1.0f - Mathf.Pow(1.0f - (changeDirectionTimer / changeDirectionInterval), 2f);

    // Smoothly transition from the current direction to the random direction
        actualDirection = Vector3.Lerp(randomDirection, actualDirection, interpolationFactor);
        transform.Translate(actualDirection * movementSpeed * Time.deltaTime);

        // Update timer
        changeDirectionTimer -= Time.deltaTime;
        headTowardTargetTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            actualDirection = randomDirection;
            GetRandomDirection();
            changeDirectionTimer = changeDirectionInterval;
        }

        if (headTowardTargetTimer <= 0f)
        {
            headTowardTargetTimer = headToTargetInterval;
            SearchNewTarget();
        }

        // Check if player is too close
        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) <= fleeDistance)
        {
            currentState = AIState.Flee;
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

        // Select a random target from the list
        int randomIndex = Random.Range(0, targets.Length);
        currentTarget = targets[randomIndex].gameObject;
        currentState = AIState.MoveToTarget;
    }

    void OnDrawGizmos()
    {
        ForGizmo(transform.position, actualDirection);
        ForGizmo(transform.position, randomDirection);
    }
}
