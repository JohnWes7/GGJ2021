using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    
    //预制体路径
    string circle = "Prefabs/Object/Circle";
    string diamond = "Prefabs/Object/Diamond";
    string heart = "Prefabs/Object/Heart";
    string plum = "Prefabs/Object/Plum";
    string star = "Prefabs/Object/Star";
    string triangle = "Prefabs/Object/Triangle";

    string gameOverPanel = "Prefabs/UI/GameOverPanel";
    string victoryPanel = "Prefabs/UI/GameVictoryPanel";

    //预制体列表
    [SerializeField]
    List<GameObject> prefabsList = new List<GameObject>();

    //物体列表
    List<GameObject> objectList = new List<GameObject>();

    //玩家
    public GameObject player;
    public Vector2 planeOffsetX = new Vector2(-9.6f, 9.6f);
    public Vector2 planeOffsetY = new Vector2(-5.4f, 5.4f);

    public bool isGameOver = false;

    //游戏面板
    public GamePanelController gamePanel;

    //游戏胜利面板
    //游戏失败

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InIt();
        player.transform.position = new Vector3(0, -7.5f, 0);
        player.transform.DOMoveY(-4, 0.4f).SetEase(Ease.OutBack).onComplete += () => { player.GetComponent<PlayerController>().enabled = true; };
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void InIt()
    {
        prefabsList.Add(Resources.Load<GameObject>(circle));
        prefabsList.Add(Resources.Load<GameObject>(diamond));
        prefabsList.Add(Resources.Load<GameObject>(heart));
        prefabsList.Add(Resources.Load<GameObject>(plum));
        prefabsList.Add(Resources.Load<GameObject>(star));
        prefabsList.Add(Resources.Load<GameObject>(triangle));

        for (int i = 0; i < prefabsList.Count; i++)
        {
            //GameObject temp1 = Instantiate<GameObject>(prefabsList[i], new Vector3(Random.Range(planeOffsetX.x + 1, planeOffsetX.y - 1), Random.Range(planeOffsetY.x + 1, planeOffsetY.y - 1), 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            GameObject temp1 = Instantiate<GameObject>(prefabsList[i]);
            temp1.name = prefabsList[i].name;
            temp1.GetComponent<ObjectController>().Move();
            GameObject temp2 = Instantiate<GameObject>(prefabsList[i]);
            temp2.name = prefabsList[i].name;
            temp2.GetComponent<ObjectController>().Move();

            //加入列表
            objectList.Add(temp1);
            objectList.Add(temp2);
        }
    }

    /// <summary>
    /// 消除物体时调用
    /// </summary>
    /// <param name="go"></param>
    public void RemoveObject(GameObject go)
    {
        objectList.Remove(go);

        //如果全部被消除了 游戏胜利
        if (objectList.Count == 0)
        {
            GameVictory();
        }
    }

    /// <summary>
    /// 游戏结束回调
    /// </summary>
    public void GameOver()
    {
        isGameOver = true;

        GameObject gameover = Resources.Load<GameObject>(gameOverPanel);
        GameObject temp = Instantiate<GameObject>(gameover, new Vector3((float)Screen.width * 0.5f, (float)Screen.height * -0.5f, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
        temp.name = gameover.name;//改名

        temp.transform.DOLocalMoveY(0, 0.3f);

        gamePanel.GameOverView();
    }

    /// <summary>
    /// 游戏胜利回调
    /// </summary>
    public void GameVictory()
    {
        isGameOver = true;

        GameObject victory = Resources.Load<GameObject>(victoryPanel);
        GameObject temp = Instantiate<GameObject>(victory, new Vector3((float)Screen.width * 0.5f, (float)Screen.height * -0.5f, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
        temp.name = victory.name;//改名

        //刷新时间
        temp.GetComponent<GameVictoryPanelController>().UpdateTime(player.GetComponent<PlayerController>().gameTime);

        temp.transform.DOLocalMoveY(0, 0.3f);

        gamePanel.GameOverView();
    }
}
