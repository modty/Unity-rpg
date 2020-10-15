using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    /// <summary>
    /// 角色图片的引用
    /// </summary>
    private SpriteRenderer parentRenderer;

    //一系列碰撞物体
    private List<Obstacle> obstacles = new List<Obstacle>();

	void Start ()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
	}
	
    /// <summary>
    /// 当碰撞产生
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle") 
        {
            // 获取碰撞对象引用
            Obstacle o = collision.GetComponent<Obstacle>();

            o.FadeOut();
            
            //如果没有设置碰撞对象或者说碰撞到低层数的物体
            if (obstacles.Count == 0 || o.MySpriteRenderer.sortingOrder -1 < parentRenderer.sortingOrder)
            {
                //将碰撞对象隐藏在角色后
                parentRenderer.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;
            }

            // 将碰撞对象加入到数组中，方便跟踪
            obstacles.Add(o);
        }
        
    }

    /// <summary>
    /// 当退出碰撞区域
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
       if (collision.tag == "Obstacle")
        {
            Obstacle o = collision.GetComponent<Obstacle>();

            o.FadeIn();
            
            // 将其从列表中移除
            obstacles.Remove(o);

            //没有碰撞的物体后
            if (obstacles.Count == 0)
            {
                parentRenderer.sortingOrder = 200;
            }
            else// 如果还有碰撞的物体。刷新
            {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;
            }
          
        }

     
    }
}
