using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public string towerGrade;  // ��: "Normal", "Rare", "Unique"...
    public int towerTypeNumber; // ��: 1~8

    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.E))
        {
            TryUpgrade();
        }
    }

    void TryUpgrade()
    {
        Debug.Log($"[���׷��̵� �õ�] {towerGrade}_{towerTypeNumber}");
        Tower[] allTowers = FindObjectsOfType<Tower>();

        foreach (Tower other in allTowers)
        {
            if (other != this &&
                other.towerGrade == this.towerGrade &&
                other.towerTypeNumber == this.towerTypeNumber)
            {
                // ���׷��̵� ���� ����
                Vector3 spawnPos = transform.position;

                TowerSpawner.Instance.SpawnUpgradedTower(towerGrade, towerTypeNumber, spawnPos);

                Destroy(other.gameObject);
                Destroy(this.gameObject);

                return;
            }
        }

        Debug.Log(" ���� ���׷��̵� ������ �����ϴ� �ٸ� Ÿ���� �����ϴ�.");
    }
    [Header("���� ����")]
    public float attackRange = 3f;         // ���� ��Ÿ�
    public float attackRate = 1f;          // �ʴ� ���� Ƚ��
    private float attackCooldown = 0f;

    [Header("����ü")]
    public GameObject projectilePrefab;    // �߻��� ����ü ������
    public Transform firePoint;            // ����ü�� �����Ǵ� ��ġ

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
