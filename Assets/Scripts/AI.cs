using System.Collections;
using System.Collections.Generic;
using System.Threading;
using static DrawArrow;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AIState
{
    RandomMovement,
    LookForTarget,
    MoveToTarget,
    HoverAroundTarget,
    Flee,
    returnInPlayerRange
}

public class AI : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)]
    public float movementSpeed = 3f;
    [SerializeField, Range(0f, 10f)]
    public float fleeSpeed = 6f;
    [SerializeField, Range(0f, 10f)]
    public float changeDirectionInterval = 2f;
    [SerializeField, Range(0f, 20f)]
    public float headToTargetInterval = 10f;
    [SerializeField, Range(0f, 10f)]
    private float modifyTargetInterval = 5f;
    [SerializeField, Range(0f, 10f)]
    private float lookForTargetInterval = 5f;
    [SerializeField, Range(0f, 10f)]
    public float fleeDistance = 5f;
    [SerializeField, Range(0f, 100f)]
    public float radiusAllowed = 20f;
    private GameObject currentTarget;
    public GameObject playerToFleeFrom;
    private float changeDirectionTimer;
    private float headTowardTargetTimer;
    private float modifyTargetTimer;
    private float lookForTargetTimer;
    private Vector3 actualDirection;
    private Vector3 randomDirection;
    private AIState currentState = AIState.RandomMovement;

    private Animator AIanim;

    void Awake()
    {
        AIanim = GetComponent<Animator>();
    }

    void Start()
    {
        changeDirectionTimer = changeDirectionInterval;
        headTowardTargetTimer = headToTargetInterval;
        modifyTargetTimer = modifyTargetInterval;
        lookForTargetTimer = lookForTargetInterval;
        actualDirection = new Vector3(0,0,0);
    }

    void Update()
    {
        List<PlayerMovements> players = GameManager.instance.playerMovements;
        PlayerMovements closestPlayer = null;
        float minDistance = float.MaxValue;

        foreach(PlayerMovements player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                closestPlayer = player;
            }
        }

        playerToFleeFrom = closestPlayer.gameObject;

        switch (currentState)
        {
            case AIState.RandomMovement:
                RandomMovement();
                AIanim.SetBool("flying", true);
                AIanim.SetBool("escaping", false);
                AIanim.SetBool("stole", false);
                AIanim.SetBool("found", false);
                AIanim.SetBool("search", false);
                break;
            case AIState.LookForTarget:
                LookForTarget();
                AIanim.SetBool("flying", false);
                AIanim.SetBool("escaping", false);
                AIanim.SetBool("stole", false);
                AIanim.SetBool("found", false);
                AIanim.SetBool("search", true);
                break;
            case AIState.MoveToTarget:
                MoveToTarget();
                AIanim.SetBool("flying", true);
                AIanim.SetBool("escaping", false);
                AIanim.SetBool("stole", false);
                AIanim.SetBool("found", false);
                AIanim.SetBool("search", false);
                break;
            case AIState.HoverAroundTarget:
                HoverAroundTarget();
                AIanim.SetBool("flying", false);
                AIanim.SetBool("escaping", false);
                AIanim.SetBool("found", true);
                AIanim.SetBool("search", false);
                break;
            case AIState.Flee:
                Flee();
                AIanim.SetBool("flying", false);
                AIanim.SetBool("escaping", true);
                AIanim.SetBool("stole", false);
                AIanim.SetBool("found", false);
                AIanim.SetBool("search", false);
                break;
            case AIState.returnInPlayerRange:
                returnInPlayerRange();
                AIanim.SetBool("flying", true);
                AIanim.SetBool("escaping", false);
                AIanim.SetBool("stole", false);
                AIanim.SetBool("found", false);
                AIanim.SetBool("search", false);
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
            currentState = AIState.LookForTarget;
        }

        // Check if player is too close
        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) <= fleeDistance)
        {
            changeDirectionTimer = changeDirectionInterval;
            headTowardTargetTimer = headToTargetInterval;
            currentState = AIState.Flee;
        }

        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) > radiusAllowed)
        {
            currentState = AIState.returnInPlayerRange;
        }
    }

    void LookForTarget()
    {
        lookForTargetTimer -= Time.deltaTime;

        if(lookForTargetTimer<= 0f)
        {
            lookForTargetTimer = lookForTargetInterval;
            SearchNewTarget();
        }

        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) <= fleeDistance)
        {
            lookForTargetTimer = lookForTargetInterval;
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
        modifyTargetTimer -= Time.deltaTime;
        // Calculate a random point within a 1 unit radius sphere around the target
        Vector3 above = new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y+1,currentTarget.transform.position.z);

        // Move towards the random point
        transform.position = Vector3.MoveTowards(transform.position, above, movementSpeed * Time.deltaTime);

        if (modifyTargetTimer <= 0f && !AIanim.GetBool("stole"))
        {
            StartCoroutine(PlayStoleAnimation());
        }

        // Check if player is too close
        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) <= fleeDistance)
        {
            modifyTargetTimer = modifyTargetInterval;
            currentState = AIState.Flee;
        }
    }


    void Flee()
    {
        // Flee from the player
        Vector3 fleeDirection = transform.position - playerToFleeFrom.transform.position;
        fleeDirection.y = 0f; // We don't want the AI to flee upwards
        randomDirection = fleeDirection.normalized;
        actualDirection = Vector3.Lerp(actualDirection,randomDirection, 0.1f);
        transform.Translate(actualDirection.normalized * fleeSpeed * Time.deltaTime);

        // Check if player is far enough
        if (Vector3.Distance(transform.position, playerToFleeFrom.transform.position) > fleeDistance * 2f)
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
            currentState = AIState.RandomMovement;
        }

        // Filter out targets based on distance to player and dream type
        List<Dream> validTargets = new List<Dream>();
        foreach (Dream target in targets)
        {
            // Check if the dream type is GOOD
            if (target.GetDreamType() == DreamType.GOOD)
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
            currentState = AIState.RandomMovement;
            return;
        }

        // Select a random target from the list of valid targets
        int randomIndex = Random.Range(0, validTargets.Count-1);
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
            return;
        }
    }

    IEnumerator PlayStoleAnimation()
    {
        AIanim.SetBool("stole", true);
        yield return new WaitForSeconds(0.3f);
        AIanim.SetBool("stole", false);
         modifyTargetTimer = modifyTargetInterval;
        Dream targetDream = currentTarget.GetComponent<Dream>();
        targetDream.setDreamType(DreamType.BAD);

        currentState = AIState.RandomMovement;
    }

    void OnDrawGizmos()
    {
        #if !UNITY_EDITOR
        ForGizmo(transform.position, actualDirection);
        ForGizmo(transform.position, randomDirection);
        #endif
        Gizmos.color = Color.cyan;
        if(playerToFleeFrom != null)
        Gizmos.DrawWireSphere(playerToFleeFrom.transform.position, radiusAllowed);

        Gizmos.color = Color.red;
        if (playerToFleeFrom != null)
            Gizmos.DrawWireSphere(playerToFleeFrom.transform.position, fleeDistance);
    }
}