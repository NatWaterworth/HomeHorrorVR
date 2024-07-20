using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NavigationController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    public float jumpDistance = 2f; // Distance within which Teddy will jump between points
    public float jumpHeight = 2f; // Height of the jump

    private int currentWaypointIndex = 0;
    private bool isJumping = false;

    public void EnableNavigation()
    {
        navMeshAgent.enabled = true;
        MoveToNextWaypoint();
    }

    public void DisableNavigation()
    {
        navMeshAgent.enabled = false;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0 || !navMeshAgent.enabled) return;

        Vector3 destination = waypoints[currentWaypointIndex].position;
        Vector3 direction = destination - transform.position;
        float distance = direction.magnitude;

        if (distance <= jumpDistance && !IsPathFullyConnected(transform.position, destination))
        {
            StartCoroutine(JumpTo(destination));
        }
        else
        {
            navMeshAgent.SetDestination(destination);
        }

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    bool IsPathFullyConnected(Vector3 start, Vector3 end)
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(end, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    IEnumerator JumpTo(Vector3 destination)
    {
        isJumping = true;
        navMeshAgent.enabled = false;

        Vector3 startPos = transform.position;
        float jumpProgress = 0f;
        while (jumpProgress < 1f)
        {
            jumpProgress += Time.deltaTime / 1f; // 1 second jump duration
            float heightOffset = Mathf.Sin(Mathf.PI * jumpProgress) * jumpHeight;
            transform.position = Vector3.Lerp(startPos, destination, jumpProgress) + Vector3.up * heightOffset;
            yield return null;
        }

        transform.position = destination;
        navMeshAgent.enabled = true;
        isJumping = false;
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (navMeshAgent.enabled && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            MoveToNextWaypoint();
        }
    }
}
