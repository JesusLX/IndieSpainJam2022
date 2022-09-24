using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivableObstacle : MonoBehaviour, IObstacle {
    public GameObject ActiveVisual;
    public GameObject DeactiveVisual;
    public bool activated;
    public bool activatedWalkable;
    public bool deactivatedWalkable;
    public ActivatorObstacle activator;

    public IObstacle.Type type;
    public bool isWalkable;
    private bool hasAction;

    private void Start() {
        if (activator != null) {
            this.activated = activator.activated;
            if (activated) {
                DeactiveVisual.SetActive(false);
                ActiveVisual.SetActive(true);
            } else {
                ActiveVisual.SetActive(false);
                DeactiveVisual.SetActive(true);
            }
            activator.OnActivationChanged.AddListener(OnActivate);
        } else {
            Debug.LogError("No tiene activador", this);
        }
    }

    public  void DoAction() {

    }

    public void OnActivate(bool activated) {
        if (activated) {
            DeactiveVisual.SetActive(false);
            ActiveVisual.SetActive(true);
            SetIsWalkable(activatedWalkable);
        } else {
            ActiveVisual.SetActive(false);
            DeactiveVisual.SetActive(true);
            SetIsWalkable(deactivatedWalkable);
        }
    }

    public  IObstacle.Type GetObstacleType() {
        return type;
    }

    public  bool HasAction() {
        return false;
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