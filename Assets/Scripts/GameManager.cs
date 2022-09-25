using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string nextLevel;
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public void Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    internal void NextLevel() {
        SceneManager.LoadScene(nextLevel);

    }
}
