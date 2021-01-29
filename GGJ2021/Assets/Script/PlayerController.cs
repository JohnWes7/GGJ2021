using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float hori = 0;
    float verc = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //按键检测
        ButtonDetect();
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
            verc = Mathf.Lerp(verc, 1, 5 * Time.deltaTime);
        }
        else if (Input.GetKey(Config.Instance.down))
        {
            Debug.Log("s");
            verc = Mathf.Lerp(verc, -1, 5f * Time.deltaTime);
        }
        else
        {
            verc = Mathf.Lerp(verc, 0, 5f * Time.deltaTime);
        }

        transform.Translate(0, verc * Config.Instance.speed * Time.deltaTime, 0);
        Debug.Log(verc);

        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");

        //transform.Translate(0, y * Config.Instance.speed * Time.deltaTime, 0);
        //Debug.Log(y);


        ////左转
        //if (Input.GetKey(Config.Instance.left))
        //{
        //    Debug.Log("a");
        //    transform.Rotate(0, 0, Config.Instance.speed * 30 * Time.deltaTime);
        //}
        ////右转
        //if (Input.GetKey(Config.Instance.right))
        //{
        //    Debug.Log("d");
        //    transform.Rotate(0, 0, -Config.Instance.speed * 30 * Time.deltaTime);
        //}
    }
}
