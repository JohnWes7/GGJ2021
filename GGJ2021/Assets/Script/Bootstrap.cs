using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void CallBack(params object[] objs);

public class Bootstrap : MonoBehaviour
{
    private static Bootstrap instance;
    public static Bootstrap Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("StartScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="callBack">加载后回调函数</param>
    /// <param name="objs">回调函数参数</param>
    public void LoadScene(string sceneName,CallBack callBack,params object[] objs)
    {
        //加载场景
        SceneManager.LoadScene(sceneName);

        //回调函数
        callBack(objs);
    }
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    public void LoadScene(string sceneName)
    {
        //加载场景
        SceneManager.LoadScene(sceneName);
    }
}
