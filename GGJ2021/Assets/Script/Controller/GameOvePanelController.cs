using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOvePanelController : MonoBehaviour
{
    public void Back()
    {
        transform.DOMoveY(1670, 0.3f).onComplete += () => { Bootstrap.Instance.LoadScene("StartScene"); };
    }
    public void Replay()
    {
        transform.DOMoveY(1670, 0.3f).onComplete += () => { Bootstrap.Instance.LoadScene("GameScene"); };
    }

    public void BackCallBack(params object[] objs)
    {
        GameObject StartPanel = GameObject.Find("/Canvas/StartPanel");

        Debug.Log(SceneManager.GetActiveScene().name);
        
        //Debug.Log(StartPanel.name);
        //StartPanel.transform.localScale = new Vector3(0, -1080, 0);
        //StartPanel.transform.DOMoveY(0, 0.2f);
    }
}
