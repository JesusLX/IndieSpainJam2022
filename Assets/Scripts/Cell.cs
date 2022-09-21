using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cell : MonoBehaviour
{
    public Sprite selectedBackground;
    public SpriteRenderer visual;
    public BaseObstacle obstacle = null;
    public IObstacle.Type obstacleType = IObstacle.Type.None;
    public Vector2 position ;
    private float speed;
    private ObtaclesManager obtaclesManager;

   
    public void init(bool animation)
    {
        visual.sprite = this.selectedBackground;
        position = this.gameObject.transform.position;
        if (animation) {
            StartCoroutine(SmoothLerp(3f));
        }
    }
    public void SetObstacle(IObstacle.Type obstacleType) {
        this.obstacleType = obstacleType;
        if (this.hasObstacle()) {
            DestroyImmediate(this.obstacle.gameObject);
            this.obstacle = null;
        }
        this.obstacle = Instantiate(ObtaclesManager.Instance.GetObstacle(obstacleType), this.transform).GetComponent<BaseObstacle>();
    }

    public bool hasObstacle() {
        return obstacle != null;
    }

    private IEnumerator SmoothLerp(float time)
    {
        Vector3 startingPos = transform.position + Vector3.up * 30;
        Vector3 endPos = transform.position;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            visual.gameObject.transform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        visual.gameObject.transform.position = transform.position;

    }
}