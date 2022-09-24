using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpObstacle : MonoBehaviour ,IObstacle {
    public GameObject ActiveVisual;
    public GameObject DeactiveVisual;
    public bool activated;
    public bool oneTimeActivable;    
    public bool hasAction = true;
    public UnityEvent<bool>OnActivationChanged;

    public IObstacle.Type type;
    public bool isWalkable;

    private void Start() {
        if (activated) {
            DeactiveVisual.SetActive(false);
            ActiveVisual.SetActive(true);
        } else {
            ActiveVisual.SetActive(false);
            DeactiveVisual.SetActive(true);
        }
    }
    public  void DoAction() {
        Debug.Log("Activando");
        activated = !activated;
        if(activated) {
            DeactiveVisual.SetActive(false);
            ActiveVisual.SetActive(true);
        } else {
            ActiveVisual.SetActive(false);
            DeactiveVisual.SetActive(true);
        }
        OnActivationChanged?.Invoke(activated);
        if (this.oneTimeActivable) {
            this.hasAction = false;
        }
    }

    public  IObstacle.Type GetObstacleType() {
        return type;
    }

    public  bool HasAction() {
        return this.hasAction;
    }

    public  bool IsWalkable() {
        return this.isWalkable;
    }

    public  void SetIsWalkable(bool walkable) {
        this.isWalkable = walkable;
    }

    public  GameObject GetGameObject() {
        return this.gameObject;
    }
}