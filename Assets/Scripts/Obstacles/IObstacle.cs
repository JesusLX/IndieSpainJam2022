using UnityEngine;

public interface IObstacle {
    enum Type {
        None, UpperWall, BottomWall, LeftWall, RigthWall, Exit, Entrance, UpperLeftWallCorner, BottomLeftWallCorner, UpperRigthWallCorner, BottomRigthWallCorner, InnerWall, Void, ActivatorLever, ActivableVoidBridge, PickUpKey, PickUpCoin
    }
    public Type GetObstacleType();
    bool IsWalkable();
    void SetIsWalkable(bool walkable);

    bool HasAction();
    void DoAction();
    GameObject GetGameObject();
}