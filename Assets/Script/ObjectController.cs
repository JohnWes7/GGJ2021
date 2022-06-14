using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectController : MonoBehaviour
{
    public float moveTime = 3;
    public float moveTimer = 0;

    public Color highLight;
    public Color black;
    public SpriteRenderer m_spriteRenderer;
    Sequence sequence;

    public bool isSelect = false;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSceneManager.instance.isGameOver)
        {
            return;
        }
        //如果没有被选择就加时间
        if (!isSelect)
        {
            moveTimer += Time.deltaTime;
        }
        
    }

    private void LateUpdate()
    {
        if (GameSceneManager.instance.isGameOver)
        {
            return;
        }

        if (moveTimer > moveTime)
        {
            Move();
            moveTimer = 0;
        }
    }

    /// <summary>
    /// 随机闪现
    /// </summary>
    public void Move()
    {
        //如果被选择了就不移动了
        if (isSelect)
        {
            return;
        }
        Transform player = GameSceneManager.instance.player.transform;
        Vector2 x = GameSceneManager.instance.planeOffsetX;
        Vector2 y = GameSceneManager.instance.planeOffsetY;

        Vector3 newPos;
        while (true)
        {
            newPos = new Vector3(Random.Range(x.x + 1, x.y - 1), Random.Range(y.x + 1, y.y - 1));
            float angle = Mathf.Acos(Vector3.Dot((newPos - player.position).normalized, player.up)) * Mathf.Rad2Deg;
            float distance = Vector3.Distance(newPos, player.position);

            //检测会不会跳玩家的脸
            if (distance < 0.5f)
            {
                continue;
            }

            //检测旁边有没有临近的物体
            if (Physics2D.OverlapCircle(newPos, 1f))
            {
                continue;
            }

            if (PlayerController.instance != null)
            {
                if (distance > PlayerController.instance.distance || angle > PlayerController.instance.lookAngle / 2 + 15)
                {
                    break;
                }
            }
            else
            {
                if (distance > 2.5f || angle > 50)
                {
                    break;
                }
            }
            
        }

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        transform.position = newPos;
    }
    /// <summary>
    /// 在player视野时候被player调用（被定住）
    /// </summary>
    public void BeRoot()
    {
        if (!isSelect)
        {
            moveTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 被选择时候被player调用
    /// </summary>
    public void BeSelect()
    {
        //把之前的动画清除
        if (sequence != null)
        {
            Color color = m_spriteRenderer.color;
            sequence.onComplete = null;//清空回调函数
            sequence.Kill(true);
            m_spriteRenderer.color = color;
        }

        //变为高亮
        m_spriteRenderer.DOColor(highLight, 0.2f);

        isSelect = true;
        //加入新的动画
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(transform.lossyScale * 0.9f, 0.15f));
        sequence.Append(transform.DOScale(transform.lossyScale * 1.1f, 0.15f));
        sequence.Append(transform.DOScale(transform.lossyScale * 1.0f, 0.15f));
        sequence.Append(m_spriteRenderer.DOColor(black, 10f).SetEase(Ease.InCirc));

        sequence.onComplete += AniCallBack;
    }

    /// <summary>
    /// 选错时动画
    /// </summary>
    public void BeSelectWrong()
    {
        transform.DOShakeRotation(0.5f, new Vector3(0, 0, 45), 10, 90);
    }

    /// <summary>
    /// 点击高亮动画结束后回调函数
    /// </summary>
    public void AniCallBack()
    {
        isSelect = false;
        //立马一次闪现(如果不在范围内）
        moveTimer = 2.9f;
        //清除select
        PlayerController.instance.select1 = null;
    }
    public void Exit()
    {
        //把之前的动画清除
        if (sequence != null)
        {
            Color color = m_spriteRenderer.color;
            sequence.onComplete = null;//清空回调函数
            sequence.Kill(true);
            m_spriteRenderer.color = color;
        }

        //变为高亮
        sequence = DOTween.Sequence();//重置
        sequence.Append(m_spriteRenderer.DOColor(highLight, 0.1f));
        sequence.Append(m_spriteRenderer.DOColor(black, 0.1f)).SetEase(Ease.InCirc);
        transform.DOMove(PlayerController.instance.transform.position, 0.2f);
        transform.DOScale(0, 0.2f);

        sequence.onComplete += () => {
            GameSceneManager.instance.RemoveObject(gameObject);//在列表中消失自己
            Destroy(gameObject); };
    }
}
