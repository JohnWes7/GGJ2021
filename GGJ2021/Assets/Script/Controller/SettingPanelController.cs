﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelController : MonoBehaviour
{
    public void Back()
    {
        transform.parent.DOLocalMoveX(0, 0.2f).OnComplete(() => { Destroy(gameObject); });
    }
}
