using UnityEngine;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance;

    [Header("경로 좌표를 순서대로 입력하세요")]
    public List<Vector2Int> pathTileCoords = new List<Vector2Int>();

    [Header("적이 따라갈 경로")]
    public List<Transform> pathPoints = new List<Transform>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetupPathTiles()
    {
        pathPoints.Clear();

        for (int i = 0; i < pathTileCoords.Count - 1; i++)
        {
            Vector2Int from = pathTileCoords[i];
            Vector2Int to = pathTileCoords[i + 1];

            List<Vector2Int> segment = GetLineBetween(from, to);

            foreach (Vector2Int coord in segment)
            {
                Tile tile = TileManager.Instance.GetTileAt(coord.x, coord.y);
                if (tile != null)
                {
                    tile.SetPathColor(Color.red);
                    pathPoints.Add(tile.transform);
                }
                else
                {
                    Debug.LogWarning($"[PathManager] 좌표 {coord}에 해당하는 타일을 찾을 수 없습니다.");
                }
            }
        }
    }

    private List<Vector2Int> GetLineBetween(Vector2Int a, Vector2Int b)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        if (a.x == b.x) // 세로
        {
            int dir = a.y < b.y ? 1 : -1;
            for (int y = a.y; y != b.y + dir; y += dir)
            {
                list.Add(new Vector2Int(a.x, y));
            }
        }
        else if (a.y == b.y) // 가로
        {
            int dir = a.x < b.x ? 1 : -1;
            for (int x = a.x; x != b.x + dir; x += dir)
            {
                list.Add(new Vector2Int(x, a.y));
            }
        }
        else
        {
            Debug.LogWarning($"[PathManager] 대각선 경로는 지원되지 않습니다: {a} → {b}");
        }

        return list;
    }

    private void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Count < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            if (pathPoints[i] != null && pathPoints[i + 1] != null)
            {
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
            }
        }
    }
}
