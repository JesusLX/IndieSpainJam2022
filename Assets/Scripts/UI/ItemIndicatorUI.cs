using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemIndicatorUI : MonoBehaviour
{
    public TextMeshProUGUI indicator;
    public IItem.Type itemIndicator;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();    
        player.OnItemAdded.AddListener(UpdateIndicator);
        player.OnItemRemoved.AddListener(UpdateIndicator);
    }

    void UpdateIndicator(IItem.Type item) {
        if(item == itemIndicator) {
            indicator.text = player.GetItemCount(item).ToString();
        }
    }
   
}
