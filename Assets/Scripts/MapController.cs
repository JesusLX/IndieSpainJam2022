using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public int height = 0;
    public int width = 0;
    public float offset = 0f;
    public GameObject cellPrefab;
    public List<Cell> publicMap;

    public Vector2Int EntrancePosition;
    public Vector2Int ExitPosition;

    public ICharacter character;
    public Color primaryColor;
    public Color secondaryColor;

    public static MapController Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
    }

    [ContextMenu("Generate Map")]
    public void GenerateMap() {
        Cell[,] map = new Cell[height, width];
        publicMap = new List<Cell>();
        for (int h = 0; h < height; h++) {
            for (int w = 0; w < width; w++) {
                if (map[h, w] != null) {
                    DestroyImmediate(map[h, w].gameObject);
                }
                GameObject tmpCell = Instantiate(cellPrefab, new Vector3(w + offset, h + offset, 0), Quaternion.identity, transform);
                tmpCell.gameObject.name = "Cell (" + w + "," + h + ")";
                map[h, w] = tmpCell.GetComponent<Cell>();
                map[h, w].visual.color = ((w + h) % 2 == 0) ? primaryColor : secondaryColor;
                map[h, w].mapPosition = new Vector2Int(w, h);
                this.TrySetObstacle(h == height - 1, "Upper", map[h, w], IObstacle.Type.UpperWall);
                this.TrySetObstacle(h == 0, "Botton", map[h, w], IObstacle.Type.BottomWall);
                this.TrySetObstacle(w == 0, "Left", map[h, w], IObstacle.Type.LeftWall);
                this.TrySetObstacle(w == width - 1, "Rigth", map[h, w], IObstacle.Type.RigthWall);
                this.TrySetObstacle(h == height - 1 && w == 0, "UpperLeft", map[h, w], IObstacle.Type.UpperLeftWallCorner);
                this.TrySetObstacle(h == height - 1 && w == width - 1, "UpperRight", map[h, w], IObstacle.Type.UpperRigthWallCorner);
                this.TrySetObstacle(h == 0 && w == 0, "BottomLeft", map[h, w], IObstacle.Type.BottomLeftWallCorner);
                this.TrySetObstacle(h == 0 && w == width - 1, "BottomRight", map[h, w], IObstacle.Type.BottomRigthWallCorner);
                this.TrySetObstacle(EntrancePosition.y == h && EntrancePosition.x == w, "Entrance", map[h, w], IObstacle.Type.Entrance);
                this.TrySetObstacle(ExitPosition.y == h && ExitPosition.x == w, "Exit", map[h, w], IObstacle.Type.Exit);
                map[h, w].init(false);
                publicMap.Add(map[h, w]);
            }
        }
    }

    public bool TrySetObstacle(bool condition, string debugText, Cell cell, IObstacle.Type type) {
        if (condition) {
            Debug.Log(debugText);
            cell.SetObstacle(type);
        }
        return condition;
    }

    [ContextMenu("Update Map")]
    public void UpdateMap() {
        foreach (Cell cell in publicMap) {
            cell.visual.color = ((cell.mapPosition.x + cell.mapPosition.y) % 2 == 0) ? primaryColor : secondaryColor;
            cell.SetObstacle(cell.obstacleType);
        }
    }
    internal Cell GetCell(Vector2Int position) {
        return publicMap.Find(c => c.mapPosition == new Vector2Int(position.x, position.y));
    }
}
