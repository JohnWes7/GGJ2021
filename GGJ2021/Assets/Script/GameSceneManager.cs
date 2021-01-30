using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    public Vector2 planeOffsetX = new Vector2(-9.6f, 9.6f);
    public Vector2 planeOffsetY = new Vector2(-5.4f, 5.4f);

    private void Awake()
    {
        instance = this;
    }

    

    
}
