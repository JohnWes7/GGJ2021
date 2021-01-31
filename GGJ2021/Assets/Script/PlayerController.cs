using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    //精灵
    public SpriteRenderer white;
    //动画队列
    Sequence sequence;

    public Transform pos;
    float hori = 0;
    float verc = 0;
    float damping = 10;

    //初始检测距离
    public float distance = 2.5f;
    //初始视角范围
    public float lookAngle = 70f;
    public float scale = 1f;

    public GameObject select1;
    //public GameObject select2;

    //电量
    private int power = 100;
    private float powerTimer = 0;
    public UnityEvent<int> onPowerChange = new UnityEvent<int>();

    bool isDead = false;

    //游戏时间
    public float gameTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        //添加回调函数
        onPowerChange.AddListener(GameSceneManager.instance.gamePanel.UpdatePower);//显示更新回调
        onPowerChange.AddListener(DetectPower);//电量检测函数

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        //计时器
        if (!GameSceneManager.instance.isGameOver)
        {
            powerTimer += Time.deltaTime;
            gameTime += Time.deltaTime;
        }

        if (powerTimer >= 1)
        {
            //掉电量
            power = Mathf.Clamp(power - 1, 0, 100);
            //执行在点亮改变时的回调函数(检测power是否为零，更新UI显示)
            onPowerChange.Invoke(power);

            powerTimer = 0;
        }

        //按键检测
        ButtonDetect();
        //出界判定
        OutOfBounds();
        

        //射线检测
        List<GameObject> sightList = ObjectInYourSight();
        #region debug
        //Debug.Log(sightList.Count);
        //for (int i = 0; i < sightList.Count; i++)
        //{
        //    Debug.Log(sightList[i].name);
        //} 
        #endregion
        //鼠标点击
        MouseDown(sightList);
        //定住物体
        RootObject(sightList);
    }

    /// <summary>
    /// Update中的按键检测
    /// </summary>
    public void ButtonDetect()
    {
        //前进
        if (Input.GetKey(Config.Instance.up))
        {
            verc = Mathf.Lerp(verc, 1, damping * Time.deltaTime);
        }
        else if (Input.GetKey(Config.Instance.down))
        {
            verc = Mathf.Lerp(verc, -1, damping * Time.deltaTime);
        }
        else
        {
            verc = Mathf.Lerp(verc, 0, damping * Time.deltaTime);
        }

        transform.Translate(0, verc * Config.Instance.speed * Time.deltaTime, 0);

        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");

        //transform.Translate(0, y * Config.Instance.speed * Time.deltaTime, 0);
        //Debug.Log(y);


        //左转
        if (Input.GetKey(Config.Instance.left))
        {
            hori = Mathf.Lerp(hori, 1, damping * Time.deltaTime);
        }
        //右转
        else if (Input.GetKey(Config.Instance.right))
        {
            hori = Mathf.Lerp(hori, -1, damping * Time.deltaTime);
        }
        else
        {
            hori = Mathf.Lerp(hori, 0, damping * Time.deltaTime);
        }
        transform.Rotate(0, 0, hori * Config.Instance.speed * 30 * Time.deltaTime);
    }

    /// <summary>
    /// 取在视野范围内的物体
    /// </summary>
    /// <returns>所有被检测到的物体列表</returns>
    public List<GameObject> ObjectInYourSight()
    {
        int lookAccurate = 4;
        List<GameObject> list = new List<GameObject>();
        RaycastHit2D[] hitInfo;

        hitInfo = RayDetect(Quaternion.identity, distance);
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (!list.Contains(hitInfo[i].transform.gameObject))
            {
                list.Add(hitInfo[i].transform.gameObject);
            }
        }

        float sub = (lookAngle / 2) / lookAccurate;
        for (int i = 0; i < lookAccurate; i++)
        {
            RaycastHit2D[] hitLeft = RayDetect(Quaternion.Euler(0, 0, sub * (i + 1)), distance);
            RaycastHit2D[] hitRight = RayDetect(Quaternion.Euler(0, 0, -1 * sub * (i + 1)), distance);

            for (int j = 0; j < hitLeft.Length; j++)
            {
                if (!list.Contains(hitLeft[j].transform.gameObject))
                {
                    list.Add(hitLeft[j].transform.gameObject);
                }
            }
            for (int j = 0; j < hitRight.Length; j++)
            {
                if (!list.Contains(hitRight[j].transform.gameObject))
                {
                    list.Add(hitRight[j].transform.gameObject);
                }
            }
        }

        return list;
    }

    /// <summary>
    /// 射线检测
    /// </summary>
    /// <param name="eularAngle">角度偏移量</param>
    /// <param name="distance">检测长度</param>
    /// <returns>检测到的物体</returns>
    public RaycastHit2D[] RayDetect(Quaternion eularAngle, float distance)
    {
        Debug.DrawRay(transform.position, eularAngle * transform.up.normalized * distance, Color.green);

        RaycastHit2D[] hitInfo;
        hitInfo = Physics2D.RaycastAll(transform.position, eularAngle * transform.up.normalized, distance, 1 << LayerMask.NameToLayer("Object"));

        return hitInfo;
    }

    /// <summary>
    /// 定住视野范围内的物体
    /// </summary>
    /// <param name="sightList">视野范围内物体的列表</param>
    public void RootObject(List<GameObject> sightList)
    {
        for (int i = 0; i < sightList.Count; i++)
        {
            sightList[i].GetComponent<ObjectController>().BeRoot();
        }
    }

    /// <summary>
    /// 出界
    /// </summary>
    public void OutOfBounds()
    {
        Vector3 pos = this.pos.position;

        //出界判定
        if (pos.y > GameSceneManager.instance.planeOffsetY.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 2 * GameSceneManager.instance.planeOffsetY.y);
        }
        if (pos.y < GameSceneManager.instance.planeOffsetY.x)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 2 * GameSceneManager.instance.planeOffsetY.y);
        }
        if (pos.x > GameSceneManager.instance.planeOffsetX.y)
        {
            transform.position = new Vector3(transform.position.x - 2 * GameSceneManager.instance.planeOffsetX.y, transform.position.y);
        }
        if (pos.x < GameSceneManager.instance.planeOffsetX.x)
        {
            transform.position = new Vector3(transform.position.x + 2 * GameSceneManager.instance.planeOffsetX.y, transform.position.y);
        }
    }

    /// <summary>
    /// 鼠标点击检测
    /// </summary>
    /// <param name="sightList">视野范围内物体列表</param>
    public void MouseDown(List<GameObject> sightList)
    {
        if (Input.GetKeyDown(Config.Instance.mouseDown))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), Vector2.zero);
            
            if (hit.collider)
            {
                //如果是点到了范围内的物体
                if (sightList.Contains(hit.collider.gameObject))
                {
                    //如果select1没东西
                    if (select1 == null)
                    {
                        hit.collider.GetComponent<ObjectController>().BeSelect();
                        select1 = hit.collider.gameObject;
                        return;
                    }
                    //如果select1存了物体
                    else
                    {
                        if (hit.collider.gameObject == select1)
                        {
                            hit.collider.GetComponent<ObjectController>().BeSelect();
                            select1 = hit.collider.gameObject;
                            return;
                        }
                        //如果第二个选的物体的名字一样
                        if (hit.collider.gameObject != select1 && hit.collider.name == select1.name)
                        {
                            select1.GetComponent<ObjectController>().Exit();
                            hit.collider.GetComponent<ObjectController>().Exit();

                            //增加电量
                            AddPower(40);

                            select1 = null;
                            return;
                        }
                        //如果第二个选的物体的名字不一样
                        else
                        {
                            hit.collider.GetComponent<ObjectController>().BeSelectWrong();
                        }
                    }


                }
            }
        }
    }

    /// <summary>
    /// 检测power
    /// </summary>
    /// <param name="power"></param>
    public void DetectPower(int power)
    {
        if (power <= 0)
        {
            Dead();
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void Dead()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        sequence = DOTween.Sequence();//重置

        //sequence.Append(white.DOFade(0.3f, 0.1f));
        //sequence.Append(white.DOFade(1f, 0.1f));
        //sequence.Append(white.DOFade(0.3f, 0.1f));
        //sequence.Append(white.DOFade(1f, 0.1f));
        //sequence.Append(white.DOFade(0, 0.3f));
        sequence.Append(white.DOFade(0, 1.5f).SetEase(Ease.InBounce));

        sequence.onComplete += () => { 
            GameSceneManager.instance.GameOver();
            Destroy(gameObject);
        };
        
        
    }

    /// <summary>
    /// 增加电量方法
    /// </summary>
    /// <param name="num">增加的电量</param>
    public void AddPower(int num)
    {
        power = Mathf.Clamp(power += num, 0, 100);
        onPowerChange.Invoke(power);
    }
}
