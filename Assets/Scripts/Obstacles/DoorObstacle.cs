using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorObstacle : MonoBehaviour, IObstacle {
    public GameObject ActiveVisual;
    public GameObject DeactiveVisual;
    public bool activated;
    public bool activatedWalkable;
    public bool deactivatedWalkable;
    public bool isWalkable;
    private bool hasAction;
    public IItem.Type activator;
    public IObstacle.Type type;

  


    private void Start() {
        if (activated) {
            DeactiveVisual.SetActive(false);
            ActiveVisual.SetActive(true);
        } else {
            ActiveVisual.SetActive(false);
            DeactiveVisual.SetActive(true);
        }
    }

    public void DoAction() {
        if (FindObjectOfType<Player>().SubstractItem(this.activator)) {
            OnActivate(true);
            FindObjectOfType<FadeController>().FadeOut();
        }
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

    public IObstacle.Type GetObstacleType() {
        return type;
    }

    public bool HasAction() {
        return true;
    }

    public bool IsWalkable() {
        return this.isWalkable;
    }

    public void SetIsWalkable(bool walkable) {
        this.isWalkable = walkable;
    }

    public GameObject GetGameObject() {
        return this.gameObject;
    }
}
