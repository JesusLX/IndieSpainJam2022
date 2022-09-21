using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public int height = 0;
    public int width = 0;
    public float offset = 0f;
    public GameObject cellPrefab;
    [SerializeField]
    public Cell[,] map;

    public Vector2 EntrancePosition;
    public Vector2 ExitPosition;


    void Start()
    {
      
    }
    [ContextMenu("Generate Map")]
    public void GenerateMap() {
        map = new Cell[height, width];
        for (int h = 0; h < height; h++) {
            for (int w = 0; w < width; w++) {
                if(map[h, w] != null) {
                    DestroyImmediate(map[h, w].gameObject);
                }
                GameObject tmpCell = Instantiate(cellPrefab, new Vector3(w + offset, h + offset, 0), Quaternion.identity, transform);
                map[h, w] = tmpCell.GetComponent<Cell>();
                map[h, w].position = new Vector2(w, h);
                this.TrySetObstacle(h == height - 1,"Upper",map[h, w],IObstacle.Type.UpperWall);
                this.TrySetObstacle(h == 0,"Botton",map[h, w],IObstacle.Type.BottomWall);
                this.TrySetObstacle(w == 0,"Left",map[h, w],IObstacle.Type.LeftWall);
                this.TrySetObstacle(w == width - 1,"Rigth",map[h, w],IObstacle.Type.RigthWall);
                this.TrySetObstacle(EntrancePosition.y == h && EntrancePosition.x == w,"Entrance",map[h, w],IObstacle.Type.Entrance);
                this.TrySetObstacle(ExitPosition.y == h && ExitPosition.x == w,"Exit",map[h, w],IObstacle.Type.Exit);
                this.TrySetObstacle(h == height - 1 && w == 0, "UpperLeft", map[h, w], IObstacle.Type.UpperLeftWallCorner);
                this.TrySetObstacle(h == height - 1 && w == width - 1, "UpperRight", map[h, w], IObstacle.Type.UpperRigthWallCorner);
                this.TrySetObstacle(h == 0 && w == 0, "BottomLeft", map[h, w], IObstacle.Type.BottomLeftWallCorner);
                this.TrySetObstacle(h == 0 && w == width - 1, "BottomRight", map[h, w], IObstacle.Type.BottomRigthWallCorner);

                map[h, w].init(false);
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
        for (int h = 0; h < height; h++) {
            for (int w = 0; w < width; w++) {
                map[h, w].SetObstacle(map[h, w].obstacleType);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
