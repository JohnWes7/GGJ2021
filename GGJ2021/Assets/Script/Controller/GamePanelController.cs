using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePanelController : MonoBehaviour
{
    public Text BorderPowerText;
    public GameObject CellBorder;

    // 位置跟随
    void Update()
    {
        //游戏结束停止跟随
        if (GameSceneManager.instance.isGameOver)
        {
            return;
        }

        //位置跟随
        BorderPowerText.transform.position = Camera.main.WorldToScreenPoint(CellBorder.transform.position);
        //旋转跟随
        BorderPowerText.transform.rotation = CellBorder.transform.rotation * Quaternion.Euler(0, 0, -90);
        //颜色跟随
        BorderPowerText.color = CellBorder.GetComponent<SpriteRenderer>().color;
    }


    /// <summary>
    /// 更新能量
    /// </summary>
    /// <param name="power">能量</param>
    public void UpdatePower(int power)
    {
        BorderPowerText.text = power.ToString();
    }

    public void GameOverView()
    {
        //移动动画
        BorderPowerText.transform.DOLocalMove(new Vector3(70, 0, 0), 0.3f);
        //摆正
        BorderPowerText.transform.DORotate(new Vector3(0, 0, 0), 0.3f);
        //缩放动画
        BorderPowerText.transform.DOScale(new Vector3(3, 3, 0), 0.3f);
        BorderPowerText.DOColor(new Color(1, 1, 1, 1), 0.05f);
    }
}
