using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehavior : StateMachineBehaviour
{
    private float timer;
    private List<Transform> waypoints = new List<Transform>();
    private NavMeshAgent agent;

    private Transform player;
    private float chaseRange = 15;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        DetermineWaypoints(animator);
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);
        agent.speed = 2.5f;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Determine a new waypoint
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);
        }

        // Change to idle
        timer += Time.deltaTime;
        if (timer > 10)
        {
            animator.SetBool("IsPatrolling", false);
        }

        // Change to chasing or fleeing if in radius
        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance < chaseRange && animator.GetBool("IsChaser") == true)
        {
            animator.SetBool("IsChasing", true);
        }
        if (distance < chaseRange && animator.GetBool("IsChaser") == false)
        {
            animator.SetBool("IsFleeing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    private void DetermineWaypoints(Animator animator)
    {
        Transform wayPointsObject = null;

        if (animator.gameObject.CompareTag("enemy1")) wayPointsObject = GameObject.FindGameObjectWithTag("WP1").transform;
        if (animator.gameObject.CompareTag("enemy2")) wayPointsObject = GameObject.FindGameObjectWithTag("WP2").transform;
        if (animator.gameObject.CompareTag("enemy3")) wayPointsObject = GameObject.FindGameObjectWithTag("WP3").transform;

        if (wayPointsObject != null)
        {
            foreach (Transform t in wayPointsObject.transform)
            {
                waypoints.Add(t);
            }
        }
    }
}
