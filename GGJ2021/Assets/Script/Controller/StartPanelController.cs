using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartPanelController : MonoBehaviour
{
    private string TutorialsPath = "Prefabs/UI/TutorialsPanel";
    private string SettingPath = "Prefabs/UI/SettingPanel";

    //开始游戏回调函数
    public void StarGmae()
    {
        transform.DOMoveY(1670, 0.3f).onComplete += () => { Bootstrap.Instance.LoadScene("GameScene"); };
    }

    //调出设置面板
    public void Setting()
    {
        //Debug.Log(Screen.width + " " + Screen.height);
        //Debug.Log(transform.position + "Local:" + transform.localPosition);
        GameObject settingPanel = Resources.Load<GameObject>(SettingPath);
        GameObject temp = Instantiate<GameObject>(settingPanel, new Vector3((float)Screen.width * 1.5f, (float)Screen.height / 2, 0), Quaternion.identity, transform);
        temp.name = settingPanel.name;


        //动画
        transform.DOLocalMoveX(-1920, 0.2f);//.OnComplete(() => { Debug.Log(transform.position + "Local:" + transform.localPosition); });
    }
    /// <summary>
    /// 调用教程面板
    /// </summary>
    public void Tutorials()
    {
        //获得预制体
        GameObject tutorialsPanelPrefabs = Resources.Load<GameObject>(TutorialsPath);
        Debug.Log(tutorialsPanelPrefabs.name);
        //实例化
        GameObject tutorialsPanel = Instantiate<GameObject>(tutorialsPanelPrefabs, new Vector3((float)Screen.width * 1.5f, (float)Screen.height / 2, 0), Quaternion.identity, transform);
        //改名字
        tutorialsPanel.name = tutorialsPanelPrefabs.name;

        //动画
        transform.DOLocalMoveX(-1920, 0.2f);
    }

    //退出游戏回调函数
    public void Quit()
    {
        Application.Quit();
    }
}
