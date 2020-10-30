using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlaneScript : MonoBehaviour
{
    [SerializeField]private GameObject[] selects;
    [SerializeField]private Image[] icons;
    [SerializeField]private Camera mainCam;
    [SerializeField]private RectTransform rod;
    [SerializeField]private RectTransform parent;
    void Update()
    {
        Vector2 v2 = Input.mousePosition - transform.position;
        float angle=(float) (Math.Atan2(v2.y, v2.x)*180/Math.PI);
        float distance = (float) Math.Sqrt(v2.x * v2.x + v2.y * v2.y);
        float width = distance < 90 ? distance : 90;
        Vector3 scale = parent.localScale;
        rod.sizeDelta=new Vector2(width*scale.x,(float) (width/15.3)*scale.y);
        rod.localEulerAngles =new Vector3(0f,0f,angle);
        // 中
        if (distance <= 30*scale.x)
        {
            SelectActive(4);
        }
        else
        {
            // 上
            if(angle>=45&&angle<135)
            {
                SelectActive(0);
            }
            // 右
            else if (angle<45 &&angle >=-45)
            {
                SelectActive(1);
            } 
            // 下
            else if(angle>=-135&&angle<-45)
            {
                SelectActive(2);
            }
            // 左
            else if(angle>=135||angle<-135)
            {
                SelectActive(3);
            }
        }
    }

    private void SelectActive(int index)
    {
        for (int i = 0; i < selects.Length; i++)
        {
            if (i==index)
            {
                selects[i].SetActive(true);
            }
            else
            {
                selects[i].SetActive(false);
            }
        }
    }
}
