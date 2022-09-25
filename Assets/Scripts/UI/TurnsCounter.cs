using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnsCounter : MonoBehaviour
{
    public TextMeshProUGUI indicator;
    public Image healthBarImage;
    Player player;

    void Start() {
        player = FindObjectOfType<Player>();
        player.OnTurnPassed.AddListener(UpdateIndicator);
        indicator.text = player.turns.ToString();

    }

    void UpdateIndicator() {
        indicator.text = player.turns.ToString();

        float fillQuantity = (float)player.turns / 100;
        healthBarImage.fillAmount = Mathf.Clamp(fillQuantity, 0, 1f);
        Debug.Log(fillQuantity);
    }
}
