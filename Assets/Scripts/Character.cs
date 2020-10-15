﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 所有角色需要继承的父类
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour {

    // 角色移动速度
    [SerializeField]
    private float speed;

    protected Animator myAnimator;
    // 角色移动方向
    protected Vector2 direction;
    
    /// <summary>
    /// 角色刚体
    /// </summary>
    private Rigidbody2D myRigidbody;
    
    /// <summary>
    /// 标记是否正在攻击
    /// </summary>
    protected bool isAttacking = false;
    
    /// <summary>
    /// 协同攻击
    /// </summary>
    protected Coroutine attackRoutine;

    /// <summary>
    /// 标记是否移动
    /// </summary>
    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

    [SerializeField]
    protected Transform hitBox;
    
    [SerializeField]
    protected Stat health;

    /// <summary>
    /// 初始生命值
    /// </summary>
    [SerializeField]
    private float initHealth;

    
    protected virtual void Start(){
        health.Initialize(initHealth, initHealth);
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // 标记为virtual,在子类中重载（也可以不重载）
    protected virtual void Update ()
    {
        HandleLayers();
    }

    protected void FixedUpdate()
    {
        Move();
    }

    // 角色移动
    public void Move()
    {
        myRigidbody.velocity = direction.normalized * speed;
    }
    /// <summary>
    /// 正确调用动画类型
    /// </summary>
    public void HandleLayers()
    {
        // 判断是否正在移动
        if (IsMoving)
        {
            ActivateLayer("WalkLayer");

            // 控制角色朝向
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);

            StopAttack();
        }
        else if (isAttacking)
        {
            ActivateLayer("AttackLayer");
        }
        else
        {
            // 返回静止状态
            ActivateLayer("IdleLayer");
        }
    }

    /// <summary>
    /// 根据字符串调用动画类型
    /// </summary>
    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName),1);
    }
    /// <summary>
    /// 停止攻击
    /// </summary>
    public void StopAttack()
    {
        isAttacking = false; // 停止攻击
        myAnimator.SetBool("attack", isAttacking); // 停止攻击动画

        if (attackRoutine != null) // 检测是否有攻击协同
        {
            StopCoroutine(attackRoutine);
        }

    }
    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0)
        {
            myAnimator.SetTrigger("die");
        }
    }
}
