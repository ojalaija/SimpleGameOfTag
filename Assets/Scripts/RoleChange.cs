using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleChange : MonoBehaviour
{
    private Animator animator;

    private void OnEnable()
    {
        EventManager.ChangeRoles += OnEventChangeRoles;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEventChangeRoles()
    {
        Debug.Log("Changing roles");
        if (animator.GetBool("IsChaser") == false)
        {
            animator.SetBool("IsChaser", true);
            animator.SetBool("IsFleeing", false);
        }
        else
        {
            animator.SetBool("IsChaser", false);
            animator.SetBool("IsChasing", false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (animator.GetBool("IsChaser") == true)
            {
                EventManager.OnChangeRoles();

                animator.SetBool("IsFleeing", true);
                animator.SetBool("IsPatrolling", false);
            }
            else
            {
                EventManager.OnChangeRoles();
            }
        }
    }

    private void OnDisable()
    {
        EventManager.ChangeRoles -= OnEventChangeRoles;
    }

}
