using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour, IObstacle
{
    public IObstacle.Type type;
    public bool isWalkable;
    public void DoAction() {
    }

    public IObstacle.Type GetObstacleType() {
        return type;
    }

    public bool HasAction() {
        return false;
    }

    public bool IsWalkable() {
        return this.isWalkable;
    }

    public void SetIsWalkable(bool walkable) {
        this.isWalkable = walkable;
    }

}
