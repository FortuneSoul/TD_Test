using System.Collections;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    private Enemy.Factory _enemyFactory;

    [Inject]
    private void Construct(Enemy.Factory enemyFactory)
    {
        _enemyFactory = enemyFactory;
    }
    
    public void Spawn(int count, LevelData.Path path, EnemyCommonSettings enemyStats)
    {
        StartCoroutine(SpawnRoutine(count, path, enemyStats));
    }

    private IEnumerator SpawnRoutine(int count, LevelData.Path path, EnemyCommonSettings enemyStats)
    {
        Vector2 dir = (path.waypoints[0] - path.spawnPos).normalized;
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        for (int i = 0; i < count; i++)
        {
            Enemy newEnemy = _enemyFactory.Create(enemyStats);
            
            newEnemy.transform.position = path.spawnPos;
            newEnemy.transform.rotation = Quaternion.Euler(Vector3.forward * zAngle);
            
            newEnemy.SetWaypoints(path.waypoints);
            
            yield return new WaitForSeconds(.5f);
        }
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }
}
