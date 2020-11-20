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
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        moveSpeed = 0.01f;
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

    public void MoveTowards(Vector2 vector2)
    {
        if (!isMove)
        {
            targetPosition = vector2;
            isMove = true;
        }
    }

    private void Move()
    {
        Position(Vector2.MoveTowards(rb.position,targetPosition,moveSpeed));
    }

    public SpriteRenderer SpriteRenderer => sprite;
}
