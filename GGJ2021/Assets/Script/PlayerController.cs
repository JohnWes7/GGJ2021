using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float hori = 0;
    float verc = 0;
    float damping = 10;

    //初始检测距离
    float distance = 2;
    //初始视角范围
    float lookAngle = 40f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //按键检测
        ButtonDetect();
        //射线检测
        ObjectInYourSight();
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
        Debug.Log(verc);

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
    public List<GameObject> ObjectInYourSight()
    {
        int lookAccurate = 4;
        List<GameObject> list = new List<GameObject>();
        RaycastHit[] hitInfo;

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
            RaycastHit[] hitLeft = RayDetect(Quaternion.Euler(0, 0, sub * (i + 1)), distance);
            RaycastHit[] hitRight = RayDetect(Quaternion.Euler(0, 0, -1 * sub * (i + 1)), distance);

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

        return null;
    }
    public RaycastHit[] RayDetect(Quaternion eularAngle, float distance)
    {
        Debug.DrawRay(transform.position, eularAngle * transform.forward.normalized, Color.green);

        RaycastHit[] hitInfo;
        hitInfo = Physics.RaycastAll(transform.position, eularAngle * transform.forward.normalized, 20f, 1 << LayerMask.NameToLayer("Object"));

        return hitInfo;
    }
}
