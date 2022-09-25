using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnsCounter : MonoBehaviour
{
    public TextMeshProUGUI indicator;
    Player player;

    void Start() {
        player = FindObjectOfType<Player>();
        player.OnTurnPassed.AddListener(UpdateIndicator);
        indicator.text = player.turns.ToString();

    }

    void UpdateIndicator() {
        indicator.text = player.turns.ToString();
    }
}
