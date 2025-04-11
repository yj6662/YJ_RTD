using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public GameObject tilePrefab;
    public int columns = 10;
    public int rows = 10;
    public float spacing = 0.9f;

    public Dictionary<Vector2Int, Tile> tileDict = new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        GenerateTiles();
    }

    void GenerateTiles()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 spawnPos = new Vector3(x * spacing-5, y * spacing-4, 0f);
                GameObject tileObj = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                tileObj.name = $"Tile_{x}_{y}";

                Tile tile = tileObj.GetComponent<Tile>();
                Vector2Int coord = new Vector2Int(x, y);

                tileDict.Add(coord, tile);
            }
        }
        if (PathManager.Instance != null)
        {
            PathManager.Instance.SetupPathTiles();
        }
    }


    public Tile GetTileAt(int x, int y)
    {
        Vector2Int coord = new Vector2Int(x, y);
        if (tileDict.ContainsKey(coord))
            return tileDict[coord];
        return null;
    }
}
