using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public string towerGrade;  // 예: "Normal", "Rare", "Unique"...
    public int towerTypeNumber; // 예: 1~8

    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.E))
        {
            TryUpgrade();
        }
    }

    void TryUpgrade()
    {
        Debug.Log($"[업그레이드 시도] {towerGrade}_{towerTypeNumber}");
        Tower[] allTowers = FindObjectsOfType<Tower>();

        foreach (Tower other in allTowers)
        {
            if (other != this &&
                other.towerGrade == this.towerGrade &&
                other.towerTypeNumber == this.towerTypeNumber)
            {
                // 업그레이드 조건 충족
                Vector3 spawnPos = transform.position;

                TowerSpawner.Instance.SpawnUpgradedTower(towerGrade, towerTypeNumber, spawnPos);

                Destroy(other.gameObject);
                Destroy(this.gameObject);

                return;
            }
        }

        Debug.Log(" 씬에 업그레이드 조건을 만족하는 다른 타워가 없습니다.");
    }
    [Header("공격 설정")]
    public float attackRange = 3f;         // 공격 사거리
    public float attackRate = 1f;          // 초당 공격 횟수
    private float attackCooldown = 0f;

    [Header("투사체")]
    public GameObject projectilePrefab;    // 발사할 투사체 프리팹
    public Transform firePoint;            // 투사체가 생성되는 위치

    void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0f)
        {
            GameObject target = FindTarget();
            if (target != null)
            {
                Attack(target);
                attackCooldown = 1f / attackRate;
            }
        }
    }

    GameObject FindTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= attackRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.gameObject;
            }
        }

        return closestEnemy;
    }

    void Attack(GameObject target)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTarget(target);
        }
    }
    private void OnDestroy()
{
    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
    if (hit.collider != null)
    {
        Tile tile = hit.collider.GetComponent<Tile>();
        if (tile != null)
        {
            tile.isOccupied = false;
        }
    }
}
}
