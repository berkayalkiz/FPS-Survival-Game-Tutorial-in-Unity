using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class HealtScript : MonoBehaviour
{
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_Controller;

    public float health = 100f;

    public bool isPlayer, isBoar, isCannibal;

    private bool isDead;


    void Awake()
    {
        if (isBoar || isCannibal)
        {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            //get enemy audio
        }

        if (isPlayer)
        {

        }
    }

    public void ApplyDamage(float damage)
    {
        if(isDead) { return; }

        health -= damage;
    }
}
