using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{

    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_State;

    public float walk_Speed = 0.5f;
    public float run_Spped = 4f;

    public float chase_Distance = 7f;
    private float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;

    public GameObject attack_Point;


    private void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
    }

    void Start()
    {
        enemy_State = EnemyState.PATROL;

        patrol_Timer = patrol_For_This_Time;


        attack_Timer = wait_Before_Attack;

        current_Chase_Distance = chase_Distance;
    }

    
    void Update()
    {
        if (enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }
        if (enemy_State == EnemyState.CHASE)
        {
            Chase();
        }
        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }

    
    
    void Patrol()
    {
        
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        patrol_Timer += Time.deltaTime;

        if(patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();

            patrol_Timer = 0f;
        }

        if(navAgent.velocity.sqrMagnitude > 0) // look this sqrMagnitude
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }

        if(Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);

            enemy_State = EnemyState.CHASE;

            //play spotted audio
        }

    }//patrol

    
    void Chase()
    {

        navAgent.isStopped = false; // enable to the move again (if enemy is stopping)
        navAgent.speed = run_Spped;

        navAgent.SetDestination(target.position);

        if (navAgent.velocity.sqrMagnitude > 0) // look this sqrMagnitude
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);

        }

        if (Vector3.Distance(transform.position, target.position) <= attack_Distance)
        {
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);

            enemy_State = EnemyState.ATTACK;

            if(chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }
        else if (Vector3.Distance(transform.position,target.position) > chase_Distance)
        {
            // player run away the enemy

            enemy_Anim.Run(false);
            enemy_State = EnemyState.PATROL;

            patrol_Timer = patrol_For_This_Time;

            if (chase_Distance != current_Chase_Distance)
            {
                chase_Distance = current_Chase_Distance;
            }
        }

    }//chase
    
    void Attack()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;

        if(attack_Timer > wait_Before_Attack)
        {
            enemy_Anim.Attack();

            attack_Timer = 0f;

            // play attack sound
        }

        if (Vector3.Distance(transform.position,target.position) > attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }

    }//attack

    void SetNewRandomDestination()
    {

        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

        navAgent.SetDestination(navHit.position);

    }// set new random destination

    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

}
