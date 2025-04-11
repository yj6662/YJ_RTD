using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject normalEnemyPrefab;
    public GameObject bossEnemyPrefab;
    public float spawnInterval = 0.5f;
    public int enemiesPerWave = 30;

    private int currentWave = 1;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(DelayedStartWave());
    }

    IEnumerator DelayedStartWave()
    {
        yield return new WaitForSeconds(0.1f); // �� ������ ���� ��ٸ���
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        isSpawning = true;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy(false);
            yield return new WaitForSeconds(spawnInterval);
        }

        if (currentWave % 10 == 0)
        {
            SpawnEnemy(true); // ����
        }

        isSpawning = false;
        GameManager.Instance.AddGold(400);
        yield return StartCoroutine(WaitUntilAllEnemiesDead());
        yield return new WaitForSeconds(10f);
        currentWave++;
        StartCoroutine(StartWave());
    }

    IEnumerator WaitUntilAllEnemiesDead()
    {
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null; // ���� �����ӱ��� ���
        }
    }
    void SpawnEnemy(bool isBoss)
    {
        GameObject prefab = isBoss ? bossEnemyPrefab : normalEnemyPrefab;
        Vector3 spawnPos = TileManager.Instance.GetTileAt(0, 9).transform.position;
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
