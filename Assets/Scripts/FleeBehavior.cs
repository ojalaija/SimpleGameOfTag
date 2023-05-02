using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class FleeBehavior : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private float displacementDist = 5f;
    private float backToNormalRange = 30;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 8;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Move away from player

        agent.SetDestination(agent.transform.position + (agent.transform.position - player.position) * displacementDist);
        agent.isStopped = false;


        // Check for catching or being too far
        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance > backToNormalRange)
        {
            animator.SetBool("IsFleeing", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
