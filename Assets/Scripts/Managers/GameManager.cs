using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
// 69
public class GameManager : MonoBehaviour {
    
    /// <summary>
    /// 角色引用
    /// </summary>
    [SerializeField]
    private Player player;
    private NPC currentTarget;
	void Update ()
    {
        ClickTarget();
	}

    private void ClickTarget()
    {
        // 判断鼠标左键是否按下，判断是否点击到UI上
        if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
        {
            // 在鼠标点击处向游戏世界发射射线
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,Mathf.Infinity,512);
            if (hit.collider != null&&hit.collider.tag == "Enemy") // 如果碰撞到物体
            {
                if (currentTarget != null)// 如果当前有目标
                {
                    currentTarget.DeSelect(); // 取消当前选中
                }
                currentTarget = hit.collider.GetComponent<NPC>(); // 选中新物体

                player.MyTarget = currentTarget.Select(); // 给角色选中物体引用
                
                UIManager.MyInstance.ShowTargetFrame(currentTarget);
            }
            else
            {
                UIManager.MyInstance.HideTargetFrame();
                if (currentTarget != null) // 如果没有目标
                {
                    currentTarget.DeSelect(); // 取消选中
                }

                // 取消对其引用
                currentTarget = null;
                player.MyTarget = null;
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("点击");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null && (hit.collider.tag == "Enemy"|| hit.collider.tag == "Interactable")&&hit.collider.gameObject.GetComponent<IInteractable>() == player.MyInteractable)
            {
                player.Interact();
            }
        }
   
    }
}
