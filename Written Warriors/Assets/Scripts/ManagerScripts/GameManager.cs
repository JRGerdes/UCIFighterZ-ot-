﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    

    public string PathP1 = "SampleCharactePrFab";
    public string PathP2 = "SampleCharactePrFab";
    private Character self1;
    private Character self2;
    public Character Self1 { get => self1; set => self1 = value; }
    public Character Self2 { get => self2; set => self2 = value; }
    bool P1 = true;
    //public Camera cam;
    public static GameManager instance;
    public GameObject HitEffect;
    public GameObject BlockEffect;
    public GameObject DeathEffect;

    public int w1 = 0;
    public int w2 = 0;

    private void Awake()
    {

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(instance);


    }

    public string ReturnPath()
    {
        if (P1)
        {
            P1 = false;
            return PathP1;
        }
        else
        {
            return PathP2;
        }

    }

    public IEnumerator FadeScreenIn(Image screen)
    {
        var tempColor = screen.color;
        while (tempColor.a > 0.0)
        {
            tempColor.a -= 0.1f;
            screen.color = tempColor;
            yield return null;
        }
        yield return null;
    }

    public IEnumerator FadeScreenOut(Image screen)
    {
        var tempColor = screen.color;
        while (tempColor.a < 1.0f)
        {
            tempColor.a += 0.1f;
            screen.color = tempColor;
            yield return null;
        }
        yield return null;
    }


    void Update()
    {
        if (Application.targetFrameRate!= 60)
            Application.targetFrameRate = 60;
    }
}
