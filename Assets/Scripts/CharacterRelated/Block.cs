using System;
using UnityEngine;
// 用于分割块的类
[Serializable]
public class Block
{
    // 一系列的方块
    [SerializeField]
    private GameObject first, second;

    /// <summary>
    /// 使方块失效
    /// </summary>
    public void Deactivate()
    {
        first.SetActive(false);
        second.SetActive(false);
    }

    /// <summary>
    /// 激活方块
    /// </summary>
    public void Activate()
    {
        first.SetActive(true);
        second.SetActive(true);
    }
}
