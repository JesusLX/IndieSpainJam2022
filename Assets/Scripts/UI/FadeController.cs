using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class FadeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnFadeOutDone() {
        GameManager.Instance.NextLevel();
    }

    internal void FadeOut() {
        GetComponent<Animator>().Play("FadeOut");
    }
}
