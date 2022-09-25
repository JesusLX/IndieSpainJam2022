using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BaseObstacle : MonoBehaviour, IObstacle
{
    public IObstacle.Type type;
    public bool isWalkable;
    private bool hasAction;
    [SerializeField]
    public IObstacle childClass;

    private void Start() {
       childClass = GetComponents<IObstacle>().ToList().Find(o => o != this);
    }

    public virtual void DoAction() {
        if (childClass != null) {
            childClass.DoAction();
        }
    }

    public virtual IObstacle.Type GetObstacleType() {
        if (childClass != null) {
          type = childClass.GetObstacleType();
        }
        return type;
    }

    public virtual bool HasAction() {
        if (childClass != null) {
            hasAction = childClass.HasAction();
        }

        return hasAction;
    }

    public virtual bool IsWalkable() {
        if (childClass != null) {
            isWalkable = childClass.IsWalkable();
        }
        return this.isWalkable;
    }

    public virtual void SetIsWalkable(bool walkable) {
        if (childClass != null) {
            childClass.SetIsWalkable(walkable);
        }
        this.isWalkable = walkable;
    }

    public virtual GameObject GetGameObject() {
        return this.gameObject;
    }
}
