using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 萨满人偶
/// </summary>
public class ShamaDoll : MonoBehaviour
{
    private static ShamaDoll instance;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isMove;
    private float moveSpeed;
    private Vector2 targetPosition;
    private SpriteRenderer sprite;
    private List<bool> readyArray;
    private Shama _shama;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        moveSpeed = 1f;
        sprite = GetComponent<SpriteRenderer>();
        instance = this;
    }

    public static ShamaDoll Instance => instance;
    public Animator Animator => animator;

    private void Update()
    {
        if (isMove)
        {
            Move();
        }
    }

    public void Position(Vector2 vector2)
    {
        rb.position = vector2;
    }

    public void MoveTowards(Vector2 vector2,float speed,List<bool> readyArray,Shama shama)
    {
        if (!isMove&&!animator.GetBool("Move")&&!animator.GetBool("DoAction"))
        {
            moveSpeed = speed;
            targetPosition = vector2;
            isMove = true;
            this.readyArray = readyArray;
            _shama = shama;
        }
    }
    public void MoveTowards(Vector2 vector2,float speed)
    {
        if (!isMove)
        {
            animator.SetBool("Move",true);
            moveSpeed = speed;
            targetPosition = vector2;
            isMove = true;
        }
    }
    private void Move()
    {
        animator.SetFloat("X",Player.Instance.ControlledChaState.PlayerPosition.x-rb.position.x);
        Position(Vector2.MoveTowards(rb.position,targetPosition,moveSpeed*Time.deltaTime));
        if (rb.position==targetPosition)
        {
            if (readyArray!=null)
            {
                readyArray.Add(true);
                _shama.TryDollActive();
            }
            animator.SetBool("Move",false);
            isMove = false;
        }
        if (!SpriteRenderer.enabled)
        {
            SpriteRenderer.enabled= true;
        }
    }
    public SpriteRenderer SpriteRenderer => sprite;

    public void Active(bool active)
    {
        animator.SetBool("DoAction",active);
    }
    
}
