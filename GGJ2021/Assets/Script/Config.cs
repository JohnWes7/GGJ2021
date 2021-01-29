using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : Single<Config>
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;

    //玩家速度
    public float speed;

    public Config()
    {
        up = (KeyCode)PlayerPrefs.GetInt("up", (int)KeyCode.W);
        down = (KeyCode)PlayerPrefs.GetInt("down", (int)KeyCode.S);
        left = (KeyCode)PlayerPrefs.GetInt("left", (int)KeyCode.A);
        right = (KeyCode)PlayerPrefs.GetInt("right", (int)KeyCode.D);

        speed = Random.Range(1f, 5f);
    }

    

}
