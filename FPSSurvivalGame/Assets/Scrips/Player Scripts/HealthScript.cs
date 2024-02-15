using System;
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

    private EnemyAudio enemyAudio;

    private PlayerStats playerStats;

    void Awake()
    {
        if (isBoar || isCannibal)
        {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            //get enemy audio
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }


        if (isPlayer)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }

    public void ApplyDamage(float damage)
    {
        if(isDead) { return; }

        health -= damage;

        if (isPlayer)
        {
            playerStats.DisplayHealthStats(health);
        }

        if(isBoar || isCannibal)
        {
            if (enemy_Controller.Enemy_State == EnemyState.PATROL)
            {
                enemy_Controller.chase_Distance = 50f;
            }
        }

        if (health <= 0f)
        {
            PlayerDied();

            isDead = true;
        }
    }

    void PlayerDied()
    {

        if (isCannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 5f);

            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;

            StartCoroutine(DeadSound());

            //enemymanager spawn more enemies

            EnemyManager.instance.EnemyDied(true);

        }

        if (isBoar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();

            StartCoroutine(DeadSound());

            //enemymanager spawn more enemies
            EnemyManager.instance.EnemyDied(false);
        }

        if (isPlayer)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

            for (int i=0; i< enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            //call enemy manager to stop spawning enemies
            EnemyManager.instance.StopSpawning();

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        }

        if(tag == Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
    }//player died

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);

        enemyAudio.PlayDeadSound();
    }
}

