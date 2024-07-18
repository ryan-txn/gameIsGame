using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public static class EnemyCounter
{
    private static int enemyCount;

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
        }
    }

    public static int GetEnemyCount()
    {
        return enemyCount;
    }
}
