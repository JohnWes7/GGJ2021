using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instanc;

    public Transform pos;
    float hori = 0;
    float verc = 0;
    float damping = 10;

    //初始检测距离
    public float distance = 2.5f;
    //初始视角范围
    public float lookAngle = 70f;
    public float scale = 1f;


    // Start is called before the first frame update
    void Start()
    {
        instanc = this;
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log("w");
            verc = Mathf.Lerp(verc, 1, damping * Time.deltaTime);
        }
        else if (Input.GetKey(Config.Instance.down))
        {
            Debug.Log("s");
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
            sightList[i].GetComponent<ObjectController>().moveTimer -= Time.deltaTime;
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

    public void MouseDown()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

        }
    }
}
