using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 所有角色需要继承的父类
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour {

    // 角色移动速度
    [SerializeField]
    private float speed;

    public Animator MyAnimator { get; set; }
    // 角色移动方向
    protected Vector2 direction;
    
    /// <summary>
    /// 角色刚体
    /// </summary>
    private Rigidbody2D myRigidbody;
    
    /// <summary>
    /// 标记是否正在攻击
    /// </summary>
    public bool IsAttacking { get; set; }
    
    [SerializeField]
    protected Transform hitBox;
    
    [SerializeField]
    protected Stat health;
    
    public Transform MyTarget { get; set; }

    /// <summary>
    /// 协同攻击
    /// </summary>
    protected Coroutine attackRoutine;
    public Stat MyHealth
    {
        get { return health; }
    }
    /// <summary>
    /// 初始生命值
    /// </summary>
    [SerializeField]
    private float initHealth;

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
    public Vector2 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }
    public bool IsAlive
    {
        get
        {
            return  health.MyCurrentValue > 0;
        }
    }
    
    protected virtual void Start(){
        health.Initialize(initHealth, initHealth);
        myRigidbody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
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
        if (IsAlive)
        {
            myRigidbody.velocity = direction.normalized * speed;

        }
    }
    /// <summary>
    /// 正确调用动画类型
    /// </summary>
    public void HandleLayers()
    {
        if (IsAlive)
        {
            // 判断是否正在移动
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");

                // 控制角色朝向
                MyAnimator.SetFloat("x", direction.x);
                MyAnimator.SetFloat("y", direction.y);
            }
            else if (IsAttacking)
            {
                ActivateLayer("AttackLayer");
            }
            else
            {
                // 返回静止状态
                ActivateLayer("IdleLayer");
            }
        }else{
            ActivateLayer("DeathLayer");
        }
    }

    /// <summary>
    /// 根据字符串调用动画类型
    /// </summary>
    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName),1);
    }
    public virtual void TakeDamage(float damage, Transform source)
    {
        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigidbody.velocity = Direction;
            MyAnimator.SetTrigger("die");
        }
    }
}
