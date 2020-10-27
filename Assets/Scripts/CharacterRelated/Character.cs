using Assets.Scripts.Debuffs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// 所有角色需要继承的父类
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{

    /// <summary>
    /// 角色目标移动速度
    /// </summary>
    [SerializeField]
    private float speed;
    /// <summary>
    /// 当前移动速度
    /// </summary>
    private float currentSpeed;
    /// <summary>
    /// 物品类型（作为唯一标签）
    /// </summary>
    [SerializeField]
    private string type;
    /// <summary>
    /// 物品名
    /// </summary>
    [SerializeField]
    private string title;
    /// <summary>
    /// 物品等级
    /// </summary>
    [SerializeField]
    private int level;

    /// <summary>
    /// 角色的动画控制器
    /// </summary>
    public Animator Animator { get; set; }

    public Transform CurrentTile { get; set; }

    public SpriteRenderer SpriteRenderer { get; set; }

    /// <summary>
    /// 角色朝向
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// 角色刚体
    /// </summary>
    [FormerlySerializedAs("rigidbody")] [SerializeField]
    private Rigidbody2D rigidbody;

    /// <summary>
    /// 标记是否正在攻击
    /// </summary>
    public bool IsAttacking { get; set; }

    /// <summary>
    /// 攻击协同（新线程）
    /// </summary>
    protected Coroutine actionRoutine;

    [SerializeField]
    private Transform hitBox;

    [SerializeField]
    protected Stat health;

    public Character Target { get; set; }

    public Stack<Vector3> Path { get; set; }

    public List<Debuff> Debuffs { get; set; } = new List<Debuff>();

    private List<Debuff> newDebuffs = new List<Debuff>();

    private List<Debuff> expiredDebuffs = new List<Debuff>();

    public Stat Health
    {
        get { return health; }
    }

    /// <summary>
    /// 初始生命值
    /// </summary>
    [SerializeField]
    protected float initHealth;

    /// <summary>
    /// 标记是否移动
    /// </summary>
    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
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

    public float CurrentSpeed
    {
        get
        {
            return currentSpeed;
        }

        set
        {
            currentSpeed = value;
        }
    }

    public bool IsAlive
    {
        get
        {
          return  health.CurrentValue > 0;
        }
    }

    public string Type
    {
        get
        {
            return type;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public Rigidbody2D Rigidbody
    {
        get
        {
            return rigidbody;
        }
    }

    public Transform Hitbox
    {
        get
        {
            return hitBox;
        }

        set
        {
            hitBox = value;
        }
    }

    public float Speed { get => speed; private set => speed = value; }

    protected virtual void Start()
    {
        currentSpeed = Speed;
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 标记为virtual,在子类中重载（也可以不重载）
    /// </summary>
    protected virtual void Update ()
    {
        HandleLayers();

        HandleDebuffs();
	}

    public void FixedUpdate()
    {
        Move();
    }


    /// <summary>
    /// 角色移动
    /// </summary>
    public void Move()
    {
        if (Path == null)
        {
            if (IsAlive)
            {
                Rigidbody.velocity = Direction.normalized * CurrentSpeed;
            }
        }
    }
    /// <summary>
    /// 正确调用动画类型
    /// </summary>
    /// <returns></returns>
    public virtual void HandleLayers()
    {
        if (IsAlive)
        {
            // 判断是否正在移动
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");

                // 控制角色朝向
                Animator.SetFloat("x", Direction.x);
                Animator.SetFloat("y", Direction.y);
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
        }
        else
        {
            ActivateLayer("DeathLayer");
        }

    }

    private void HandleDebuffs()
    {

        if (Debuffs.Count > 0)
        {
            foreach (Debuff debuff in Debuffs)
            {
                debuff.Update();
            }
        }

        if (newDebuffs.Count > 0)
        {
            Debuffs.AddRange(newDebuffs);
            newDebuffs.Clear();
        }

        if (expiredDebuffs.Count > 0)
        {
            foreach (Debuff debuff in expiredDebuffs)
            {
                Debuffs.Remove(debuff);
            }

            expiredDebuffs.Clear();
        }
    }

    public void ApplyDebuff(Debuff debuff)
    {
        // 检查是否有相同名字的Debuff
        Debuff tmp = Debuffs.Find(x => x.Name == debuff.Name);

        if (tmp != null)
        {
            // 移除旧的Debuff
            RemoveDebuff(tmp);
        }

        // 应用新的Debuff
        this.newDebuffs.Add(debuff);
    }

    public void RemoveDebuff(Debuff debuff)
    {
        UIManager.Instance.RemoveDebuff(debuff);
        this.expiredDebuffs.Add(debuff);
    }

    /// <summary>
    /// 根据字符串调用动画类型
    /// </summary>
    public virtual void ActivateLayer(string layerName)
    {
        for (int i = 0; i < Animator.layerCount; i++)
        {
            Animator.SetLayerWeight(i, 0);
        }

        Animator.SetLayerWeight(Animator.GetLayerIndex(layerName),1);
    }

    /// <summary>
    /// 角色受伤
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage, Character source)
    {
        health.CurrentValue -= damage;

        CombatTextManager.Instance.CreateText(transform.position, damage.ToString(), SCTTYPE.DAMAGE,false);

        if (health.CurrentValue <= 0)
        {
            // 确保死亡时停止移动
            Direction = Vector2.zero;
            Rigidbody.velocity = Direction;
            GameManager.Instance.OnKillConfirmed(this);
            Animator.SetTrigger("die");
        }
    }

    public void GetHealth(int health)
    {
        Health.CurrentValue += health;
        CombatTextManager.Instance.CreateText(transform.position, health.ToString(),SCTTYPE.HEAL,true);
    }

}
