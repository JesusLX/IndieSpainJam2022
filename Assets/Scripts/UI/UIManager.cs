using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject AudioOnButton, AudioOffButton, ReloadButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnMusicOn() {
        AudioManager.Instance.ChangeAllVolumes(0.2f);
        AudioOnButton.SetActive(true);
        AudioOffButton.SetActive(false);
    }

    public void turnMusicOff() {
        AudioManager.Instance.ChangeAllVolumes(0);
        AudioOffButton.SetActive(true);
        AudioOnButton.SetActive(false);
    }

    public void resetGame() {
        GameManager.Instance.Reset();
        Debug.Log("ea");
    }

}
