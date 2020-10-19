using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class Enemy : Character, IInteractable
{
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

    /// <summary>
    /// 血条画布
    /// </summary>
    [SerializeField]
    private CanvasGroup healthGroup;

    /// <summary>
    /// 敌人当前状态
    /// </summary>
    private IState currentState;

    [SerializeField]
    private LootTable lootTable;

    [SerializeField]
    private AStar astar;

    [SerializeField]
    protected int damage;

    private bool canDoDamage = true;

    /// <summary>
    /// 敌人的攻击距离
    /// </summary>
    [SerializeField]
    private float attackRange;
  

    /// <summary>
    /// 距离上次攻击过去的时间
    /// </summary>
    public float MyAttackTime { get; set; }

    public Vector3 MyStartPosition { get; set; }

    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }

    [SerializeField]
    private float initAggroRange;

    public float MyAggroRange { get; set; }

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.transform.position) < MyAggroRange;
        }
    }

    public AStar MyAstar
    {
        get
        {
            return astar;
        }
    }
    /// <summary>
    /// 敌人攻击范围
    /// </summary>
    public float MyAttackRange
    {
        get
        {
            return attackRange;
        }

        set
        {
            attackRange = value;
        }
    }

    protected void Awake()
    {
        health.Initialize(initHealth, initHealth);
        SpriteRenderer sr;
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        ChangeState(new IdleState());
    }

    protected override void Start()
    {
        base.Start();
        MyAnimator.SetFloat("y", -1);
    }

    protected override void Update()
    {
        if (IsAlive)
        {

            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();

            if (MyTarget != null && !Player.MyInstance.IsAlive)
            {
                ChangeState(new EvadeState());
            }
        }

        base.Update();

    }

    /// <summary>
    /// 当敌人被选中
    /// </summary>
    public Character Select()
    {
        //显示血条
        healthGroup.alpha = 1;

        return this;
    }

    /// <summary>
    /// 当敌人被取消选中
    /// </summary>
    public void DeSelect()
    {
        //隐藏
        healthGroup.alpha = 0;

        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);

        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
  
    }

    
    public override void TakeDamage(float damage, Character source)
    {
        if (!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);

                base.TakeDamage(damage, source);

                OnHealthChanged(health.MyCurrentValue);

                if (!IsAlive)
                {
                    Player.MyInstance.MyAttackers.Remove(this);
                    Player.MyInstance.GainXP(XPManager.CalculateXP((this as Enemy)));
                }
            }

        }

    }

    public void DoDamage()
    {
        if (canDoDamage)
        {
            MyTarget.TakeDamage(damage, this);
            canDoDamage = false;
        }
      
    }

    public void CanDoDamage()
    {
        canDoDamage = true;
    }

    /// <summary>
    /// 改变敌人的状态
    /// </summary>
    /// <param name="newState">新状态</param>
    public void ChangeState(IState newState)
    {
        if (currentState != null)  // 确保状态不为空
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void SetTarget(Character target)
    {
        if (MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public void Interact()
    {
        if (!IsAlive)
        {
            List<Drop> drops = new List<Drop>();

            foreach (IInteractable interactable in Player.MyInstance.MyInteractables)
            {
                if (interactable is Enemy && !(interactable as Enemy).IsAlive)
                {
                    drops.AddRange((interactable as Enemy).lootTable.GetLoot());
                }
            }

            LootWindow.MyInstance.CreatePages(drops);

        }
    }

    public void StopInteract()
    {
        LootWindow.MyInstance.Close();
    }

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }

    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }

        Destroy(gameObject);
    }
}
