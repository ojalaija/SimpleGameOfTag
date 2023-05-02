using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PauseBehavior : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private float timer;
    private float chaseRange = 15;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // A little pause to give player time to start running
        agent.SetDestination(agent.transform.position);

        timer += Time.deltaTime;
        float distance = Vector3.Distance(agent.transform.position, player.position);

        if (timer > 2)
        {
            if (distance < chaseRange && animator.GetBool("IsChaser") == true)
            {
                animator.SetBool("IsChasing", true);
            }
            else
            {
                animator.SetBool("IsPatrolling", true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
