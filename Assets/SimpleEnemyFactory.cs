using UnityEngine;

public class SimpleEnemyFactory : EnemyFactory
{
    public GameObject enemyPrefab;

    public override GameObject CreateEnemy(Vector3 spawnPosition)
    {
        return Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
