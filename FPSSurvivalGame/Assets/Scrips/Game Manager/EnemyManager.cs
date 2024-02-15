using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    private GameObject boarPrefab, cannibalPrefeb;

    public Transform[] cannibalSpawnPoints, boarSpawnPoints;

    [SerializeField]
    private int cannibalEnemyCount, boarEnemyCount;

    private int initialCannibalCount, initialBoarCount;

    public float waitBeforeSpawnEnemiesTime = 10f;


    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
