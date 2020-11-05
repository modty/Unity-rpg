using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 torwadsPosition;
    public float currentSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        preTime = Time.time;
        InvokeRepeating(nameof(doWeapon),0,.01f);
    }

    private bool start=true;
    private bool isBack=false;
    private bool isAwake;
    private float preTime;
    
    private float newTime;

    private float frameTimes;

    private Vector2 MousePosition()
    {
        Vector2 vector2 = rb.position;
        Vector2 vector2Temp=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vector2 = vector2Temp - vector2;
        return vector2;
    }
    public Vector2 getTowardsPosition(Vector2 currentPosition)
    {
        Vector2 vector2 = MousePosition();
        double c = Math.Atan2(vector2.y, vector2.x);
        return new Vector2((float) (Constants.WeaponThrowDistance*Math.Cos(c)),(float) (Constants.WeaponThrowDistance*Math.Sin(c)));
    }

    public float calculateDistance()
    {
        newTime = Time.time;
        float times=newTime-preTime;
        float v1 = -1;
        if (isBack)
        {
            v1=currentSpeed + Constants.WeaponThrowAcceleration * times;
        }
        else
        {
            v1=currentSpeed - Constants.WeaponThrowAcceleration * times;
        }
        float distance = (currentSpeed + v1) * times / 2;
        currentSpeed = v1;
        preTime = newTime;
        return distance;
    }

    public void doWeapon()
    {
        if (start)
        {
            torwadsPosition=getTowardsPosition(rb.position);
            currentSpeed = Constants.WeaponThrowMaxSpeed;
            preTime = Time.time;
            start = false;
        }
        transform.Rotate(0, 0, Constants.WeaponThrowRotateSpeed * Time.deltaTime);
        if (isBack)
        {
            rb.position = Vector2.MoveTowards(rb.position,PlayerState.Instance.PlayerPosition, .05f);
            if (rb.position == PlayerState.Instance.PlayerPosition)
            {
                isBack = false;
                start = true;
                gameObject.SetActive(false);
            }
        }
        else
        {
            rb.position=Vector2.MoveTowards(rb.position,torwadsPosition,.05f);
            if (rb.position == torwadsPosition)
            {
                isBack = true;
            }
        }
    }
}
