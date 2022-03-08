using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterCharacterAnimationStateController : MonoBehaviour
{
    enum ActionList
    {
        Idle,
        Walk,
        Run,
    }

    Animator animator;

    public float walkSpeed;
    public float runSpeed;
    Vector3 markedPosition;

    ActionList action;
    ActionList Action
    {
        get { return action; }

        set
        {
            action = value;
            //animator.SetInteger("Action", (int)action);
        }
    }

    bool isAiming;
    bool IsAiming
    {
        get { return isAiming; }

        set
        {
            isAiming = value;
            animator.SetBool("Aiming", isAiming);
        }
    }


    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        //Walk
        if (Input.GetButtonDown("Fire1"))
        {
            markedPosition = FindPosition();
            markedPosition.y = 0;

            if (Action != ActionList.Walk)
            {
                animator.SetTrigger("Walk");
                Action = ActionList.Walk;
            }
        }

        if (Action == ActionList.Walk)
        {
            MoveToMarkedPosition(walkSpeed);

            if (transform.position == markedPosition)
            {
                animator.SetTrigger("Idle");
                Action = ActionList.Idle;
            }
        }


        //Run
        if (Input.GetButtonDown("Fire2"))
        {
            markedPosition = FindPosition();
            markedPosition.y = 0;

            if (Action != ActionList.Run)
            {
                animator.SetTrigger("Run");
                Action = ActionList.Run;
            }
        }

        if (Action == ActionList.Run)
        {
            MoveToMarkedPosition(runSpeed);

            if (transform.position == markedPosition)
            {
                animator.SetTrigger("Idle");
                Action = ActionList.Idle;
            }
        }


        //Aiming
        if (Input.GetKeyDown(KeyCode.A))
        {
            IsAiming = !IsAiming;
            Debug.Log(IsAiming);
        }



    }

    private void MoveToMarkedPosition(float _speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, markedPosition, _speed * Time.deltaTime);
    }

    private Vector3 FindPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(hit.point);
        }

        return hit.point;
    }
}
