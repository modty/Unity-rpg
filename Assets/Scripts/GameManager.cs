using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    
    /// <summary>
    /// 角色引用
    /// </summary>
    [SerializeField]
    private Player player;
	
	void Update ()
    {
        ClickTarget();
	}

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
        {
            // 在鼠标点击处向游戏世界发射射线
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,Mathf.Infinity,512);
            if (hit.collider != null) // 如果碰撞到物体
            {
                if (hit.collider.tag == "Enemy") // 如果碰撞物体是敌人
                {
                    // 将其设置为目标
                    player.MyTarget = hit.transform.GetChild(0);
                }
               
            }
            else
            {
                // 取消目标
                player.MyTarget = null;
            }
        }
   
    }
}
