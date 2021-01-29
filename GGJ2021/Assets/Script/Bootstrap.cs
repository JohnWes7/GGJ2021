using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("GameScene");
        Debug.Log(Config.Instance.up);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
