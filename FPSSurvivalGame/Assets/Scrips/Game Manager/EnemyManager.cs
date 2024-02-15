using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    private GameObject boarPrefab, cannibalPrefab;

    public Transform[] cannibalSpawnPoints, boarSpawnPoints;

    [SerializeField]
    private int cannibalEnemyCount, boarEnemyCount;

    private int initialCannibalCount, initialBoarCount;

    public float waitBeforeSpawnEnemiesTime = 10f;


    void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        initialCannibalCount = cannibalEnemyCount;
        initialBoarCount = boarEnemyCount;

        SpawnEnemies();

        StartCoroutine("CheckToSpawnEnemies");
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void SpawnEnemies()
    {
        SpawnCannibals();
        SpawnBoars();
    }

    void SpawnCannibals()
    {
        int index = 0;

        for(int i = 0; i < cannibalEnemyCount; i++){

            if(index >= cannibalSpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(cannibalPrefab, cannibalSpawnPoints[index].position, Quaternion.identity);

            index++;
        }

       cannibalEnemyCount = 0;
    }

    void SpawnBoars()
    {
        int index = 0;

        for (int i = 0; i < boarEnemyCount; i++)
        {

            if (index >= boarSpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(boarPrefab, boarSpawnPoints[index].position, Quaternion.identity);

            index++;
        }

        boarEnemyCount = 0;
    }

    IEnumerator CheckToSpawnEnemies()
    {
        yield return new WaitForSeconds(waitBeforeSpawnEnemiesTime);

        SpawnCannibals();

        SpawnBoars();

        StartCoroutine("CheckToSpawnEnemies");

    }

    public void EnemyDied(bool cannibal)
    {
        if (cannibal)
        {
            cannibalEnemyCount++;

            if (cannibalEnemyCount > initialCannibalCount)
            {
                cannibalEnemyCount = initialCannibalCount;
            }
        }
        else
        {
            boarEnemyCount++;

            if(boarEnemyCount>initialBoarCount)
            {
                boarEnemyCount = initialBoarCount;
            }
        }
    }

    public void StopSpawning()
    {
        StopCoroutine("CheckToSpawnEnemies");
    }

}
