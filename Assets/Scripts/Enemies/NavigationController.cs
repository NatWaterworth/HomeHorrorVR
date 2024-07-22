using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NavigationController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    public Animator animator;
    public float jumpHeight = 2f; // Maximum height of the jump
    public float jumpDuration = 1f; // Duration of the jump

    private int currentWaypointIndex = 0;
    private bool isJumping = false;

    public void Setup(Animator animator)
    {
        this.animator = animator;
    }

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
        navMeshAgent.SetDestination(destination);

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    IEnumerator JumpAcrossLink(Vector3 endPos)
    {
        isJumping = true;
        navMeshAgent.enabled = false;
        animator.SetBool("Jump", true);

        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpDuration;
            float heightOffset = Mathf.Sin(Mathf.PI * t) * jumpHeight;
            transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * heightOffset;
            yield return null;
        }

        transform.position = endPos;
        navMeshAgent.enabled = true;
        isJumping = false;
        animator.SetBool("Jump", false);
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (navMeshAgent.isOnOffMeshLink)
        {
            StartCoroutine(JumpAcrossLink(navMeshAgent.currentOffMeshLinkData.endPos));
            navMeshAgent.CompleteOffMeshLink();
        }
        else if (navMeshAgent.enabled && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            MoveToNextWaypoint();
        }
    }
}
