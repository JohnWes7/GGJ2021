using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVictoryPanelController : MonoBehaviour
{
    public Text Time;

    public void Back()
    {
        transform.DOMoveY(1670, 0.3f).onComplete += () => { Bootstrap.Instance.LoadScene("StartScene"); };
    }
    public void Replay()
    {
        transform.DOMoveY(1670, 0.3f).onComplete += () => { Bootstrap.Instance.LoadScene("GameScene"); };
    }

    public void UpdateTime(float time)
    {
        Time.text = time.ToString("0.00") + "s";
    }
}
