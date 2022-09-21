using System.Collections.Generic;
using UnityEngine;

public class ObtaclesManager : MonoBehaviour {
    public List<GameObject> obstacles;

    public static ObtaclesManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public GameObject GetObstacle(IObstacle.Type type) {
        if (type == IObstacle.Type.None) return null;
        Debug.Log(obstacles.Count);
        GameObject tmpObstacle = obstacles.Find(o => o.GetComponent<BaseObstacle>().GetObstacleType() == type);
        return tmpObstacle;
    }
}