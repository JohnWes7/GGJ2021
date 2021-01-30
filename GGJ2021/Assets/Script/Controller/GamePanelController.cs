using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelController : MonoBehaviour
{
    public GameObject BorderText;
    public GameObject CellBorder;

    // Update is called once per frame
    void Update()
    {
        //位置跟随
        BorderText.transform.position = Camera.main.WorldToScreenPoint(CellBorder.transform.position);
        //旋转跟随
        BorderText.transform.rotation = CellBorder.transform.rotation * Quaternion.Euler(0, 0, -90);
        //颜色跟随
        BorderText.GetComponent<Text>().color = CellBorder.GetComponent<SpriteRenderer>().color;
    }

    public void UpdateBorder(int power)
    {

    }
}
