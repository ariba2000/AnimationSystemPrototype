using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreatureAnimationStateController : MonoBehaviour
{
    enum ActionList
    {
        Idle,
        Walk,
        Run,
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
                case ActionList.Death:
                    animator.CrossFadeInFixedTime("Base Layer.Death", 0.25f);
                    break;

            }
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

        //Get Hit
        if (Input.GetKeyDown(KeyCode.G))
        {
            Action = ActionList.Idle;
            animator.CrossFadeInFixedTime("GetHit", 0.25f);
        }


        //Attack
        if (Input.GetKeyDown(KeyCode.A))
        {
            Action = ActionList.Idle;
            animator.CrossFadeInFixedTime("Attack", 0.25f);
        }
        
        //Death
        if (Input.GetKeyDown(KeyCode.D))
        {
            Action = ActionList.Death;
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
