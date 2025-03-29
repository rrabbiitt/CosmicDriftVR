using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private EnemyAI enemyPrefab;
    [SerializeField] private float spawnInterval;
    [SerializeField] private int maxEnemiesNumber;
    [SerializeField] private Player player;

    private List<EnemyAI> spawnedEnemies = new List<EnemyAI>();
    private float timeSinceLastSpawn;
    private int spawnPointindex = 0;

    private void Start()
    {
        timeSinceLastSpawn = spawnInterval;
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn > spawnInterval)
        {
            timeSinceLastSpawn = 0f;
            if(spawnedEnemies.Count < maxEnemiesNumber)
            {
                SpawnEnemy();
            }
        }

        // 삭제되었을 수도 있는 적 캐릭터를 확인하고 리스트에서 제거
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
            }
        }
    }

    private void SpawnEnemy()
    {
        EnemyAI enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        //int spawnPointindex = spawnedEnemies.Count % spawnPoints.Length;
        enemy.Init(player, spawnPoints[spawnPointindex]);
        spawnPointindex++;
        if(spawnPointindex == spawnPoints.Length)
        {
            spawnPointindex = 0;
        }
        spawnedEnemies.Add(enemy);

        /// OnDestroyEvent 이벤트에 리스트에서 제거하는 함수를 등록
        enemy.OnDestroyEvent += OnEnemyDestroyed;
    }

    private void OnEnemyDestroyed(EnemyAI enemy)
    {
        spawnedEnemies.Remove(enemy); // 리스트에서 해당 적 캐릭터 제거
    }
}
