using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasBehavior : MonoBehaviour
{
    [SerializeField] private GameObject chase;
    [SerializeField] private GameObject run;
    [SerializeField] private GameObject startInstructions;
    [SerializeField] private TextMeshProUGUI state;
    [SerializeField] private GameObject enemy;

    private Animator enemyAnimator;
    private string playerState = "Runner";

    private void OnEnable()
    {
        EventManager.ChangeRoles += OnEventChangeRoles;
    }

    private void Start()
    {
        enemyAnimator = enemy.GetComponent<Animator>();
        state.text = "You are: " + playerState;
        startInstructions.SetActive(true);
        Invoke("CloseInstructions", 9f);
    }

    private void Update()
    {
        state.text = "You are: " + playerState;
    }
    private void OnEventChangeRoles()
    {
        if (enemyAnimator.GetBool("IsChaser") == false)
        {
            playerState = "Runner";

            if (chase.activeSelf) CloseChase();
            if (startInstructions.activeSelf) CloseInstructions();

            run.SetActive(true);
            Invoke("CloseRun", 1f);
        }
        else
        {
            playerState = "Chaser";

            if (run.activeSelf) CloseRun();
            if (startInstructions.activeSelf) CloseInstructions();

            chase.SetActive(true);
            Invoke("CloseChase", 1f);
        }
    }

    private void CloseRun()
    {
        run.SetActive(false);
    }
    private void CloseChase()
    {
        chase.SetActive(false);
    }

    private void CloseInstructions()
    {
        startInstructions.SetActive(false);
    }

    private void OnDisable()
    {
        EventManager.ChangeRoles -= OnEventChangeRoles;
    }
}
