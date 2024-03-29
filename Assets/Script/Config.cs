﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : Single<Config>
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode mouseDown;

    //玩家速度
    public float speed;

    public Config()
    {
        up = (KeyCode)PlayerPrefs.GetInt("up", (int)KeyCode.W);
        down = (KeyCode)PlayerPrefs.GetInt("down", (int)KeyCode.S);
        left = (KeyCode)PlayerPrefs.GetInt("left", (int)KeyCode.A);
        right = (KeyCode)PlayerPrefs.GetInt("right", (int)KeyCode.D);

        mouseDown = (KeyCode)PlayerPrefs.GetInt("mouseDown", (int)KeyCode.Mouse0);

        speed = 5f;
    }

    

}
