using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isOccupied = false;
    public bool isBuildable = true;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPathColor(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = color;

        isBuildable = false;
    }
}
