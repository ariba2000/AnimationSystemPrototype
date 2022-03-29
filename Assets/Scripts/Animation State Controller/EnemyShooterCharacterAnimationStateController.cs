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
        Shoot,
        Jump,
        Death
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
                        animator.CrossFadeInFixedTime("Base Layer.WalkAiming Blend Tree", 0.25f);
                        break;
                    case ActionList.Run:
                        animator.CrossFadeInFixedTime("Base Layer.RunAiming Blend Tree", 0.25f);
                        break;
                    case ActionList.Shoot:
                        animator.CrossFadeInFixedTime("Base Layer.Shoot", 0.25f);
                        break;
                    case ActionList.Death:
                        animator.CrossFadeInFixedTime("Base Layer.Death", 0.25f);
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
                    case ActionList.Jump:
                        animator.CrossFadeInFixedTime("Base Layer.Jump", 0.25f);
                        break;
                    case ActionList.Death:
                        animator.CrossFadeInFixedTime("Base Layer.Death", 0.25f);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    bool isAiming = false;
    bool isShooting = false;
    bool gettingHit;

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
            animator.SetFloat("XAxis", 0f);
            animator.SetFloat("YAxis", 1f);

            isShooting = !isShooting;
            isAiming = !isAiming;
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
                isAiming = false;
                Action = ActionList.Crouch;
            }
        }


        //Shoot
        if (Input.GetKeyDown(KeyCode.S) && isAiming)
        {
            isShooting = !isShooting;

        }


        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Action == ActionList.Jump)
            {
                animator.CrossFadeInFixedTime("Landing", 0.1f);
            }
            else
            {
                Action = ActionList.Jump;
            }
        }

        if (Action == ActionList.Jump)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Landing") && stateInfo.normalizedTime > 1)
            {
                Action = ActionList.Idle;
            }
        }


        //Death
        if (Input.GetKeyDown(KeyCode.D))
        {
            Action = ActionList.Death;
        }

        //Get Hit
        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.CrossFadeInFixedTime("GetHit", 0.25f);
            gettingHit = true;
        }

        if (gettingHit)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("GetHit") && stateInfo.normalizedTime > 1)
            {
                gettingHit = false;
                Action = Action;
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

        if (Physics.Raycast(ray, out hit) && !isShooting)
        {
            transform.LookAt(hit.point);
        }

        Vector3 direction = (hit.point - transform.position);
        float radAngle = Vector3.Angle(transform.forward, direction) * Mathf.Deg2Rad;

        if (Vector3.Cross(transform.forward, direction).y < 0)
        {
            radAngle = -radAngle;
        }

        Debug.Log("Y: " + Mathf.Cos(radAngle) + " X: " + Mathf.Sin(radAngle));
        animator.SetFloat("XAxis", Mathf.Sin(radAngle));
        animator.SetFloat("YAxis", Mathf.Cos(radAngle));

        return hit.point;
    }
}
