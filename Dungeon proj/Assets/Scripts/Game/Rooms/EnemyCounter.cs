using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public static class EnemyCounter
{
    private static int enemyCount;

    // Event that can be subscribed to for detecting when all enemies are killed
    //public static event Action OnAllEnemiesKilled;

    public static void SetEnemies(int count)
    {
        enemyCount = count;
    }

    public static void RemoveEnemy()
    {
        if (enemyCount > 0)
        {
            enemyCount--;
            Debug.Log("Enemy removed. Total enemies: " + enemyCount);
        }
        else if (enemyCount < 0)
        {
            Debug.Log("Enemy count is less than expected");
            //OnAllEnemiesKilled.Invoke();
        }
    }

    public static int GetEnemyCount()
    {
        return enemyCount;
    }
}
