using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    //玩家
    public GameObject player;
    public Vector2 planeOffsetX = new Vector2(-9.6f, 9.6f);
    public Vector2 planeOffsetY = new Vector2(-5.4f, 5.4f);

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player.transform.position = new Vector3(0, -7.5f, 0);
        player.transform.DOMoveY(-4, 0.4f).SetEase(Ease.OutBack).onComplete += () => { player.GetComponent<PlayerController>().enabled = true; };
    }


}
