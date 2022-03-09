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
        Crouch
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

            if (isAiming)
            {
                switch (Action)
                {
                    case ActionList.Idle:
                        animator.SetTrigger("IdleAiming");
                        break;
                    case ActionList.Walk:
                        animator.SetTrigger("WalkAiming");
                        break;
                    case ActionList.Run:
                        animator.SetTrigger("RunAiming");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (Action)
                {
                    case ActionList.Idle:
                        animator.SetTrigger("Idle");
                        break;
                    case ActionList.Walk:
                        animator.SetTrigger("Walk");
                        break;
                    case ActionList.Run:
                        animator.SetTrigger("Run");
                        break;
                    default:
                        break;
                }
            }

            //animator.SetBool("Aiming", isAiming);
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
                if (IsAiming)
                    animator.SetTrigger("WalkAiming");
                else
                    animator.SetTrigger("Walk");

                Action = ActionList.Walk;
            }
        }

        if (Action == ActionList.Walk)
        {
            MoveToMarkedPosition(walkSpeed);

            if (transform.position == markedPosition)
            {
                if (IsAiming)
                    animator.SetTrigger("IdleAiming");
                else
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
                if (IsAiming)
                    animator.SetTrigger("RunAiming");
                else
                    animator.SetTrigger("Run");

                Action = ActionList.Run;
            }
        }

        if (Action == ActionList.Run)
        {
            MoveToMarkedPosition(runSpeed);

            if (transform.position == markedPosition)
            {
                if (IsAiming)
                    animator.SetTrigger("IdleAiming");
                else
                    animator.SetTrigger("Idle");

                Action = ActionList.Idle;
            }
        }


        //Aiming
        if (Input.GetKeyDown(KeyCode.A) && Action != ActionList.Crouch)
        {
            IsAiming = !IsAiming;
        }

        //Crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Action == ActionList.Crouch)
            {
                animator.SetTrigger("Idle");
                Action = ActionList.Idle;
            }
            else
            {
                animator.SetTrigger("Crouch");
                Action = ActionList.Crouch;

                isAiming = false;
            }
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
