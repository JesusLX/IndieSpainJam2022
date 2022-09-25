using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cell : MonoBehaviour {
    public Sprite selectedBackground;
    public SpriteRenderer visual;
    public BaseObstacle obstacle = null;
    public IObstacle.Type obstacleType = IObstacle.Type.None;
    public Vector2Int mapPosition;
    private float speed;
    private ObtaclesManager obtaclesManager;
    private ICharacter _overMeCharacter;
    public Animator animator;
    public ICharacter OverMeCharacter { get => _overMeCharacter; set => _overMeCharacter = value; }

    public void init(bool animation) {
        visual.sprite = this.selectedBackground;
        if (animation) {
            StartCoroutine(SmoothLerp(3f));
        }
    }
    [ContextMenu("Set Obstacle")]
    public void RefreshObstacle() {
        if (this.hasObstacle()) {
            DestroyImmediate(this.obstacle.GetGameObject());
            this.obstacle = null;
        }
        if (obstacleType != IObstacle.Type.None)
            this.obstacle = Instantiate(ObtaclesManager.Instance.GetObstacle(obstacleType), this.transform).GetComponent<BaseObstacle>();
    }


    public void SetObstacle(IObstacle.Type obstacleType) {
        if (this.hasObstacle()) {
            if (this.obstacle.GetGameObject() == obstacle) return;
            this.gameObject.name = this.gameObject.name.Replace(obstacleType.ToString(),"");
            DestroyImmediate(this.obstacle.GetGameObject());
            this.obstacle = null;
        }
        this.obstacleType = obstacleType;
        GameObject newObstacle = ObtaclesManager.Instance.GetObstacle(obstacleType);
        if (newObstacle != null) {
            this.obstacle = Instantiate(newObstacle, this.transform).GetComponent<BaseObstacle>();
            gameObject.name = this.gameObject.name+obstacleType.ToString();
        } else {
            Debug.Log(obstacleType);
        }
    }

    public bool hasObstacle() {
        return obstacle != null;
    }

    private IEnumerator SmoothLerp(float time) {
        Vector3 startingPos = transform.position + Vector3.up * 30;
        Vector3 endPos = transform.position;

        float elapsedTime = 0;

        while (elapsedTime < time) {
            visual.gameObject.transform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        visual.gameObject.transform.position = transform.position;

    }


    internal void DoDamage(ICharacter.CharacterType targetType, int damage) {
        if (OverMeCharacter != null && OverMeCharacter.MyType == targetType) {
            OverMeCharacter.GetDamage(damage);
            this.animator.Play("Damage");
        }
    }
}