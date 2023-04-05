using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateGrid : MonoBehaviour
{
    public static GenerateGrid Instance;
    public Tilemap tilemap;
    private Vector3Int[,] spots;
    private Astar astar;
    private BoundsInt bounds;
    void Start()
    {
        Instance = this;
        tilemap.CompressBounds();
        bounds = tilemap.cellBounds;

        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);
    }
    public void CreateGrid()
    {
        spots = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    spots[i, j] = new Vector3Int(x, y, 0);
                }
                else
                {
                    spots[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }
    public List<Spot> GeneratePath(Vector3 startPos, Vector3 endPos)
    {
        Vector3Int s = tilemap.WorldToCell(startPos);
        Vector2Int start = new Vector2Int(s.x, s.y);
        CreateGrid();
        List<Spot> roadPath = new List<Spot>();
        Vector3Int e = tilemap.WorldToCell(endPos);
        Vector2Int end = new Vector2Int(e.x, e.y);
        print(startPos);
        print(endPos);
        print(start);
        print(end);
        roadPath = astar.CreatePath(spots, start, end, 1000);
        print(roadPath);
        return roadPath;
    }
}
