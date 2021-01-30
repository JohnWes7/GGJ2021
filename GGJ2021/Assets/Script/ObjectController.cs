using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectController : MonoBehaviour
{
    public float moveTime = 3;
    public float moveTimer = 0;

    public bool isSelect = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;
        
    }

    private void LateUpdate()
    {
        if (moveTimer > moveTime)
        {
            Move();
            moveTimer = 0;
        }
    }

    public void Move()
    {
        Transform player = PlayerController.instanc.transform;
        Vector2 x = GameSceneManager.instance.planeOffsetX;
        Vector2 y = GameSceneManager.instance.planeOffsetY;

        Vector3 newPos;
        while (true)
        {
            newPos = new Vector3(Random.Range(x.x + 1, x.y - 1), Random.Range(y.x + 1, y.y - 1));
            float angle = Mathf.Acos(Vector3.Dot((newPos - player.position).normalized, player.up)) * Mathf.Rad2Deg;
            float distance = Vector3.Distance(newPos, player.position);

            if (distance < 0.5f)
            {
                continue;
            }

            if (distance > PlayerController.instanc.distance || angle > PlayerController.instanc.lookAngle / 2 + 15)
            {
                break;
            }
        }

        transform.position = newPos;
    }

    public void BeSelect()
    {
        isSelect = true;

        
    }
}
