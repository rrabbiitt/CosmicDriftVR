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

        // �����Ǿ��� ���� �ִ� �� ĳ���͸� Ȯ���ϰ� ����Ʈ���� ����
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

        /// OnDestroyEvent �̺�Ʈ�� ����Ʈ���� �����ϴ� �Լ��� ���
        enemy.OnDestroyEvent += OnEnemyDestroyed;
    }

    private void OnEnemyDestroyed(EnemyAI enemy)
    {
        spawnedEnemies.Remove(enemy); // ����Ʈ���� �ش� �� ĳ���� ����
    }
}
