using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NavigationController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    public Animator animator;
    public float maxJumpHeight = 2f; // Maximum height of the jump
    public float minJumpHeight = 0.5f; // Minimum height of the jump to avoid clipping
    public float jumpDuration = 1f; // Duration of the jump
    public float preJumpPause = 0.5f; // Pause before jump
    public float postJumpPause = 0.5f; // Pause after jump
    public float jumpHeightOffset = 0.5f; // Extra height to avoid clipping

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

        // Select a random waypoint as the next destination
        Vector3 destination = waypoints[Random.Range(0, waypoints.Length)].position;
        navMeshAgent.SetDestination(destination);
    }

    IEnumerator JumpAcrossLink(Vector3 endPos)
    {
        isJumping = true;
        navMeshAgent.enabled = false;

        // Pre-jump pause
        animator.SetBool("Jump", true);
        yield return new WaitForSeconds(preJumpPause);

        // Calculate the required jump height
        Vector3 startPos = transform.position;
        float heightDifference = endPos.y - startPos.y;
        float actualJumpHeight = Mathf.Max(minJumpHeight, Mathf.Min(maxJumpHeight, heightDifference + jumpHeightOffset));

        // Jump
        float elapsedTime = 0f;
        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpDuration;
            float heightOffset = Mathf.Sin(Mathf.PI * t) * actualJumpHeight;
            transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * heightOffset;
            yield return null;
        }

        transform.position = endPos;
        animator.SetBool("Jump", false);

        // Post-jump pause
        yield return new WaitForSeconds(postJumpPause);

        navMeshAgent.enabled = true;
        isJumping = false;
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
