using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum CharacterState
{
    Idle,
    Moving,
    Jumping,
    Screaming
}

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
    public float minIdleTime = 2f; // Minimum idle time
    public float maxIdleTime = 5f; // Maximum idle time

    private Vector3 lastPosition;
    private float moveSpeed;
    private bool isJumping = false;
    private CharacterState currentState;
    private bool isStateRoutineRunning = false;
    private Coroutine stateMachineCoroutine;

    public void Setup(Animator animator)
    {
        this.animator = animator;
        currentState = CharacterState.Idle;
    }

    public void EnableNavigation()
    {
        if (stateMachineCoroutine == null)
        {
            navMeshAgent.enabled = true;
            stateMachineCoroutine = StartCoroutine(StateMachine());
        }
    }

    public void DisableNavigation()
    {
        if (stateMachineCoroutine != null)
        {
            StopCoroutine(stateMachineCoroutine);
            stateMachineCoroutine = null;
            navMeshAgent.enabled = false;
            animator.SetFloat("MoveSpeed", 0);
        }
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    private IEnumerator Idle()
    {
        isStateRoutineRunning = true;
        animator.SetBool("Idle", true);
        float idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        animator.SetBool("Idle", false);
        ChangeState(CharacterState.Moving);
        isStateRoutineRunning = false;
    }

    private IEnumerator Moving()
    {
        isStateRoutineRunning = true;
        MoveToNextWaypoint();
        while (navMeshAgent.enabled && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            yield return null;
        }
        if (navMeshAgent.isOnOffMeshLink)
        {
            ChangeState(CharacterState.Jumping);
        }
        else
        {
            // Decide whether to idle or scream next
            if (Random.value < 0.5f)
            {
                ChangeState(CharacterState.Idle);
            }
            else
            {
                ChangeState(CharacterState.Screaming);
            }
        }
        isStateRoutineRunning = false;
    }

    private IEnumerator Jumping()
    {
        isStateRoutineRunning = true;
        Vector3 endPos = navMeshAgent.currentOffMeshLinkData.endPos;
        yield return StartCoroutine(JumpAcrossLink(endPos));
        navMeshAgent.CompleteOffMeshLink();
        ChangeState(CharacterState.Moving);
        isStateRoutineRunning = false;
    }

    private IEnumerator Screaming()
    {
        isStateRoutineRunning = true;
        animator.SetTrigger("Scream");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        ChangeState(CharacterState.Moving);
        isStateRoutineRunning = false;
    }

    private void ChangeState(CharacterState newState)
    {
        if (!isStateRoutineRunning)
        {
            currentState = newState;
        }
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
        float actualJumpHeight = Mathf.Max(minJumpHeight, Mathf.Min(maxJumpHeight, Mathf.Abs(heightDifference) + jumpHeightOffset));

        // Set the animator parameter for the jump height
        animator.SetFloat("JumpHeight", heightDifference);

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
    }

    void Update()
    {
        if (navMeshAgent.enabled)
        {
            // Calculate move speed based on change in position
            moveSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
            lastPosition = transform.position;

            // Update the animator with the move speed
            animator.SetFloat("MoveSpeed", moveSpeed);
        }
    }
}
