using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public static TowerSpawner Instance;

    public GameObject[] normalTowers;
    public GameObject[] rareTowers;
    public GameObject[] uniqueTowers;
    public GameObject[] epicTowers;
    public GameObject[] legendaryTowers;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[TowerSpawner] 싱글톤 정상 등록");
        }
        else
        {
            Destroy(gameObject);  // 중복 방지
        }
    }
    public void SpawnUpgradedTower(string currentGrade, int oldTypeNumber, Vector3 position)
    {
        Debug.Log($"[SpawnUpgradedTower] 호출됨 - {currentGrade}_{oldTypeNumber}");
        string nextGrade = GetNextGrade(currentGrade);
        GameObject[] pool = GetTowerPool(nextGrade);

        if (pool == null || pool.Length == 0)
        {
            Debug.LogWarning($"[Upgrade] {currentGrade} → {nextGrade} 업그레이드 실패");
            return;
        }

        int rand = Random.Range(0, pool.Length);
        GameObject newTower = pool[rand];
        Instantiate(newTower, position, Quaternion.identity);

        Debug.Log($"[Upgrade] {currentGrade}_{oldTypeNumber} → {nextGrade}_{rand + 1}");
    }

    private string GetNextGrade(string grade)
    {
        switch (grade)
        {
            case "Normal": return "Rare";
            case "Rare": return "Unique";
            case "Unique": return "Epic";
            case "Epic": return "Legendary";
            default: return null;
        }
    }

    private GameObject[] GetTowerPool(string grade)
    {
        switch (grade)
        {
            case "Normal": return normalTowers;
            case "Rare": return rareTowers;
            case "Unique": return uniqueTowers;
            case "Epic": return epicTowers;
            case "Legendary": return legendaryTowers;
            default: return null;
        }
    }
    public GameObject[] towerPrefabs; // 8개의 노말 타워
    public int towerCost = 100;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Tile tile = hit.collider.GetComponent<Tile>();

                if (tile != null && !tile.isOccupied && tile.isBuildable)
                {
                    if (GameManager.Instance.SpendGold(towerCost))
                    {
                        SpawnRandomTower(tile.transform.position);
                        tile.isOccupied = true;
                    }
                    else
                    {
                        Debug.Log("골드가 부족합니다!");
                    }
                }
            }
        }
    }

    void SpawnRandomTower(Vector3 position)
    {
        int index = Random.Range(0, towerPrefabs.Length);
        Instantiate(towerPrefabs[index], position, Quaternion.identity);
    }
}
