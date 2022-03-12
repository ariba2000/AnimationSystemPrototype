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
        Crouch,
        Shoot
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

            if (isAiming)
            {
                switch (action)
                {
                    case ActionList.Idle:
                        animator.CrossFadeInFixedTime("Base Layer.Aiming", 0.25f);
                        break;
                    case ActionList.Walk:
                        animator.CrossFadeInFixedTime("Base Layer.WalkAiming", 0.25f);
                        break;
                    case ActionList.Run:
                        animator.CrossFadeInFixedTime("Base Layer.RunAiming", 0.25f);
                        break;
                    case ActionList.Shoot:
                        animator.CrossFadeInFixedTime("Base Layer.Shoot", 0.25f);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (action)
                {
                    case ActionList.Idle:
                        animator.CrossFadeInFixedTime("Base Layer.Idle", 0.25f);
                        break;
                    case ActionList.Walk:
                        animator.CrossFadeInFixedTime("Base Layer.Walk", 0.25f);
                        break;
                    case ActionList.Run:
                        animator.CrossFadeInFixedTime("Base Layer.Run", 0.25f);
                        break;
                    case ActionList.Crouch:
                        animator.CrossFadeInFixedTime("Base Layer.Crouch", 0.25f);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    bool isAiming;
    bool IsAiming
    {
        get { return isAiming; }

        set
        {
            isAiming = value;
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

                Action = ActionList.Walk;
            }
        }

        if (Action == ActionList.Walk)
        {
            MoveToMarkedPosition(walkSpeed);

            if (transform.position == markedPosition)
            {
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
                Action = ActionList.Run;
            }
        }

        if (Action == ActionList.Run)
        {
            MoveToMarkedPosition(runSpeed);

            if (transform.position == markedPosition)
            {

                Action = ActionList.Idle;
            }
        }


        //Aiming
        if (Input.GetKeyDown(KeyCode.A) && Action != ActionList.Crouch && Action != ActionList.Shoot)
        {
            IsAiming = !IsAiming;
            Action = Action;
        }

        //Crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Action == ActionList.Crouch)
            {
                Action = ActionList.Idle;
            }
            else
            {
                IsAiming = false;
                Action = ActionList.Crouch;
            }
        }


        //Shoot
        if (Input.GetKeyDown(KeyCode.S) && IsAiming)
        {
            if (Action == ActionList.Shoot)
            {
                Action = ActionList.Idle;
            }
            else
            {
                Action = ActionList.Shoot;
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
